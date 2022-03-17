using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;

namespace FocuSpider
{
    public enum LogLevel
    {
        [Description("Info")]
        INFO = 0,
        [Description("Error")]
        ERROR = 1

    }

    public class Logger
    {

        public static void Log(LogLevel level, string message)
        {
            try
            {
                string line = $"{level.ToString()} {DateTime.Now} | {message} \r\n";
                File.AppendAllText("Log.txt", line);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
