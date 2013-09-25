using Buche;
using Xunit;

namespace BucheTests
{
    public class RequestInfoTest
    {
        const string SessionId = "sid";
        const string RequestId = "rid";

        [Fact]
        public void TestBasic()
        {
            var requestInfo = new RequestInfo(SessionId, RequestId);

            Assert.Equal(SessionId, requestInfo.SessionId);
            Assert.Equal(RequestId, requestInfo.RequestId);
        }

        [Fact]
        public void TestFromString()
        {
            var requestInfoString = string.Format("{0}-{1}", SessionId, RequestId);
            var requestInfo = RequestInfo.FromString(requestInfoString);

            Assert.Equal(SessionId, requestInfo.SessionId);
            Assert.Equal(RequestId, requestInfo.RequestId);
        }

        [Fact]
        public void TestFromStringNullSessionId()
        {
            var requestInfoString = RequestId;
            var requestInfo = RequestInfo.FromString(requestInfoString);
            Assert.Equal(RequestId, requestInfo.RequestId);
            Assert.Null(requestInfo.SessionId);
        }

        [Fact]
        public void TestToString()
        {
            var requestInfoString = string.Format("{0}-{1}", SessionId, RequestId);
            var requestInfo = new RequestInfo(SessionId, RequestId);

            Assert.Equal(requestInfoString, requestInfo.ToString());
        }

        [Fact]
        public void TestToStringNullSessionId()
        {
            var requestInfo = new RequestInfo(null, RequestId);
            Assert.Equal(RequestId, requestInfo.ToString());
        }
    }
}
