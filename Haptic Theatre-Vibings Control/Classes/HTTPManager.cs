#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\HTTPManager.cs
* ****************************************************************************************** 
 DESCRIPTION   : Controls the use of Http connections

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 30/08/2015                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Haptic_Theatre_Vibings_Control.Classes
{
    public class HTTPManager
    {
        public string sendRequest(string ip)
        {
            WebRequest webRequest = new WebRequest(ip, "POST", "a=value1&b=value2");
            var response = webRequest.GetResponse();

            return response;
        }


    }
}