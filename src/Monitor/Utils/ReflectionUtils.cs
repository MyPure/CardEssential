using System;
using System.Reflection;

namespace CardEssential.Monitor.Utils;

public static class ReflectionUtils
{
    public static object ExecuteMethod<T>(string methodName, BindingFlags bindingFlags, object instance = null, object[] parameters = null)
    {
        return ExecuteMethod(typeof(T), methodName, bindingFlags, instance, parameters);
    }
    
    public static object ExecuteMethod(Type targetType, string methodName, BindingFlags bindingFlags, object instance = null, object[] parameters = null)
    {
        var method = targetType.GetMethod(methodName, bindingFlags);
        if (method != null)
        {
            return method.Invoke(instance, parameters);
        }

        return null;
    }
}