using System;
using System.Web.Mvc;

namespace Buche
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class Loggable : FilterAttribute
    {
    }
}
