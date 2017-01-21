#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Controllers\TestandDevelopmentController.cs
* ****************************************************************************************** 
 DESCRIPTION   : Scratch pad code for development

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 30/08/2015                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using System;
using System.Web.Mvc;
using Haptic_Theatre_Vibings_Control.Classes;
using Haptic_Theatre_Vibings_Control.Models;

namespace Haptic_Theatre_Vibings_Control.Controllers
{
    public class TestandDevelopmentController : Controller
    {
        /// <summary>
        /// Return the main development view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Return the main Http development view
        /// </summary>
        /// <returns></returns>
        public ActionResult HttpDevelopment()
        {
            HttpViewModel httpViewModel = new HttpViewModel
            {
                HttpRequestType = HttpRequestType.Get,
                HttpPortNumber = "50002"
            };

            //set defaults
            return View(httpViewModel);
        }

        #region Http Development

        /// <summary>
        /// Send a tcp message
        /// </summary>
        /// <param name="httpViewModel"></param>
        /// <returns></returns>
        /// <remarks>
        /// Test URLs
        /// http://httpbin.org/robots.txt
        /// http://httpbin.org/get
        /// http://httpbin.org/post
        /// </remarks>
        public ActionResult SendTcpMessage(HttpViewModel httpViewModel)
        {
            if (httpViewModel == null) throw new ArgumentNullException(nameof(HttpViewModel));

            Uri uriResult;
            bool result = Uri.TryCreate(httpViewModel.HttpRequest, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if(!result)
            {
                httpViewModel.HttpResponse = "Not a valid URL";

                return PartialView("_HttpResponse", httpViewModel);
            }

            string response="";

            if (httpViewModel.HttpRequestType == HttpRequestType.Get)
            {
                response = HTTPManager.SendGetRequest(httpViewModel.HttpRequest);
            }
            if (httpViewModel.HttpRequestType == HttpRequestType.Post)
            {
                response = HTTPManager.SendPostRequest(httpViewModel.HttpRequest);
            }

            httpViewModel.HttpResponse = response;

            return PartialView("_HttpResponse",httpViewModel);
        }

        /// <summary>
        /// Send a Udp message
        /// </summary>
        /// <param name="httpViewModel"></param>
        /// <returns></returns>
        public ActionResult SendUdpBroadcast(HttpViewModel httpViewModel)
        {
            if (httpViewModel == null) throw new ArgumentNullException(nameof(HttpViewModel));

            var response = HTTPManager.SendUdpBroadcast(httpViewModel.HttpRequest, httpViewModel.HttpPortNumber);

            httpViewModel.HttpResponse = response;

            return PartialView("_HttpResponse", httpViewModel);  //Note This is not used in the view we just need it because ActionResults demand a Return
        }


        /// <summary>
        /// Receive a Udp message
        /// </summary>
        /// <returns></returns>
        /// <remarks>50002</remarks>
        public ActionResult ReceiveUDPMessage(HttpViewModel httpViewModel)
        {
            httpViewModel.HttpResponse = HTTPManager.ReceiveUdpBroadcast(httpViewModel.HttpPortNumber);

            SensorProcessing.ProcessData(httpViewModel.HttpResponse);

            return PartialView("_HttpResponse", httpViewModel);
        }


        /// <summary>
        /// Cancel UDP Receive
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelReceiveUDPMessage(HttpViewModel httpViewModel)
        {
            string response = "";

            HTTPManager.CancelUdpBroadcast();

            httpViewModel.HttpResponse = response;

            return PartialView("_HttpResponse", httpViewModel);  //Note This is not used in the view we just need it because ActionResults demand a Return
        }


        /// <summary>
        /// Record a Udp message
        /// </summary>
        /// <returns></returns>
        /// <remarks>50002</remarks>
        public ActionResult RecordUDPMessage(HttpViewModel httpViewModel)
        {
            httpViewModel.HttpResponse = HTTPManager.ReceiveUdpBroadcast(httpViewModel.HttpPortNumber);

            SensorProcessing.RecordData(httpViewModel);

            return PartialView("_HttpResponse", httpViewModel);
        }


        /// <summary>
        /// Cancel UDP Receive
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelRecordUDPMessage(HttpViewModel httpViewModel)
        {
            string response = "";

            HTTPManager.CancelUdpBroadcast();

            httpViewModel.HttpResponse = response;

            SensorProcessing.CancelRecordData();

            return PartialView("_HttpResponse", httpViewModel);  //Note This is not used in the view we just need it because ActionResults demand a Return
        }
        #endregion
    }
}