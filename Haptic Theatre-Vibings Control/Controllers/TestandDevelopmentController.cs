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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
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
            HttpViewModel httpViewModel = new HttpViewModel();
            httpViewModel.HttpRequestType = HttpRequestType.Get;
            return View(httpViewModel);
        }

        #region Http Development

        /// <summary>
        /// Send an Http message
        /// </summary>
        /// <param name="httpViewModel"></param>
        /// <returns></returns>
        /// <remarks>
        /// Test URLs
        /// http://httpbin.org/robots.txt
        /// http://httpbin.org/get
        /// http://httpbin.org/post
        /// </remarks>
        public ActionResult SendHttpMessage(HttpViewModel httpViewModel)
        {
            if (httpViewModel == null) throw new ArgumentNullException(nameof(HttpViewModel));

            string response="";

            HTTPManager httpManager = new HTTPManager();

            if (httpViewModel.HttpRequestType == HttpRequestType.Get)
            {
                response = httpManager.SendGetRequest(httpViewModel.HttpRequest);
            }
            if (httpViewModel.HttpRequestType == HttpRequestType.Post)
            {
                response = httpManager.SendPostRequest(httpViewModel.HttpRequest);
            }

            httpViewModel.HttpResponse = response;
            Thread.Sleep(2000);

            return PartialView("_HttpResponse",httpViewModel);
        }

        #endregion
    }
}