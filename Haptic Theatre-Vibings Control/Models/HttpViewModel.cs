#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Models\HttpViewModel.cs
* ****************************************************************************************** 
 DESCRIPTION   : Stores the data to show in an Http view

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 31/08/2015                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using Haptic_Theatre_Vibings_Control.Classes;

namespace Haptic_Theatre_Vibings_Control.Models
{

    public class HttpViewModel 
    {
        public string HttpRequest { get; set; }
        public string HttpResponse { get; set; }
        public string HttpRequestType { get; set; }
        
        public HttpViewModel()
        {
        }

        public HttpViewModel(string httpRequest, string httpResponse)
        {
            this.HttpRequest = httpRequest;
            this.HttpResponse = httpResponse;
        }
    }
}