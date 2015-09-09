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
using System.Threading;
using System.Web;

namespace Haptic_Theatre_Vibings_Control.Classes
{
    public static class HTTPManager
    {

        static UdpClient _receiveUdpClient = new UdpClient(80);
        private static string _receivedUdpMessage = " No Messages";
        private static string currentUDPPort = "-1";


        public static string SendGetRequest(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "GET");
            var response = webRequest.GetResponse();

            return response;
        }

        public static string SendPostRequest(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "POST");
            var response = webRequest.GetResponse();

            return response;
        }

        internal static string SendUdpBroadcast(string httpRequest, string httpPortNumber)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, Convert.ToInt16(httpPortNumber));

            byte[] bytes = Encoding.ASCII.GetBytes(httpRequest);

            client.Send(bytes, bytes.Length, ip);
            client.Close();

            string response = "Broadcast: " + httpRequest;

            return response;

        }

        internal static string ReceiveUdpBroadcast(string httpPortNumber)
        {
            if (httpPortNumber != currentUDPPort)
            {
                _receiveUdpClient.Close();
                _receiveUdpClient = new UdpClient(Convert.ToInt16(httpPortNumber));

                currentUDPPort = httpPortNumber;
            }
            //_receiveUdpClient = new UdpClient(2365);
            try
            {
                _receiveUdpClient.BeginReceive(new AsyncCallback(ReceiveUdpMessages), null);
            }
            catch (Exception e)
            {
                int x = 2;
            }

            return _receivedUdpMessage;

        }

        private static void ReceiveUdpMessages(IAsyncResult res)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);
            byte[] received = _receiveUdpClient.EndReceive(res, ref RemoteIpEndPoint);

            //Process codes
            _receiveUdpClient.BeginReceive(new AsyncCallback(ReceiveUdpMessages), null);

            _receivedUdpMessage = System.Text.Encoding.UTF8.GetString(received);
        }
    }
}