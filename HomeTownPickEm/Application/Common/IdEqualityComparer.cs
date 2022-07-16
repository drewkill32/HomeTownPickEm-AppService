using System.Collections.Generic;

namespace HomeTownPickEm.Application.Common
{
    public class IdEqualityComparer<T> : IEqualityComparer<T> where T : IHasId
    {
        
        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id;
        }
    }
}