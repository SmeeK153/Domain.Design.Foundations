## Domain.Design.Foundations

Foundations is designed to work without external dependencies to leave domain 
implementations free to incorporate different technologies and patterns on top 
of them. Here are the three (3) basic building blocks and a quick example for each:

### ValueObject
#### Identity
`ValueObject`s are immutable objects that represent conceptual identities that 
which are formed by their component values (as opposed to being identified by a 
unique identifier).

#### Equality
Equivalence between `ValueObject`s is determined by the values specified in and 
only the values specified in `GetComponentValues()`.

#### Effects
`ValueObject`s may not create any effects.

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
`Entity`'s are mutable objects that represent a reference to an underlying concept 
that may have changing properties, the component values of the concept may change
without affecting the underlying identity of the concept.

#### Equality
Equivalence between `Entity`'s is determined by their `Id` value and only by their 
`Id` value; all remaining component values are ignored.

#### Effects
`Entity`'s have the ability to create effects for state changes to be propagated to any
subsequent layers and/or sub-systems. In order to facilitate this process, any `Entity`
has the ability to emit events via `PublishDomainEvent(DomainEvent domainEvent)`. In order
for subsequent systems to consume these events, they must subscribe to the respective
`Entity` before any effects are created via `Subscribe(IObserver<DomainEvent> observer)`.

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
`Enumeration`s are immutable objects that represent constant identities that 
which are formed by their `Id` and `Name` to provide a consistent definition of the same
type of related concepts. Constant definitions may be hard-coded, or loaded statically
to provide a dynamic listing of constant values at runtime.

#### Equality
Equivalence between `Enumeration`s is determined by their `Id` and `Name` values; 
all remaining component values are ignored.

#### Effects
`Enumeration`s may not create any effects.

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