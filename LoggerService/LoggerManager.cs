﻿using Contracts.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();



        public void LogDebug(string message) => logger.Debug(message);

        public void LogError(string message) => logger.Debug(message);

        public void LogInfo(string message) => logger.Debug(message);

        public void LogWarn(string message) => logger.Debug(message);
    }
}
