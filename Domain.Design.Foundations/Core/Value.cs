using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Design.Foundations.Core
{
    /// <summary>
    /// Unique representation of a fixed, stateless abstraction
    /// </summary>
    public abstract class Value : IEquatable<Value>
    {
        /// <summary>
        /// Provides the applicable values of the derived type that should be considered in equality operations
        /// </summary>
        protected abstract IEnumerable<object> GetComponentValues();

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            
            if (!(obj is Value other))
                return false;
            
            if (obj.GetType() != GetType())
                return false;

            using IEnumerator<object> thisValues = GetComponentValues().GetEnumerator();
            using IEnumerator<object> otherValues = other.GetComponentValues().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null && otherValues.Current is null)
                    continue;
                
                if (thisValues.Current is null ^ otherValues.Current is null)
                    return false;

                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                    return false;
            }

            var thisValuesEnd = !thisValues.MoveNext();
            var otherValuesEnd = !otherValues.MoveNext();
            return thisValuesEnd && otherValuesEnd;
        }

        public bool Equals(Value? obj) => Equals((object?)obj);
        
        public override int GetHashCode() =>
            GetComponentValues()
            .Select(atomicValue => atomicValue != null ? atomicValue.GetHashCode() : 0)
            .Aggregate((aggregate, atomicValueHasCode) => aggregate ^ atomicValueHasCode);
        
        public static bool operator ==(Value left, Value right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            return left.Equals(right);
        }

        public static bool operator !=(Value left, Value right) => !(left == right);
    }
}
