using System;

namespace HomeTownPickEm.Extensions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message)
        {
        }

        public ForbiddenAccessException()
        {
        }
    }
}