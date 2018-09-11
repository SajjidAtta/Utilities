using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogWriter
{
    public class Logger
    {
        //#TODO:Log Error to windows events
        
        public enum MessageType { DEBUG, INF, ERROR };
        string LogFile;
        static Logger instance;
        private Logger()
        {
            try
            {
                LogFile = Path.GetFullPath((System.Configuration.ConfigurationManager.AppSettings["LogFilePath"]));
            }
            catch
            {
                string fullPath = System.Reflection.Assembly.GetAssembly(typeof(Logger)).Location;
                LogFile= Path.GetDirectoryName(fullPath)+"LogFile.txt";
            }
        }
        public static Logger Instance
        {
            get
            {
                if (instance == null)
                    instance = new Logger();
                return instance;
            }
        }
        public void Log(MessageType msgtype, string msg, params string[] values)
        {

            msg = String.Format("{0} -- [{1}] -- {2}", System.DateTime.Now, msgtype.ToString(), String.Format(msg.ToString(), values));


            try
            {


                if (!File.Exists(LogFile))
                {


                    using (StreamWriter sw = new StreamWriter(File.Create(LogFile)))
                    {
                        sw.WriteLine(msg);
                        sw.Close();
                    }

                }
                else
                {
                    using (StreamWriter sw = File.AppendText(LogFile))
                    {
                        sw.WriteLine(msg);
                        sw.Close();
                    }
                }

            }
            catch (Exception exp)
            {
                //Write to Event Log
            }

        }
        public void LogtoConsole(string msg, params string[] values)
        {
            msg = String.Format(msg, values);
            try
            {

                Console.WriteLine(System.DateTime.Now + " -- " + msg);



            }
            catch (Exception exp)
            {
                //Write to Event Log
            }

        }
        public void LogMethodStart()
        {
            //#TODO: Reflections related Work for Method Names
            String MethodName = "SomeName";
            Log(MessageType.DEBUG, "Method Start: {0}", MethodName);

        }
        public void LogMethodEnd()
        {
            //#TODO: Reflections related Work for Method Names
            String MethodName = "SomeName";
            Log(MessageType.DEBUG, "Method End: {0}", MethodName);
        }
    }
}
