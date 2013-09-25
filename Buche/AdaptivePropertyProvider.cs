using System;
using System.Web;

namespace Buche
{
    /// <summary>
    /// Adapted (ha ha) from http://www.2geeks1rant.com/2010/11/log4net-contextual-properties-and.html
    /// The main issue we are trying to avoid is having the sessionId and request Id be shared by multiple threads.
    /// This class attempts to use a guaranteed store for request - level storage reqardless of threading.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AdaptivePropertyProvider<T>
    {
        private readonly string _propertyName;
        private readonly T _propertyValue;

        protected internal AdaptivePropertyProvider(string propertyName, T propertyValue)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            _propertyName = propertyName;
            _propertyValue = propertyValue;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[GetPropertyName()] = propertyValue;
            }
        }

        /// <summary>
        /// If there's an HttpContext, see if it has the value in the Items collection, if not use what you have.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (HttpContext.Current != null)
            {
                string item = HttpContext.Current.Items[GetPropertyName()] as string;
                if (!string.IsNullOrEmpty(item))
                {
                    return item;
                }
            }

            return (_propertyValue != null) ? _propertyValue.ToString() : null;
        }

        private string GetPropertyName()
        {
            return string.Format("{0}{1}", AdaptivePropertyProviderFactory.PropertyNamePrefix, _propertyName);
        }
    }

    /// <summary>
    /// Factory to create the value provider.
    /// </summary>
    public class AdaptivePropertyProviderFactory
    {
        public const string PropertyNamePrefix = "log4net_app_";

        public static AdaptivePropertyProvider<T> Create<T>(string propertyName, T propertyValue)
        {
            return new AdaptivePropertyProvider<T>(propertyName, propertyValue);
        }
    }
}
