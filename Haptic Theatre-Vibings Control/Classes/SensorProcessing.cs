#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\SensorProcessing.cs
* ****************************************************************************************** 
 DESCRIPTION   : Collects sensor data and calls to change modes

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 24/08/2016                 Richard Byrne          Created 
**********************************************************************************************/

#endregion



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Haptic_Theatre_Vibings_Control.Classes
{

    public static class SensorProcessing
    {
        private static int _dataPointCount=0;
        private static Decimal _summedSensorValue = 0;
        private static Decimal _averageSensorValue = 0;

        public static void ProcessData(string sensorData)
        {
            Decimal sensorValue = ConvertToNumber(sensorData);
            _summedSensorValue += sensorValue;
            _dataPointCount++;

            if (_dataPointCount == 10)
            {
                _averageSensorValue = _summedSensorValue/10;

                ModeChangeTriggering.SwitchDisplayBySensorValue(_averageSensorValue);

                _averageSensorValue = 0;
                _summedSensorValue = 0;
                _dataPointCount = 0;
            }

        }

        private static Decimal ConvertToNumber(string sensorData)
        {
            string[] dataXYZ = new string[3];
            dataXYZ = sensorData.Split(',');

            if (dataXYZ[0] == "No Messages" || dataXYZ[0] == "")
            {
                return 0;
            }

            try
            {
                Decimal x = Math.Abs(Convert.ToDecimal(dataXYZ[0]));
                Decimal y = Math.Abs(Convert.ToDecimal(dataXYZ[1]));
                Decimal z = Math.Abs(Convert.ToDecimal(dataXYZ[2]));

                return x + y + z;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}