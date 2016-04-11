#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Controllers\SetupShowsController.cs
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

namespace Haptic_Theatre_Vibings_Control.Controllers
{
    public class SetupShowsController : Controller
    {
        // GET: Shows
        public ActionResult Index()
        {
            return View();
        }

        // GET: Modes
        public ActionResult ManageShows()
        {
            return View();
        }

        // GET: Modes
        public ActionResult SetupModes()
        {
            return View();
        }

        // GET: Modes
        public ActionResult SetupInputs()
        {
            return View();
        }
    }
}