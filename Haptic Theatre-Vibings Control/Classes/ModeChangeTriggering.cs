#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\TimeTriggering.cs
* ****************************************************************************************** 
 DESCRIPTION   : Manages sending mode change events.
                 
                 Modes:
                 1 uses time to determine when to send events from a dummy heartbeat monitor
                 2 Use sensor data collection to trigger changes

                 Uses xml for now.
                 Database in future
                 This class is mostly poop

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 11/04/2016                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using System;
using System.IO;
using System.Threading;
using System.Xml;
using System.Web.Hosting;
using Haptic_Theatre_Vibings_Control.Models;
using Microsoft.AspNet.SignalR;

namespace Haptic_Theatre_Vibings_Control.Classes
{
    public static class ModeChangeTriggering
    {
        public static bool ContinueToRead { get; set; } = false;
        private static readonly IHubContext SignalHub = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
        private static string _currentCommand ="";
        private static string _currentShowMode = "";
        private static string _previousShowMode = "";
        private static string _currentmultiModeName = "";
        private static string _previousmultiModeName = "";
        private static readonly HttpViewModel HttpViewModel = new HttpViewModel
        {
            HttpPortNumber = "50002",
            HttpRequestType = "Get"
        };

        #region Public Requests

        public static void StartShow()
        {
            int count = 0;

            while (ContinueToRead)
            {
                int heartBeat = GetHeartBeat(count);
                SignalHub.Clients.All.updateHeartRate(heartBeat.ToString());

                string newCommand = GetCommand(heartBeat);

                if (newCommand != _currentCommand)
                {
                    _currentCommand = newCommand;

                    SwitchDisplayByModeID(_currentShowMode);

                    SendCommand(_currentCommand);
                }

                
                Thread.Sleep(5000);
                count = count + 5;
            }
                count = 0;
        }

        private static int GetHeartBeat(int count)
        {
            XmlDocument heartRatesXML = LoadHeartRate();
            int heartBeat = GetHeartRateForThisCount(heartRatesXML, count);


            return heartBeat;
        }

        private static int GetHeartRateForThisCount(XmlDocument heartRatesXML, int count)
        {
            string showMode = "";
            int heartRate = 0;

            XmlNodeList nodeList = heartRatesXML.GetElementsByTagName("TimeCode");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode countXmlNode = heartRatesXML.GetElementsByTagName("count")[i];

                if (count == Convert.ToInt32(countXmlNode.InnerXml))
                {
                    XmlNode heartRateXMLNode = heartRatesXML.GetElementsByTagName("HeartRate")[i];
                    heartRate = Convert.ToInt32(heartRateXMLNode.InnerXml);

                }
            }
            return heartRate;
        }

        public static void ChangeModeByShowModeID(string showModeID)
        {
            XmlDocument commandsXML = LoadCommands();
            string command = GetCommandForThisShowMode(commandsXML, showModeID);
            SendCommand(command);
            SwitchDisplayByModeID(showModeID);
        }

        #endregion

        #region Sensor triggering

        /*
            Add method to recieve _averageSensorValue from SensorProcessing
            Create new Trigger_Sensor.xml
            That generates a show mode so use code below to get and send command from Command.XML
            Use ShowMode201 etc
         */

        public static void SwitchDisplayBySensorValue(Decimal sensorValue)
        {
            string newCommand = GetCommand(sensorValue);

            if (newCommand != _currentCommand)
            {
                _currentCommand = newCommand;

                SwitchDisplayByModeID(_currentShowMode);

                SendCommand(_currentCommand);
            }
        }

        #endregion

        #region UI

        private static void SwitchDisplayByModeID(string showModeID)
        {
            _currentShowMode = showModeID;
            SignalHub.Clients.All.setModeNotActive(_previousShowMode);
            SignalHub.Clients.All.setModeActive(_currentShowMode);
            _previousShowMode = _currentShowMode;
        }

        #endregion

        #region Network

        private static void SendCommand(string currentCommand)
        {
            XmlDocument IPsXML = LoadIPs();
            XmlNodeList nodeList = IPsXML.GetElementsByTagName("IP");

            foreach (XmlNode node in  nodeList)
            {
                string ip = node.InnerText;
                HttpViewModel.HttpRequest = ip + currentCommand;
                HTTPManager.SendModeCommand(HttpViewModel.HttpRequest);
            }
        }

        #endregion

        #region Get Commands

        /// <summary>
        /// Uses dummy heartbeat mode
        /// </summary>
        /// <param name="heartBeat"></param>
        /// <returns></returns>
        static string GetCommand(int heartBeat)
        {
            string command = _currentCommand;

            XmlDocument triggersXML =  LoadHeartBeatTriggers();
            _currentShowMode = GetShowModeForThisHeartRate(triggersXML, heartBeat);

            SignalHub.Clients.All.updateShowMode(_currentShowMode);
            SignalHub.Clients.All.setModeActive(_currentShowMode);

            if (_currentShowMode.Contains("MultiMode"))
            {
                if (_currentShowMode != _previousShowMode)
                {
                    XmlDocument multiModesXML = LoadMultiModes();
                    SendAllCommandsForThisMultiMode(multiModesXML, _currentShowMode);
                    _previousShowMode = _currentShowMode;
                }
            }
            else
            {
                XmlDocument commandsXML = LoadCommands();

                command = GetCommandForThisShowMode(commandsXML, _currentShowMode);                
            }
            
            return command;
        }

        /// <summary>
        /// Uses Acc Sensor mode
        /// </summary>
        /// <param name="sensorValue"></param>
        /// <returns></returns>
        static string GetCommand(Decimal sensorValue)
        {
            XmlDocument triggersXML = LoadSensorTriggers();
            _currentShowMode = GetShowModeForThisSensorValue(triggersXML, sensorValue);

            XmlDocument commandsXML = LoadCommands();

            string command = GetCommandForThisShowMode(commandsXML, _currentShowMode);

            return command;
        }


        static string SendAllCommandsForThisMultiMode(XmlDocument multiModesXML, string multiModeName)
        {
            XmlDocument commandsXML = LoadCommands();
            XmlNodeList nodeList = multiModesXML.GetElementsByTagName("MultiMode");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlAttributeCollection xmlAttributeCollection = nodeList[i].Attributes;

                if (xmlAttributeCollection["Name"].Value == multiModeName)
                {
                    int delay = Convert.ToInt32(xmlAttributeCollection["Delay"].Value);
                    XmlNodeList showModeNodes = nodeList[i].ChildNodes;
                    foreach (XmlNode showModeNode in showModeNodes)
                    {
                        string showMode = showModeNode.InnerText;
                        var command = GetCommandForThisShowMode(commandsXML, showMode);
                        SendCommand(command);
                        Thread.Sleep(delay);
                    }
                    break;
                }
            }
            return _currentCommand;  //acts to prevent main loop sending a command
        }

        static string GetShowModeForThisHeartRate(XmlDocument triggersXML, int heartRate)
        {
            string showMode = "";

            XmlNodeList nodeList = triggersXML.GetElementsByTagName("Trigger");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode heartRateLower = triggersXML.GetElementsByTagName("HeartRateLower")[i];

                if (heartRate >= Convert.ToInt32(heartRateLower.InnerXml))
                {
                    XmlNode heartRateUpper = triggersXML.GetElementsByTagName("HeartRateUpper")[i];
                    if (heartRate <= Convert.ToInt32(heartRateUpper.InnerXml))
                    {
                        XmlElement ShowMode = (XmlElement)triggersXML.GetElementsByTagName("ShowMode")[i];
                        showMode = ShowMode.InnerText;
                        break;
                    }
                   
                }
            }
            return showMode;
        }

        static string GetShowModeForThisSensorValue(XmlDocument triggersXML, Decimal sensorValue)
        {
            string showMode = "";

            XmlNodeList nodeList = triggersXML.GetElementsByTagName("Trigger");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode heartRateLower = triggersXML.GetElementsByTagName("SensorValueLower")[i];

                if (sensorValue >= Convert.ToInt32(heartRateLower.InnerXml))
                {
                    XmlNode heartRateUpper = triggersXML.GetElementsByTagName("SensorValueUpper")[i];
                    if (sensorValue <= Convert.ToInt32(heartRateUpper.InnerXml))
                    {
                        XmlElement ShowMode = (XmlElement)triggersXML.GetElementsByTagName("ShowMode")[i];
                        showMode = ShowMode.InnerText;
                        break;
                    }

                }
            }
            return showMode;
        }

        static string GetCommandForThisShowMode(XmlDocument triggersXML, string showModeName)
        {
            string command = "";

            XmlNodeList nodeList = triggersXML.GetElementsByTagName("ShowMode");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode nameElementNode = triggersXML.GetElementsByTagName("Name")[i];

                if (nameElementNode.InnerText == showModeName)
                {
                    XmlElement commandElementNode = (XmlElement)triggersXML.GetElementsByTagName("Command")[i];
                    command = commandElementNode.InnerText;
                    break;
                }
            }
            return command;
        }

        public static void ReadTriggers()
        {
            var signalHub = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
            Random random = new Random();
            while (ContinueToRead)
            {
                signalHub.Clients.All.updateHeartRate(random.Next(50, 90).ToString());  //send

                //string timeToTrigger = GetlatestTrigger(); 
                Thread.Sleep(1000);
            }
        }

        #endregion

        #region Read database

        private static XmlDocument LoadHeartRate()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile = HostingEnvironment.MapPath("~/Database/HeartRates.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        private static XmlDocument LoadHeartBeatTriggers()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile = HostingEnvironment.MapPath("~/Database/Triggers.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        private static XmlDocument LoadSensorTriggers()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile =
                @"E:\Data\My Documents\Dropbox\Projects\Haptic Theatre\Haptic Controller\Haptic-Theatre-Vibings-Control\Haptic Theatre-Vibings Control\bin\Database/Triggers_Aceleration_Sensor.xml";
            //var dataFile = HostingEnvironment.MapPath("~/Database/Triggers_Aceleration_Sensor.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        private static XmlDocument LoadCommands()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile =
                @"E:\Data\My Documents\Dropbox\Projects\Haptic Theatre\Haptic Controller\Haptic-Theatre-Vibings-Control\Haptic Theatre-Vibings Control\bin\Database/Commands.xml";
            //var dataFile = HostingEnvironment.MapPath("~/Database/Commands.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        private static XmlDocument LoadIPs()
        {
            XmlDocument xdoc = new XmlDocument();
            //var dataFile = HostingEnvironment.MapPath("~/Database/IPs.xml");
            var dataFile = @"E:\Data\My Documents\Dropbox\Projects\Haptic Theatre\Haptic Controller\Haptic-Theatre-Vibings-Control\Haptic Theatre-Vibings Control\bin\Database/IPs.xml";
            //var dataFile = HostingEnvironment.MapPath("~/Database/IPs.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        private static XmlDocument LoadMultiModes()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile = HostingEnvironment.MapPath("~/Database/MultiModes.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        #endregion
    }
}