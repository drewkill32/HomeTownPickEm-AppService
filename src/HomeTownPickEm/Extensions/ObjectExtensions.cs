using System;
using HomeTownPickEm.Application.Exceptions;

namespace HomeTownPickEm.Extensions
{
    public static class ObjectExtensions
    {
        public static T GuardAgainstNotFound<T>(this T obj, string message = "")
        {
            if (obj == null)
            {
                throw new NotFoundException(message ?? $"{typeof(T).Name} was not found");
            }

            return obj;
        }

        public static T GuardAgainstNotFound<T>(this T obj, object key)
        {
            if (obj == null)
            {
                throw new NotFoundException(typeof(T).Name, key);
            }

            return obj;
        }

        public static object GuardAgainstNull(this object obj, string message = "")
        {
            if (obj == null)
            {
                throw new NullReferenceException();
            }

            return obj;
        }
    }
}