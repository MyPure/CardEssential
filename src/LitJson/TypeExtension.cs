using System;
using System.Linq;

namespace LitJson
{
    public static class TypeExtension
    {
        public static Type GetGenericInterface(this Type type, Type generic)
        {
            return type.GetInterfaces().First(x => generic == (x.IsGenericType ? x.GetGenericTypeDefinition() : x));
        }
        
        public static bool HasGenericInterface(this Type type, Type generic)
        {
            return type.GetInterfaces().Any(x => generic == (x.IsGenericType ? x.GetGenericTypeDefinition() : x));
        }
    }
}