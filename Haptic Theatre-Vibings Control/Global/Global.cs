using System.Web.Mvc;

namespace Haptic_Theatre_Vibings_Control
{
    /// <summary>
    /// Site's global variables.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Is the Show running now
        /// </summary>
        static bool _isShowLive;

        /// <summary>
        /// Get or set Is the Show running now
        /// </summary>
        public static bool IsShowLive
        {
            get
            {
                return _isShowLive;
            }
            set
            {
                _isShowLive = value;
            }
        }
    }
    public class SharedViewParameters : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsShowLive = Global.IsShowLive;
        }

    }
}