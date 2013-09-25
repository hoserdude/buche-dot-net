using Buche;
using Microsoft.Practices.Unity;
using Xunit;

namespace BucheTests
{
    public class InstrumentationInterceptorTest
    {
        public InstrumentationInterceptorTest()
        {
            ContainerLocator.Container = new UnityContainer();
            ContainerLocator.Container.RegisterInstance<ILogger>(new Logger());
        }

        [Fact]
        public void TestGetRequiredTypes()
        {
            
        }

        [Fact]
        public void TestWillExecute()
        {
            InstrumentationInterceptor interceptor = new InstrumentationInterceptor();
            Assert.True(interceptor.WillExecute);
        }

        [Fact]
        public void TestInvokePositive()
        {
            //TODO implement this crazy reflection stuff
            //TraceBehavior interceptor = new TraceBehavior();
            //string target = "foo";
            //IMethodInvocation input = new VirtualMethodInvocation(target,);
            //GetNextInterceptionBehaviorDelegate getNextDelegate = new GetNextInterceptionBehaviorDelegate(delegate { interceptor.Invoke });
            //IMethodReturn result = interceptor.Invoke(input, getNextDelegate);
        }
    }
}
