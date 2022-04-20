using System;
using log4net;

namespace COAD.UTIL.Helpers
{
    public class CoadCorpLog
    {
        private ILog logger;

        private static bool initialized = false;
        private static Object locker = new Object();


        public CoadCorpLog(Type t)
        {
            if (!initialized)
            {
                lock (locker)
                {
                    if (!initialized)
                    {
                        Initialize();
                        initialized = true;
                    }
                }
            }
            logger = LogManager.GetLogger(t);
        }

        public static void Initialize()
        {
            if (System.IO.File.Exists("log4net.config"))
            {
                log4net.Config.XmlConfigurator.Configure();
            }
            else
            {
                log4net.Config.XmlConfigurator.Configure();
            }

        }

        public static Object Context
        {
            set { GlobalContext.Properties["USER"] = value; GlobalContext.Properties["NDC"] = value; }
        }

        public bool IsDebugEnabled { get { return logger.IsDebugEnabled; } }

        public void Debug(string msg)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(msg);
            }
        }

        public void Info(string msg)
        {
            logger.Info(msg);
        }
        public void Error(string msg)
        {
            logger.Error(msg);
        }

        public void Error(string msg, Exception ex)
        {
            logger.Error(msg, ex);
        }

        //public static string PastaDeEscrita => Path.GetDirectoryName
        //    (Assembly.GetExecutingAssembly().Location);

    }
}
