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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Haptic_Theatre_Vibings_Control.Classes
{
    public static class HTTPManager
    {
        private static string _receivedUdpMessage = "No Messages";
        private static int currentUDPPort = 50002;
        static UdpClient _receiveUdpClient = new UdpClient(currentUDPPort);

        #region TCP Send

        /// <summary>
        /// Send a simple Get request
        /// </summary>
        /// <param name="ip">IP of client</param>
        /// <returns></returns>
        /// <remarks>Used port 80</remarks>
        public static string SendGetRequest(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "GET");
            var response = webRequest.GetResponse();

            return response;
        }

        public static void SendModeCommand(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "POST");
            Task.Run(() => webRequest.SendCommand());  //Async as we don't care about the response
        }


        /// <summary>
        /// Send a simple Post request
        /// </summary>
        /// <param name="ip">IP of client</param>
        /// <returns></returns>
        /// <remarks>Used port 80</remarks>
        public static string SendPostRequest(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "POST");
            var response = webRequest.GetResponse();

            return response;
        }

        #endregion


        #region  UDP

        /// <summary>
        /// Broadcast a UDP message - Used to test UDP receiver
        /// </summary>
        /// <param name="httpRequest">Content of message</param>
        /// <param name="httpPortNumber">Port to use</param>
        /// <returns>Content of response ( are we going to get a UDP resonse?)</returns>
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

        /// <summary>
        /// Waits on a port for UDP messages
        /// </summary>
        /// <param name="httpPortNumber"></param>
        /// <returns></returns>
        internal static string ReceiveUdpBroadcast(string httpPortNumber)
        {
            if (httpPortNumber != currentUDPPort.ToString() || _receiveUdpClient.Client == null)
            {
                _receiveUdpClient.Close();
                _receiveUdpClient.Dispose();
                _receiveUdpClient = new UdpClient(Convert.ToInt32(httpPortNumber));

                currentUDPPort = Convert.ToInt32(httpPortNumber);
            }
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

        /// <summary>
        /// Gets the actual UDP received messages from the buffer
        /// </summary>
        /// <param name="res"></param>
        /// <remarks>Puts the message into a global variable so ReceiveUdpBroadcast() can return it to the UI</remarks>
        private static void ReceiveUdpMessages(IAsyncResult res)
        {
            try
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);

                if (_receiveUdpClient.Client == null)
                {
                    _receiveUdpClient.Close();
                    _receiveUdpClient.Dispose();
                    _receiveUdpClient = new UdpClient(Convert.ToInt32(currentUDPPort));
                }


                if (_receiveUdpClient.Client != null)
                {
                    byte[] received = _receiveUdpClient.EndReceive(res, ref RemoteIpEndPoint);

                    _receiveUdpClient.BeginReceive(new AsyncCallback(ReceiveUdpMessages), null);

                    _receivedUdpMessage = System.Text.Encoding.UTF8.GetString(received);
                }
            }
            catch (Exception e)
            {
                int y = 4;   //race condition when client is disposed can cause problem if this thread is picking up data ay the time
            }

        }


        /// <summary>
        /// Closed the UDP client
        /// </summary>
        internal static void CancelUdpBroadcast()
        {
            _receiveUdpClient.Close();
        }

        #endregion
    }
}