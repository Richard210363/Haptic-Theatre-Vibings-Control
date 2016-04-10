using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Haptic_Theatre_Vibings_Control.Controllers
{
    public class ShowsController : Controller
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