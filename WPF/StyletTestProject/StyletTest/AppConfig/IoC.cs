using System;
using System.Collections.Generic;
using System.Linq;

namespace StyletTest.AppConfig;

public static class IoC
{
    public static Func<Type, string, object> GetInstance { get; set; } = (_, _) => throw new InvalidOperationException("IoC is not initialized");
    public static Func<Type, string, IEnumerable<object>> GetAllInstances { get; set; } = (_, _) => throw new InvalidOperationException("IoC is not initialized");
    public static Action<object> BuildUp { get; set; } = _ => throw new InvalidOperationException("IoC is not initialized");

    public static T Get<T>(string? key = null)
    {
        return (T)GetInstance(typeof(T), key!);
    }

    public static IEnumerable<T> GetAll<T>(string? key = null)
    {
        return GetAllInstances(typeof(T), key!).Cast<T>();
    }
}