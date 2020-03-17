using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainApp
{
    public class ControllerLog
    {
        public static void LogInfo(string info)
        {

            string controllerLogFilePath = @".\Data\controllerlog.txt";

            if(File.Exists(controllerLogFilePath))
            { 
            StreamWriter sw = File.AppendText(controllerLogFilePath);

            sw.WriteLine(info);

            sw.Close();
            
            }


        }
    }
}
