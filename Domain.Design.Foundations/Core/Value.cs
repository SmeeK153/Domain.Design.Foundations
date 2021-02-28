using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Design.Foundations.Core
{
    /// <summary>
    /// Representation of a stateless abstraction
    /// </summary>
    public abstract class Value : IEquatable<Value>
    {
        /// <summary>
        /// Retrieves the <see cref="Value"/> instance's component values that comprise its identity for determining
        /// equality between different instances. A <see cref="Value"/> instance's uniqueness is defined only by its
        /// component values. If all of the component values between two <see cref="Value"/> instances match, and they
        /// are the same class type, then they are considered the same, even if any other properties differ.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of all the properties considered for determining equality between
        /// different <see cref="Value"/> instances</returns>
        protected abstract IEnumerable<object> GetComponentValues();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(Value? obj) => Equals((object?) obj);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() =>
            GetComponentValues()
                .Select(atomicValue => atomicValue != null ? atomicValue.GetHashCode() : 0)
                .Aggregate((aggregate, atomicValueHasCode) => aggregate ^ atomicValueHasCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Value left, Value right)
        {
            if (Equals(left, null)) return Equals(right, null) ? true : false;
            return left.Equals(right);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Value left, Value right) => !(left == right);
    }
}