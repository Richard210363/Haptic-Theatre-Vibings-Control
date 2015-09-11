#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings ControlX\Haptic Theatre-Vibings Control\Controllers\ModesController.cs
* ****************************************************************************************** 
 DESCRIPTION   : Used when setting up modes.  Where a mode is collection of colours, timings, fades etc 
 that are sent to a vibing as a package and the vibings uses to determin what to display.

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 11/09/2015                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Haptic_Theatre_Vibings_Control.Controllers
{
    public class ModesController : Controller
    {
        // GET: Modes
        public ActionResult Index()
        {
            return View();
        }
    }
}
