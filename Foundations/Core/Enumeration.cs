using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Foundations.Core
{
    public class Enumeration
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string name) =>
            (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<TEnumeration> GetAll<TEnumeration>() where TEnumeration : Enumeration =>
            typeof(TEnumeration).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(field => field.GetValue(null))
            .Cast<TEnumeration>();

        public static TEnumeration FromId<TEnumeration>(int id) where TEnumeration : Enumeration =>
            Parse<TEnumeration>(e => e.Id == id);

        public static TEnumeration FromName<TEnumeration>(string name) where TEnumeration : Enumeration =>
            Parse<TEnumeration>(e => e.Name.ToLowerInvariant() == name.ToLowerInvariant());

        private static TEnumeration Parse<TEnumeration>(Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration
        {
            var match = GetAll<TEnumeration>().FirstOrDefault(predicate);
            if (match is null)
                throw new InvalidOperationException($"No matching enumeration of {typeof(TEnumeration)}");

            return match;
        }

        public override bool Equals(object? obj) => this.Equals(obj as Enumeration);

        public bool Equals(Enumeration? enumeration)
        {
            if (enumeration is null)
                return false;

            if (Object.ReferenceEquals(this, enumeration))
                return true;

            if (Name.ToLowerInvariant().Equals(enumeration.Name.ToLowerInvariant()))
                return base.Equals(enumeration);
            else
                return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (Object.Equals(left, null))
                return Object.Equals(right, null);
            else
                return left.Equals(right);
            
        }

        public static bool operator !=(Enumeration left, Enumeration right) => !(left == right);
    }
}
