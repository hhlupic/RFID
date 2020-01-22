using System;
using System.IO;

namespace TCPServer
{
    class Logger
    {
        public static void LogException(Exception exception)
        {
            DateTime dateTime = DateTime.Now;
            string fileName = "error_log_" + dateTime.Year + dateTime.Month + dateTime.Day;
            try
            {
                using (StreamWriter streamWriter = File.AppendText(fileName))
                {
                    streamWriter.WriteLine("======================================{0}", DateTime.Now);
                    streamWriter.WriteLine("=> An Error occurred: {0} Message: {1}{2}",
                        exception.Message,
                        Environment.NewLine,
                        exception.StackTrace);
                    streamWriter.WriteLine("======================================{0}",Environment.NewLine);
                }
            }
            catch (Exception) { }
        }

        public static void LogError(string error)
        {
            DateTime dateTime = DateTime.Now;
            string fileName = "error_log_" + dateTime.Year + dateTime.Month + dateTime.Day;
            try
            {
                using (StreamWriter streamWriter = File.AppendText(fileName))
                {
                    streamWriter.WriteLine("======================================{0}", DateTime.Now);
                    streamWriter.WriteLine("=>{0} An Error occurred: {1}",
                        DateTime.Now,
                        error);
                    streamWriter.WriteLine("======================================{0}", Environment.NewLine);
                }
            }
            catch (Exception) { }
        }
    }
}
