using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Design.Foundations.Events;

namespace Domain.Design.Foundations.Core.Abstract
{
    /// <summary>
    /// Unique representation of a specific abstraction of state
    /// </summary>
    /// <typeparam name="T">The type of identity this stateful abstraction will use to distinguish itself from other
    /// instances</typeparam>
    public abstract class Enumeration<T> : Value
    {
        /// <summary>
        /// Friendly description of the <see cref="Enumeration{T}"/> instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Unique identifier of the <see cref="Enumeration{T}"/> instance.
        /// </summary>
        public T Id { get; }

        /// <summary>
        /// Create a new instance of a <see cref="Enumeration{T}"/>.
        /// </summary>
        /// <param name="id">Unique identity of the <see cref="Enumeration{T}"/> instance</param>
        /// <param name="name">Friendly name describing the <see cref="Enumeration{T}"/> instance</param>
        protected Enumeration(T id, string name)
        {
            Id = id ?? throw new DomainException($"Id is required for entity {GetType().Name}");
            Name = name ?? throw new DomainException($"Name is required for entity {GetType().Name}");
        }
        
        /// <summary>
        /// Returns a description that represents the <see cref="Enumeration{T}"/> instance.
        /// </summary>
        /// <returns>The <see cref="Name"/> of the <see cref="Enumeration{T}"/> instance.</returns>
        public sealed override string ToString() => Name;

        /// <summary>
        /// Retrieves all of the static instances of <typeparamref name="TEnumeration"/> defined either as static
        /// properties or within a static enumerable on the derived type.
        /// </summary>
        /// <typeparam name="TEnumeration">Derived type of <see cref="Enumeration{T}"/></typeparam>
        /// <returns>All of the static definitions of the derived type of <see cref="Enumeration{T}"/></returns>
        public static IEnumerable<TEnumeration> GetAll<TEnumeration>() where TEnumeration : Enumeration<T>
        {
            // Retrieve all of the fields defined on the derived type
            var fields = typeof(TEnumeration).GetRuntimeFields().ToList();

            // Prepare to aggregate each of the static instances of the derived type from supported definition techniques
            var enumerations = new List<TEnumeration>();
            
            // Lookup the singular fields that are defined as static properties on the derived type
            enumerations.AddRange(SelectFieldType<TEnumeration>(fields));

            // Lookup the enumerated fields that are defined in an enumerable on the derived type in a static context
            enumerations.AddRange(SelectFieldType<IEnumerable<TEnumeration>>(fields)
                .SelectMany(enumerationFields => enumerationFields));

            return enumerations;
        }
        
        /// <summary>
        /// Retrieves the specific static instances of <typeparamref name="TEnumeration"/> based on its <see cref="Id"/>.
        /// </summary>
        /// <param name="id">Unique identity of the <see cref="Enumeration{T}"/> instance to search for</param>
        /// <typeparam name="TEnumeration">Derived type of <see cref="Enumeration{T}"/></typeparam>
        /// <returns>Instance of the derived type of <see cref="Enumeration{T}"/> with the matching <see cref="Id"/></returns>
        /// <exception cref="InvalidOperationException">Derived type does not have a static instance with the specified
        /// identifier</exception>
        public static TEnumeration FromId<TEnumeration>(T id) where TEnumeration : Enumeration<T> =>
            Parse<TEnumeration>(e => Equals(e.Id, id));
        
        /// <summary>
        /// Retrieves the specific static instances of <typeparamref name="TEnumeration"/> based on its <see cref="Name"/>.
        /// </summary>
        /// <param name="name">Friendly name describing the <see cref="Enumeration{T}"/> instance to search for</param>
        /// <typeparam name="TEnumeration">Derived type of <see cref="Enumeration{T}"/></typeparam>
        /// <returns>Instance of the derived type of <see cref="Enumeration{T}"/> with the matching <see cref="Name"/></returns>
        /// <exception cref="InvalidOperationException">Derived type does not have a static instance with the specified
        /// name</exception>
        public static TEnumeration FromName<TEnumeration>(string name) where TEnumeration : Enumeration<T> =>
            Parse<TEnumeration>(e => e.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()));
        
        /// <summary>
        /// Retrieves the field definitions that match the requested type, useful for finding instances of
        /// <see cref="Enumeration{T}"/> defined on a derived type.
        /// </summary>
        /// <param name="fields">Definition information for the fields defined on the derived type of
        /// <see cref="Enumeration{T}"/></param>
        /// <typeparam name="TFieldType">Field type to search for on the derived type of <see cref="Enumeration{T}"/>
        /// </typeparam>
        /// <returns></returns>
        private static IEnumerable<TFieldType> SelectFieldType<TFieldType>(IEnumerable<FieldInfo> fields)
        {
            var applicableFields =
                fields.Where(field => field.FieldType?.FullName?.Equals(typeof(TFieldType).FullName) ?? false)
                    .Select(field => field.GetValue(null))
                    .Cast<TFieldType>();
            return applicableFields;
        }
        
        /// <summary>
        /// Retrieves the desired instance of the derived type of <see cref="Enumeration{T}"/> based upon a specific
        /// criteria, or raises an error if an instance doesn't exist that matches the criteria.
        /// </summary>
        /// <param name="predicate">Comparison strategy to uniquely identify the desired instance of the derived type of
        /// <see cref="Enumeration{T}"/>, if it exists</param>
        /// <typeparam name="TEnumeration">Derived type of <see cref="Enumeration{T}"/></typeparam>
        /// <returns>Instance of the derived type of <see cref="Enumeration{T}"/> that matches the predicate</returns>
        /// <exception cref="InvalidOperationException">Derived type does not have a static instance satisfying the
        /// specified criteria</exception>
        private static TEnumeration Parse<TEnumeration>(Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration<T>
        {
            var match = GetAll<TEnumeration>().SingleOrDefault(predicate);
            if (match is null)
                throw new InvalidOperationException($"No matching enumeration of {typeof(TEnumeration)}");
            return match;
        }

        /// <summary>
        /// Retrieves the <see cref="Enumeration{T}"/> instance's component values that comprise its identity for
        /// determining equality between different instances. An <see cref="Enumeration{T}"/> instance's uniqueness is
        /// defined only by the value of its <see cref="Id"/> and <see cref="Name"/>. If the <see cref="Id"/> and
        /// <see cref="Name"/> between two <see cref="Entity"/> instances match, and they are the same class type, then
        /// they are considered the same. Since <see cref="Enumeration{T}"/>s don't use other properties than
        /// <see cref="Id"/> and <see cref="Name"/>, there is no concern with other properties even being a concern.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of two properties, the <see cref="Enumeration{T}"/> instance's
        /// <see cref="Id"/> and <see cref="Name"/> values</returns>
        protected sealed override IEnumerable<object> GetComponentValues()
        {
            if (Id != null) yield return Id;
            yield return Name;
        }
    }
}
