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
using Haptic_Theatre_Vibings_Control.Classes;

namespace Haptic_Theatre_Vibings_Control.Controllers
{
    public class LiveController : Controller
    {
        // GET: Live
        public ActionResult Index()
        {
            return View();
        }

        // GET: Live
        public ActionResult StartShow()
        {
            Global.IsShowLive = !Global.IsShowLive;
            return RedirectToAction("Index", "Live");
        }

        #region JUST FOR DEMOS


        public ActionResult HeartBeatMonitor()
        {
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_001(string colourHex)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=001&color=%23" + colourHex);
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=001&color=%23" + colourHex);
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_002(string colourHex, string loopDelay)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=002&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=002&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_003(string colourHex, string loopDelay)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=003&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=003&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_004(string colourHex, string loopDelay)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=004&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=004&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_005(string colourHex, string loopDelay)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=005&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=005&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_006(string colourHex, string loopDelay)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=006&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=006&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_007(string colourHex_01, string colourHex_02, string colourHex_03, string colourHex_04, string loopDelay)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=007" +
                                       "&color01=%23" + colourHex_01 +
                                       "&color02=%23" + colourHex_02 +
                                       "&color03=%23" + colourHex_03 +
                                       "&color04=%23" + colourHex_04 +
                                       "&loopdelay=" + loopDelay);

            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=007" +
                           "&color01=%23" + colourHex_01 +
                           "&color02=%23" + colourHex_02 +
                           "&color03=%23" + colourHex_03 +
                           "&color04=%23" + colourHex_04 +
                           "&loopdelay=" + loopDelay);


            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_008()
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=008&color=%23");
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=008&color=%23");
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_009()
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=009&color=%23");
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=009&color=%23");
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_010()
        {
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_011(string colourHex, string loopDelay)
        {
            HTTPManager.SendGetRequest("http://192.168.100.13/?mode=011&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            HTTPManager.SendGetRequest("http://192.168.100.12/?mode=011&color=%23" + colourHex + "&loopdelay=" + loopDelay);
            return RedirectToAction("Index", "Live");
        }

        public ActionResult Mode_012()
        {
            return RedirectToAction("Index", "Live");
        }

        #endregion
    }
}
