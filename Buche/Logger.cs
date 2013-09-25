using System;

namespace Buche
{
    public enum LogLevel
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug
    }

    /// <summary>
    /// Interface that mimics ILog from Log4Net, so we can use something else if we want, and we can add our 
    /// own value added additional functionality.
    /// </summary>
    public interface ILogger
    {
        void SetProperty(string key, string value);
        string GetProperty(string key);

        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }

        void Debug(object message);
        void Debug(object message, Exception exception);
        void Error(object message);
        void Error(object message, Exception exception);
        void Fatal(object message);
        void Fatal(object message, Exception exception);
        void Info(object message);
        void InfoWithoutMsgPrefix(object message);
        void Info(object message, Exception exception);
        void Warn(object message);
        void Warn(object message, Exception exception);

    	void DebugFormat(string format, params object[] args);
        void DebugFormat(Exception ex, string format, params object[] args);
        void ErrorFormat(string format, params object[] args);
        void ErrorFormat(Exception ex, string format, params object[] args);
        void FatalFormat(string format, params object[] args);
        void FatalFormat(Exception ex, string format, params object[] args);
        void InfoFormat(string format, params object[] args);
        void InfoFormat(Exception ex, string format, params object[] args);
		void WarnFormat(string format, params object[] args);
        void WarnFormat(Exception ex, string format, params object[] args);

        string GetLastLogMessage(out LogLevel logLevel);
    }
}
