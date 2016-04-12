using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Haptic_Theatre_Vibings_Control.Classes
{
    [HubName("signalhub")]
    public class SignalHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}