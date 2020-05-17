## Domain.Design.Foundations

Foundations is designed to work without external dependencies to leave domain 
implementations free to incorporate different technologies and patterns on top 
of them. Here are some quick examples to get started:

### ValueObject
#### Identity
ValueObjects are identified by their component values, which may not be altered 
once the instance is created. The values used in the identity are defined via 
`GetComponentValues()` in order to allow all, or less than all, component values 
to be included in the identity.

#### Equality
The same component values that comprise the identity of the ValueObject also 
serve as the basis for instance equality.

#### Effects
ValueObjects may not create effects on their domain.

#### Example
```csharp
using Foundations.Core;

namespace Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public string First { get; }
        public string Middle { get; }
        public string Last { get; }
        
        public Name(string first, string middle, string last)
        {
            if (string.IsNullOrWhitespace(first))
            {
                throw new MissingRequiredValueObjectFieldException($"{nameof(first)} must be provided for {nameof(Name)}");
            }
            First = first;

            if (string.IsNullOrWhitespace(last))
            {
                throw new MissingRequiredValueObjectFieldException($"{nameof(last)} must be provided for {nameof(Name)}");
            }
            Last = last;

            Middle = middle;
        }

        public override IEnumerable<object> GetComponentValues()
        {
            yield return First;
            yield return Last;
            yield return Middle;
        }
    }
}
```

### Entity
#### Identity
Entities are identified by their individual Id value, which may not change, but their 
remaining component values may be altered. The underlying identity determination of an 
entity may not be changed.

#### Equality
The default equality between entities is defined via their Id value and their remaining 
component values are ignored.

#### Effects
In order for the effects (via `DomainEvent`s) published by the entity to be consumed, 
an observer must subscribe to the specific entity's events within its respective context 
(i.e., sub-domain, infrastructure, etc.) via `Subscribe(IObserver<DomainEvent> observer)`;
the result yields a disposable subscription to facilitate the unsubscription process.

#### Example
```csharp
using System;
using Foundations.Core;

namespace Domain.Entities
{
    public class Employee : Entity<Guid>
    {
        public Name Name { get; private set; }
        
        public Employee(Guid id, Name name) : base(id)
        {
            Name = name;
            // ...
        }
    }
}
```

### Enumeration
#### Identity
Entities are identified by their Id and Name values, which may not change.
The underlying identity determination of an enumeration may not be changed.
Must exist as a static value, either as an individual property or part of 
an enumerable of `Enumeration`s; the enumerable exists to provide the means 
to dynamically load `Enumeration`s, if needed.

#### Equality
The default equality between enumerations is defined via their Id and Name values.

#### Effects
Enumerations may not create effects on their domain.

#### Example
```csharp
using Foundations.Core;

namespace Domain.Enumerations
{
    public class Color : Enumeration
    {
        // Property example
        public static readonly Color White = new Color(1, "White");
        
        // Backed-Property example
        public static Color Black { get; } = new Color(2, "Black");
        
        // Enumerable property example
        public static readonly IEnumerable<Color> PrimaryColors = new List<Color>
        {
            new Color(3, "Red"),
            new Color(4, "Yellow"),
            new Color(5, "Blue")
        };
            
        // Enumerable Backed-property example 
        public static IEnumerable<Color> SecondaryColors { get; } = new List<Color>
        {
            new Color(6, "Green"),
            new Color(7, "Orange"),
            new Color(8, "Purple")
        };
        
        public Color(int id, string name) : base(id, name)
        {
        }
    }
}
```