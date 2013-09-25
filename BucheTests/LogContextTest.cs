using System;
using System.Collections.Generic;
using Buche;
using Xunit;

namespace BucheTests
{
    public class LogContextTest
    {
        [Fact]
        public void TestNullLogContextProperties()
        {
            var logContext = new LogContext(null);
            Assert.Null(logContext.Properties);
        }

        [Fact]
        public void TestSetContextWithNullLogContextProperties()
        {
            var logContext = new LogContext(null);
            var logger = new Logger();
            logger.SetContext(logContext);
            Assert.Equal(0, logger.Properties.Count);
        }

        [Fact]
        public void TestSetContextDoesNotSetNonPropertyKeysProperty()
        {
            var logContext = new LogContext();
            var logger = new Logger();

            logContext.Properties["k1"] = "v1";
            logger.SetContext(logContext);

            Assert.Equal(0, logger.Properties.Count);
            Assert.Equal(null, logger.GetProperty("k1"));
        }

        [Fact]
        public void TestSetContextSetsOnlyPropertyKeys()
        {
            var logContext = new LogContext();
            var logger = new Logger();

            foreach (var propertyKeyField in LogContext.PropertyKey.Fields)
            {
                var propertyKey = propertyKeyField.GetValue(logContext) as string;
                logContext.Properties[propertyKey] = propertyKey;
            }

            logger.SetContext(logContext);
            Assert.Equal(logContext.Properties.Count, logger.Properties.Count);

            foreach (var propertyKeyField in LogContext.PropertyKey.Fields)
            {
                var propertyKey = propertyKeyField.GetValue(logContext) as string;
                var propertyValue = logger.GetProperty(propertyKey);

                Assert.Equal(logContext.Properties[propertyValue], propertyValue);
            }
        }

        [Fact]
        public void TestClearContextClearsOnlyPropertyKeysProperties()
        {
            var logContext = new LogContext();
            var logger = new Logger();

            foreach (var propertyKeyField in LogContext.PropertyKey.Fields)
            {
                var propertyKey = propertyKeyField.GetValue(logContext) as string;
                logContext.Properties[propertyKey] = propertyKey;
            }

            logger.SetContext(logContext);
            logger.SetProperty("k1", "v1");
            logger.ClearContext();

            Assert.Equal(1, logger.Properties.Count);
            Assert.Equal("v1", logger.GetProperty("k1"));
        }
    }

    internal class Logger : ILogger
    {
        public Dictionary<string, string> Properties { get; private set; }

        public Logger()
        {
            Properties = new Dictionary<string, string>();
        }

        public void SetProperty(string key, string value)
        {
            if (value == null &&
                Properties.ContainsKey(key))
            {
                Properties.Remove(key);
            }
            else
            {
                Properties[key] = value;
            }
        }

        public string GetProperty(string key)
        {
            return Properties.ContainsKey(key) ? Properties[key] : null;
        }

        public bool IsDebugEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsErrorEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsFatalEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInfoEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsWarnEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public void Debug(object message)
        {
            throw new NotImplementedException();
        }

        public void Debug(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(object message)
        {
            throw new NotImplementedException();
        }

        public void Error(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(object message)
        {
            throw new NotImplementedException();
        }

        public void InfoWithoutMsgPrefix(object message)
        {
            throw new NotImplementedException();
        }

        public void Info(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(Exception ex, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(Exception ex, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(Exception ex, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(Exception ex, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(Exception ex, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public string GetLastLogMessage(out LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

    }
}
