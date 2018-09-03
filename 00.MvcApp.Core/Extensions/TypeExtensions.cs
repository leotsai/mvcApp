using System;

namespace MvcApp.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool Eq(this Type type, Type toCompare)
        {
            return type.Namespace == toCompare.Namespace && type.Name == toCompare.Name;
        }
    }
}
