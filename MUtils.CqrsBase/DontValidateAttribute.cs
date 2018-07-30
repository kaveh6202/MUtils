using System;

namespace MUtils.CqrsBase {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DontValidateAttribute:Attribute {
        
    }
}