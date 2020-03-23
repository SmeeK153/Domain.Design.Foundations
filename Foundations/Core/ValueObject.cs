using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundations.Core
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        protected abstract IEnumerable<object> GetComponentValues();

        public ValueObject GetCopy()
        {
            if (!(this.MemberwiseClone() is ValueObject copy))
                throw new InvalidOperationException($"{this.GetType().Name} could not be copied");

            return copy;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            if (!(obj is ValueObject other))
                return false;

            IEnumerator<object> thisValues = GetComponentValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetComponentValues().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^ otherValues.Current is null)
                    return false;

                if (thisValues.Current != null && thisValues.Current.Equals(otherValues.Current))
                    return false;
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public bool Equals(ValueObject? obj)
        {
            if (obj is null)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            return Equals((object)obj);
        }

        public override int GetHashCode() =>
            GetComponentValues()
            .Select(atomicValue => atomicValue != null ? atomicValue.GetHashCode() : 0)
            .Aggregate((aggregate, atomicValueHasCode) => aggregate ^ atomicValueHasCode);

        
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (Object.Equals(left, null))
                return Object.Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);
    }
}
