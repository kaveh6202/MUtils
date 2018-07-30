using System;

namespace MUtils.Cqrs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AdditionalActionAttribute : Attribute
    {
        
    }
}