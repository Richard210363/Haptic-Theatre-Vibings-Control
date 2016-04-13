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
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;

namespace Haptic_Theatre_Vibings_Control.Classes
{
    public static class ModeChangeTriggering
    {
        public static bool continueToRead = false;
        private static IHubContext signalHub = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
        private static string currentModeCommand ="";

        public static void StartShow_Dummy()
        {
            int heartBeat = 50;

            while (continueToRead)
            {
                string newModeCommand = GetModeCommand(heartBeat);
                if (newModeCommand != currentModeCommand)
                {
                    //SetMode
                    currentModeCommand = newModeCommand;
                    int s = 2;
                }

                signalHub.Clients.All.updateHeartRate(heartBeat.ToString());
                Thread.Sleep(2000);
                heartBeat++;
            }
        }

        static string GetModeCommand(int heartBeat)
        {
            XmlDocument triggersXML =  LoadTriggers();
            string modeCommand = GetModeCommandForThisHeartRate(triggersXML, heartBeat);

            return modeCommand;
        }

        static string GetModeCommandForThisHeartRate(XmlDocument triggersXML, int heartBeat)
        {
            string modeCommand = "";

            XmlNodeList nodeList = triggersXML.GetElementsByTagName("Trigger");
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode HeartRateLower = triggersXML.GetElementsByTagName("HeartRateLower")[i];

                if (heartBeat >= Convert.ToInt32(HeartRateLower.InnerXml))
                {
                    XmlNode HeartRateUpper = triggersXML.GetElementsByTagName("HeartRateUpper")[i];
                    if (heartBeat <= Convert.ToInt32(HeartRateUpper.InnerXml))
                    {
                        XmlElement command = (XmlElement)triggersXML.GetElementsByTagName("Command")[i];
                        modeCommand = command.InnerText;
                        break;
                    }
                   
                }
            }
            return modeCommand;
        }


        public static void ReadTriggers()
        {
            var signalHub = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
            Random random = new Random();
            while (continueToRead)
            {
                signalHub.Clients.All.updateHeartRate(random.Next(50, 90).ToString());  //send

                //string timeToTrigger = GetlatestTrigger(); 
                Thread.Sleep(1000);
            }
        }

        private static XmlDocument LoadTriggers()
        {
            XmlDocument xdoc = new XmlDocument();
            var dataFile = HostingEnvironment.MapPath("~/Database/TimeTriggers.xml");
            FileStream fileStream = new FileStream(dataFile, FileMode.Open);
            xdoc.Load(fileStream);
            fileStream.Close();
            return xdoc;
        }
    }
}