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


using System.Threading.Tasks;
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

        #region JUST FOR DEMOS

        // GET: Live
        public ActionResult StartShow()
        {
            Global.IsShowLive = !Global.IsShowLive;

            if (Global.IsShowLive)
            {
                ModeChangeTriggering.ContinueToRead = true;
                Task.Run(() => { ModeChangeTriggering.StartShow(); });
            }
            else
            {
                ModeChangeTriggering.ContinueToRead = false;
            }
            return RedirectToAction("Index", "Live");
        }

        public ActionResult HeartBeatMonitor()
        {
            return RedirectToAction("Index", "Live");
        }

        public ActionResult ChangeModeManually(string showMode)
        {
            ModeChangeTriggering.ChangeModeByShowModeID(showMode);
            return RedirectToAction("Index", "Live");
        }

 

        #endregion
    }
}
