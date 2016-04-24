#region File Information

/********************************************************************************************
* Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\SendWebRequest.cs
* ****************************************************************************************** 
 DESCRIPTION   : Sends a web request and gets the response

 usage:

             WebRequest webRequest = new WebRequest(ip, "POST", "a=value1&b=value2");
            var x = webRequest.GetResponse();

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 30/08/2015                 Richard Byrne          Created 
**********************************************************************************************/

#endregion




using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Haptic_Theatre_Vibings_Control.Classes
{
    public class WebRequest
    {
        private readonly System.Net.WebRequest request;
        private Stream _dataStream;
        private string _status;

        public String Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public WebRequest(string url)
        {
            request = System.Net.WebRequest.Create(url);
        }

        public WebRequest(string url, string method)
            : this(url)
        {

            if (method.Equals("GET") || method.Equals("POST"))
            {
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        public WebRequest(string url, string method, string data)
            : this(url, method)
        {

            // Create POST data and convert it to a byte array.
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            _dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            _dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            _dataStream.Close();

        }

        public string GetResponse()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
            // Get the original response.
            WebResponse response = request.GetResponse();

            this.Status = ((HttpWebResponse)response).StatusDescription;

            // Get the stream containing all content returned by the requested server.
            _dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(_dataStream);

            // Read the content fully up to the end.
            string responseFromServer = reader.ReadToEnd();

            // Clean up the streams.
            reader.Close();
            _dataStream.Close();
            response.Close();

            return responseFromServer;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }

        }

        public void SendCommand()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            try
            {
                WebResponse response = request.GetResponse();
                response.Close();
            }
            catch (Exception e)
            {
            }
        }

        private static void CommandSent(IAsyncResult ar)
        {
            try
            {
                var request = (HttpWebRequest)ar.AsyncState;
                var response = request.EndGetResponse(ar);
            }
            catch (Exception e)
            {

            }

        }

    }
}