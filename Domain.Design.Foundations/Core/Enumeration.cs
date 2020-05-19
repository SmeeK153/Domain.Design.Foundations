using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Design.Foundations.Core
{
    /// <summary>
    /// Unique representation of a specific type of state
    /// </summary>
    public class Enumeration : ValueObject
    {
        /// <summary>
        /// Name of the Enumeration instance
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Identifier of the Enumeration instance
        /// </summary>
        public int Id { get; }

        protected Enumeration(int id, string name) =>
            (Id, Name) = (id, name);
        
        /// <summary>
        /// Gets the string Name of the derived type instance
        /// </summary>
        public sealed override string ToString() => Name;

        /// <summary>
        /// Retrieves all of the derived type's static instances
        /// </summary>
        /// <typeparam name="TEnumeration"></typeparam>
        public static IEnumerable<TEnumeration> GetAll<TEnumeration>() where TEnumeration : Enumeration
        {
            var fields = typeof(TEnumeration).GetRuntimeFields().ToList();

            var singularFields = SelectFieldType<TEnumeration>(fields);

            var enumeratedFields = SelectFieldType<IEnumerable<TEnumeration>>(fields)
                .SelectMany(enumerationFields => enumerationFields);

            var combinedEnumerations = singularFields.Concat(enumeratedFields);
            
            return combinedEnumerations;
        }
        
        /// <summary>
        /// Retrieves the specific derived type's static instance based on its identifier
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="TEnumeration"></typeparam>
        /// <exception cref="InvalidOperationException">Derived type does not have a static instance with the specified identifier</exception>
        public static TEnumeration FromId<TEnumeration>(int id) where TEnumeration : Enumeration =>
            Parse<TEnumeration>(e => e.Id == id);
        
        /// <summary>
        /// Retrieves the specific derived type's static instance based on its name
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TEnumeration"></typeparam>
        /// <exception cref="InvalidOperationException">Derived type does not have a static instance with the specified name</exception>
        public static TEnumeration FromName<TEnumeration>(string name) where TEnumeration : Enumeration =>
            Parse<TEnumeration>(e => e.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()));
        
        private static IEnumerable<T> SelectFieldType<T>(List<FieldInfo> fields)
        {
            var applicableFields =
                fields.Where(field => field.FieldType?.FullName?.Equals(typeof(T).FullName) ?? false)
                    .Select(field => field.GetValue(null))
                    .Cast<T>();
            return applicableFields;
        }
        
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
