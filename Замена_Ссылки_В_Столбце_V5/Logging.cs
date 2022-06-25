using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLib
{
    public class Logging
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void initNLog()
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = @"Замена_Ссылки_В_Столбце_V5_LOG.txt" };
            logfile.Layout = "${longdate} ${message} ${exception:format=tostring,Data}";
            logfile.DeleteOldFileOnStartup = true;
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            logger.Info("Programm Замена_Ссылки_В_Столбце_V5 started");
        }
    }
}
