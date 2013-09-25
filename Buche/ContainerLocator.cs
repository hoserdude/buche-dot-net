using Microsoft.Practices.Unity;

namespace Buche
{
    public class ContainerLocator
    {
        private static IUnityContainer _container;

        public static IUnityContainer Container
        {
            set { if (_container == null) _container = value; }

            get { return _container; }
        }
    }
}
