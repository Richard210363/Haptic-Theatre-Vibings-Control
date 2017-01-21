#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Models\SensorData.cs
* ****************************************************************************************** 
 DESCRIPTION   : Stores the sensor data data 

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 31/08/2015                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

namespace Haptic_Theatre_Vibings_Control.Models
{

    public class SensorData
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public SensorData()
        {
        }

        public SensorData(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}