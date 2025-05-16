using System;

namespace TP_M2I_DOTNET
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromServicesAttribute : Attribute
    {
        public string Name { get; }

        public FromServicesAttribute()
        {
        }

        public FromServicesAttribute(string name)
        {
            Name = name;
        }
    }
}
