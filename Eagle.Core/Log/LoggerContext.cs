using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using Eagle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Eagle.Core.Log
{
    public sealed class LoggerContext
    {
        private static ILoggerFactory currentLoggerFactory;

        static LoggerContext() 
        {
            currentLoggerFactory = CreateLoggerFactory();
        }

        public static ILogger CurrentLogger
        {
            get 
            {
                return currentLoggerFactory.CreateLogger();
            }
        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            string loggerFactoryName = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Logger.Provider;

            if (!loggerFactoryName.HasValue())
            {
                throw new ConfigException("The default logger provider has not been defined in the ConfigSource.");
            }

            Type loggerFactoryType = Type.GetType(loggerFactoryName);

            if (loggerFactoryType == null)
            {
                throw new InfrastructureException("The IloggerFactory defined by type {0} doesn't exist.", loggerFactoryName);
            }

            return (ILoggerFactory)AppRuntime.Instance.CurrentApplication.ObjectContainer.Resolve(loggerFactoryType);
        }
    }
}
