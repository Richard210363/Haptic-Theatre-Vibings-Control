﻿#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings ControlX\Haptic Theatre-Vibings Control\Controllers\SetupController.cs
* ****************************************************************************************** 
 DESCRIPTION   : Used as the top level of settings

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 10/04/2016                 Richard Byrne          Created
**********************************************************************************************/

#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Haptic_Theatre_Vibings_Control.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        public ActionResult Index()
        {
            ViewBag.ViewName = "Setup";
            return View();
        }
    }
}