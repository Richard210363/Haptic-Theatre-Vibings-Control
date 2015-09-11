#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings ControlX\Haptic Theatre-Vibings Control\Controllers\LiveController.cs
* ****************************************************************************************** 
 DESCRIPTION   : Used when running Vibings in a LIve Show
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
    public class LiveController : Controller
    {
        // GET: Live
        public ActionResult Index()
        {
            return View();
        }
    }
}
