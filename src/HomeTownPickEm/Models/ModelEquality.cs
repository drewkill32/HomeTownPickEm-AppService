using System;
using System.Collections.Generic;

namespace HomeTownPickEm.Models
{
    public class ModelEquality<TModel> : IEqualityComparer<TModel>
    {
        private ModelEquality()
        {
        }

        public static IEqualityComparer<TModel> IdComparer => new ModelEquality<TModel>();

        public bool Equals(TModel x, TModel y)
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

            var idProp = x.GetType().GetProperty("Id");
            if (idProp == null)
            {
                throw new InvalidOperationException($"The type {x.GetType()} does not have an Id property");
            }

            var xVal = idProp.GetValue(x) ??
                       throw new NullReferenceException($"The Id Property on {typeof(TModel)} is null");
            var yVal = idProp.GetValue(y) ??
                       throw new NullReferenceException($"The Id Property on {typeof(TModel)} is null");
            if (idProp.PropertyType != typeof(int))
            {
                return xVal.Equals(yVal);
            }

            if ((int)xVal == 0 && (int)yVal == 0)
            {
                return false;
            }

            return xVal.Equals(yVal);
        }

        public int GetHashCode(TModel obj)
        {
            var idProp = obj.GetType().GetProperty("Id");
            return idProp.GetValue(obj).GetHashCode();
        }
    }
}