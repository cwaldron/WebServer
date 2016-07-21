using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.Sessions
{
    internal static class Converter
    {
        private static readonly Dictionary<Type, Func<string, object>> Converters = new Dictionary
            <Type, Func<string, object>>
        {
            {typeof(bool), x => bool.Parse(x)},
            {typeof(byte[]), x => Encoding.Default.GetBytes(x)},
            {typeof(byte), x => byte.Parse(x)},
            {typeof(char), x => char.Parse(x)},
            {typeof(Guid), x => new Guid(x)},
            {typeof(int), x => int.Parse(x)},
            {typeof(long), x => long.Parse(x)},
            {typeof(short), x => short.Parse(x)},
            {typeof(float), x => float.Parse(x)},
            {typeof(double), x => double.Parse(x)},
            {typeof(decimal), x => decimal.Parse(x)},
            {typeof(DateTime), x => DateTime.Parse(x)},
            {typeof(string), x => x}
        };

        public static T Deserialize<T>(string serialize)
        {
            Func<string, object> converter;
            if (!Converters.TryGetValue(typeof(T), out converter))
            {
                throw new ArgumentException($"Unsupported type: {typeof(T).Name}");
            }

            return (T) converter(serialize);
        }
    }
}