using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Foundations.Core
{
    public class Enumeration : ValueObject
    {
        public string Name { get; }

        public int Id { get; }

        protected Enumeration(int id, string name) =>
            (Id, Name) = (id, name);

        public sealed override string ToString() => Name;
        
        private static IEnumerable<T> SelectFieldType<T>(List<FieldInfo> fields)
        {
            var applicableFields =
                fields.Where(field => field.FieldType?.FullName?.Equals(typeof(T).FullName) ?? false)
                    .Select(field => field.GetValue(null))
                    .Cast<T>();
            return applicableFields;
        }
        
        public static IEnumerable<TEnumeration> GetAll<TEnumeration>() where TEnumeration : Enumeration
        {
            var fields = typeof(TEnumeration).GetRuntimeFields().ToList();

            var singularFields = SelectFieldType<TEnumeration>(fields);

            var enumeratedFields = SelectFieldType<IEnumerable<TEnumeration>>(fields)
                .SelectMany(enumerationFields => enumerationFields);

            var combinedEnumerations = singularFields.Concat(enumeratedFields);
            
            return combinedEnumerations;
        }

        public static TEnumeration FromId<TEnumeration>(int id) where TEnumeration : Enumeration =>
            Parse<TEnumeration>(e => e.Id == id);

        public static TEnumeration FromName<TEnumeration>(string name) where TEnumeration : Enumeration =>
            Parse<TEnumeration>(e => e.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()));

        private static TEnumeration Parse<TEnumeration>(Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration
        {
            var match = GetAll<TEnumeration>().SingleOrDefault(predicate);
            if (match is null)
                throw new InvalidOperationException($"No matching enumeration of {typeof(TEnumeration)}");
            return match;
        }

        protected sealed override IEnumerable<object> GetComponentValues()
        {
            yield return Id;
            yield return Name;
        }
    }
}
