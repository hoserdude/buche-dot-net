using System;
using System.Reflection;
using System.Reflection.Emit;
using Buche;
using Xunit;
using Xunit.Extensions;

namespace BucheTests
{
	/// <summary>
	/// Unit tests for the Log4NetLoggerAdapter class.
	/// </summary>
	public class Log4NetLoggerAdapterTest
	{
	    private ILogger logger;

		public Log4NetLoggerAdapterTest()
		{
		    MethodBase method = MethodBase.GetCurrentMethod();
            logger = new Log4NetLoggerAdapter(method);
		}

		[Fact]
		public void TestLogInit()
		{
			string requestId = logger.GetRequestId();
			Assert.NotNull(requestId);
			Assert.Equal(string.Empty, requestId);

			logger.SetRequestId("session", "request");
			requestId = logger.GetRequestId();
			Assert.NotNull(requestId);
			Assert.Equal("session-request", requestId);
		}

		[Theory]
        [InlineData(LogContext.PropertyKey.RequestId, "1234")]
        [InlineData(LogContext.PropertyKey.SessionId, "216899585")]
        public void TestLoggerSetProperty(string propertyKey, string propertyValue)
		{
            logger.SetProperty(propertyKey, propertyValue);
            Assert.Equal(propertyValue, logger.GetProperty(propertyKey));

            logger.SetProperty(propertyKey, null);
            Assert.Equal(null, logger.GetProperty(propertyKey));
		}

        [Fact]
        public void TestLoggerWithFormat()
        {
            logger.InfoFormat("Hello {0}", "world");
        }

        [Fact]
        public void TestLoggerWithFormatAndException()
        {
            Exception ex = new Exception();
            logger.ErrorFormat(ex, "Hello {0}", "world");
        }


	}
}
