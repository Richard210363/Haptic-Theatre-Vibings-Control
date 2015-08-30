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
using System.Web;
using System.Web.Mvc;
using Haptic_Theatre_Vibings_Control.Classes;

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
            return View();
        }

        #region Http Development

        /// <summary>
        /// Send an Http message
        /// </summary>
        /// <returns></returns>
        public ActionResult SendHTTPMessage()
        {
            //create the constructor with post type and few data
            HTTPManager httpManager = new HTTPManager();
            var response = httpManager.sendRequest("http://www.bbc.co.uk/news");

            //todo Pick up the response and put into a text box 

            return View();
        }

        #endregion
    }
}
