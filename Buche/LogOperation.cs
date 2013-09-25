using System;

namespace Buche
{
    public class LogOperation : IDisposable
    {
        public ILogger Logger { get; private set; }
        public string PreviousValue { get; private set; }

        public LogOperation(ILogger logger, string value)
        {
            Logger = logger;
            PreviousValue = Logger.GetProperty(LogContext.PropertyKey.Operation);

            // set the new key/value
            if (PreviousValue.HasValue())
            {
                Logger.SetProperty(LogContext.PropertyKey.Operation, PreviousValue + "." + value);
            }
            else
            {
                Logger.SetProperty(LogContext.PropertyKey.Operation, value);
            }
        }

        public void Dispose()
        {
            // restore the previous key/value
            Logger.SetProperty(LogContext.PropertyKey.Operation, PreviousValue);
        }
    }
}