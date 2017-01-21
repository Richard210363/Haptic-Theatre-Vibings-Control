#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\SensorProcessing.cs
* ****************************************************************************************** 
 DESCRIPTION   : Collects sensor data and either
                    calls to change modes
                    records the data 

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 24/08/2016                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using System;
using Haptic_Theatre_Vibings_Control.Models;

namespace Haptic_Theatre_Vibings_Control.Classes
{

    public static class SensorProcessing
    {
        private static int _dataPointCount;
        private static decimal _summedSensorValue;
        private static decimal _averageSensorValue;

        public static void ProcessData(string sensorData)
        {
            decimal sensorValue = ConvertToNumber(sensorData);
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

        public static void RecordData(HttpViewModel httpViewModel)
        {

            if (FileWriterManager.File== null)
            {
                FileWriterManager.OpenFileStream();
            }

            FileWriterManager.WriteData(httpViewModel);
        }

        internal static void CancelRecordData()
        {
            FileWriterManager.CloseFileStream();
        }

        private static decimal ConvertToNumber(string sensorData)
        {
            var dataXYZ = sensorData.Split(',');

            if (dataXYZ[0] == "No Messages" || dataXYZ[0] == "")
            {
                return 0;
            }

            try
            {
                decimal x = Math.Abs(Convert.ToDecimal(dataXYZ[0]));
                decimal y = Math.Abs(Convert.ToDecimal(dataXYZ[1]));
                decimal z = Math.Abs(Convert.ToDecimal(dataXYZ[2]));

                return x + y + z;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static SensorData ConvertToSensorData(string sensorInput)
        {
            var dataXYZ = sensorInput.Split(',');

            if (dataXYZ[0] == "No Messages" || dataXYZ[0] == "")
            {
                return new SensorData(0, 0, 0);
            }

            try
            {
                decimal x = Math.Abs(Convert.ToDecimal(dataXYZ[0]));
                decimal y = Math.Abs(Convert.ToDecimal(dataXYZ[1]));
                decimal z = Math.Abs(Convert.ToDecimal(dataXYZ[2]));

                SensorData sensorData = new SensorData(x,y,z);

                return sensorData;
            }
            catch (Exception)
            {
                return new SensorData(0, 0, 0);
            }
        }
    }
}