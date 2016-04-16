#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\TimeTriggering.cs
* ****************************************************************************************** 
 DESCRIPTION   : Manages sending mode change events.
                 Uses time to determine when to send events

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
        private static HttpViewModel _httpViewModel = new HttpViewModel
        {
            HttpPortNumber = "50002",
            HttpRequestType = "Get"
        };

        #region Public Requests

        public static void StartShow()
        {
            int heartBeat = 50;

            while (ContinueToRead)
            {
                string newCommand = GetCommand(heartBeat);
                if (newCommand != _currentCommand)
                {
                    _currentCommand = newCommand;

                    SwitchDisplayByModeID(_currentShowMode);

                    SendCommand(_currentCommand);
                }

                SignalHub.Clients.All.updateHeartRate(heartBeat.ToString());
                SignalHub.Clients.All.setModeActive(_currentShowMode);
                
                Thread.Sleep(2000);
                heartBeat++;
            }
        }

        public static void ChangeModeByShowModeID(string showModeID)
        {
            XmlDocument commandsXML = LoadCommands();
            string command = GetCommandForThisShowMode(commandsXML, showModeID);
            SendCommand(command);

            Thread.Sleep(2000);

            SwitchDisplayByModeID(showModeID);
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
                _httpViewModel.HttpRequest = ip + currentCommand;
                HTTPManager.SendGetCommand(_httpViewModel.HttpRequest);
            }
        }

        #endregion

        #region Get Commands

        static string GetCommand(int heartBeat)
        {
            XmlDocument triggersXML =  LoadTriggers();
            _currentShowMode = GetShowModeForThisHeartRate(triggersXML, heartBeat);
            
            XmlDocument commandsXML = LoadCommands();

            string command = GetCommandForThisShowMode(commandsXML, _currentShowMode);

            return command;
        }

        static string GetShowModeForThisHeartRate(XmlDocument triggersXML, int heartBeat)
        {
            string showMode = "";

            XmlNodeList nodeList = triggersXML.GetElementsByTagName("Trigger");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode heartRateLower = triggersXML.GetElementsByTagName("HeartRateLower")[i];

                if (heartBeat >= Convert.ToInt32(heartRateLower.InnerXml))
                {
                    XmlNode heartRateUpper = triggersXML.GetElementsByTagName("HeartRateUpper")[i];
                    if (heartBeat <= Convert.ToInt32(heartRateUpper.InnerXml))
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
                    XmlElement commaElementNode = (XmlElement)triggersXML.GetElementsByTagName("Command")[i];
                    command = commaElementNode.InnerText;
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

        private static XmlDocument LoadTriggers()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile = HostingEnvironment.MapPath("~/Database/Triggers.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        private static XmlDocument LoadCommands()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile = HostingEnvironment.MapPath("~/Database/Commands.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        private static XmlDocument LoadIPs()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile = HostingEnvironment.MapPath("~/Database/IPs.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }

        #endregion
    }
}