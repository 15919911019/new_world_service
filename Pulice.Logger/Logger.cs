using log4net;
using log4net.Config;
using System;
using System.IO;

namespace Public.Log
{
    public class Logger
    {
        private static ILog _logger;

        static Logger()
        {
            if (_logger == null)
            {
                if (_logger == null)
                {
                    var repository = LogManager.CreateRepository("NETCoreRepository");
                    //log4net从log4net.config文件中读取配置信息
                    XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                    _logger = LogManager.GetLogger(repository.Name, "InfoLogger");
                }
            }
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(string message, Exception exception = null)
        {
            if (exception == null)
                _logger.Debug(message);
            else
                _logger.Debug(message, exception);
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception = null)
        {
            if (exception == null)
                _logger.Info(message);
            else
                _logger.Info(message, exception);
        }

        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                _logger.Warn(message);
            else
                _logger.Warn(message, exception);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception = null)
        {
            if (exception == null)
                _logger.Error(message);
            else
                _logger.Error(message, exception);
        }
    }
}
