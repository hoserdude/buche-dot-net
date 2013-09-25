using System;
using Buche;
using Xunit;

namespace BucheTests
{
    public class LoggerTest
    {
        [Fact]
        public void TestLogMethods()
        {

        }

        [Fact]
        public void TestAdaptivePropertyProviderNoHttpContext()
        {
            const string mysessionid = "mySessionId";
            AdaptivePropertyProvider<string> result = AdaptivePropertyProviderFactory.Create("sessionId", mysessionid);
            Assert.NotNull(result);
            Assert.Equal(mysessionid, result.ToString());
        }

        [Fact]
        public void TestAdaptivePropertyProviderNullInputs()
        {
            Assert.Throws<ArgumentNullException>(delegate
                                                     {
                                                         AdaptivePropertyProviderFactory.Create(null, "foo");
                                                     });
        }
    }
}
