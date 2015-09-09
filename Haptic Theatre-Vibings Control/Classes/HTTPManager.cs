#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\HTTPManager.cs
* ****************************************************************************************** 
 DESCRIPTION   : Controls the use of Http connections

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 30/08/2015                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Haptic_Theatre_Vibings_Control.Classes
{
    public class HTTPManager
    {
        public string SendGetRequest(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "GET");
            var response = webRequest.GetResponse();

            return response;
        }

        public string SendPostRequest(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "POST");
            var response = webRequest.GetResponse();

            return response;
        }

        internal string SendUdpBroadcast(string httpRequest, string httpPortNumber)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, Convert.ToInt16(httpPortNumber));

            byte[] bytes = Encoding.ASCII.GetBytes(httpRequest);

            client.Send(bytes, bytes.Length, ip);
            client.Close();

            string response = "Broadcast: " + httpRequest;

            return response;

        }
    }
}