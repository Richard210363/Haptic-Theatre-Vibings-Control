#region File Information

/********************************************************************************************
* \Haptic Theatre-Vibings Control\Haptic Theatre-Vibings Control\Classes\FileWriterManager.cs
* ****************************************************************************************** 
 DESCRIPTION   : Controls the use of file writing
                 Used when saving data for analysis

 REVISION HISTORY: 

 Date(MM/DD/YYYY)		    REV BY		           REV DESC
 ----------------------------------------------------------------------------------------------
 17/01/2017                 Richard Byrne          Created 
**********************************************************************************************/

#endregion

using System;
using System.IO;
using System.Text;
using Haptic_Theatre_Vibings_Control.Models;


namespace Haptic_Theatre_Vibings_Control.Classes
{
    public static class FileWriterManager
    {
        public static StreamWriter File;

        public static void OpenFileStream()
        {
            string databasePath = AppDomain.CurrentDomain.BaseDirectory + @"\Database";
            File = new StreamWriter(databasePath + @"\Haptic_Sensor_data.csv", true);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Activity,");
            stringBuilder.Append("X,");
            stringBuilder.Append("Y,");
            stringBuilder.Append("Z,");
            stringBuilder.AppendLine("DateTime");

            try
            {
                File.WriteLine(stringBuilder.ToString());
                File.Flush();
            }
            catch (Exception ex)
            {
                int x = 0;
            }

        }

        internal static void CloseFileStream()
        {
            try
            {
                File.Flush();
                File.Close();
            }
            catch
            { }
        }

        public static void WriteData(HttpViewModel httpViewModel)
        {
            if (File.BaseStream == null)
            {
                OpenFileStream();
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(httpViewModel.ActivityNote);
            stringBuilder.Append(",");
            stringBuilder.Append(httpViewModel.HttpResponse);
            stringBuilder.Append(",,,");
            stringBuilder.AppendLine(DateTime.Now.ToString("hh:mm:ss.ffff"));

            try
            {
                File.WriteLine(stringBuilder.ToString());
                File.Flush();
            }
            catch (Exception ex)
            {
                int x = 0;
            }

        }
    }
}