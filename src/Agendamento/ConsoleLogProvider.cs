using Microsoft.Extensions.Logging;
using Quartz.Logging;
using System;

namespace EDM.RFLocal.Interceptador.Agendamento
{
    public class ConsoleLogProvider : ILogProvider
    {
        private ILogger _log;
        public ConsoleLogProvider(ILogger log)
        {
            _log = log;
        }
        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (level >= Quartz.Logging.LogLevel.Info && func != null)
                {
                    _log.LogInformation(func());
                }
                if (exception != null)
                    _log.LogError(exception, exception.Message);
                return true;
            };
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
        {
            throw new NotImplementedException();
        }
    }
}
