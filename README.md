Domain.Design.Foundations
=======

![Status](https://img.shields.io/github/workflow/status/smeek153/Domain.Design.Foundations/build)
![C#](https://img.shields.io/github/languages/top/smeek153/Domain.Design.Foundations)
![Size](https://img.shields.io/github/repo-size/smeek153/Domain.Design.Foundations)
![License](https://img.shields.io/github/license/smeek153/Domain.Design.Foundations)
![Nuget](https://img.shields.io/nuget/v/Domain.Design.Foundations)
![Coverage Status](https://coveralls.io/repos/github/SmeeK153/Domain.Design.Foundations/badge.svg)

Simple domain driven design seeding components implementation in .NET

## Installing

Foundations may be installed via:

__Package Manager Console__

    Install-Package Domain.Design.Foundations
    
__.NET Core CLI__
 
     dotnet add package Domain.Design.Foundations
     
## Usage

Foundations is designed to work without external dependencies to leave domain implementations free to incorporate different technologies and patterns on top of them. Here are some quick examples to get started:

### ValueObject
ValueObjects are identified by their component values and may even be equated by those values. Each implementation of a ValueObject needs to provide which component values are to be considered for any equality operations. For edge-case situations in which one or more component values need to apply during equality operations, they may be omitted from this set of values, but in nearly every circumstance this set should represent all of the component values.

_The ValueObject provides the basis for the remaining Core Foundations._

```csharp
using Foundations.Core;

namespace Domain.ValueObjects
{
    public class ExampleValueObject : ValueObject
    {
        public string ExampleString { get; }
        public bool ExampleBool { get; }
        
        public ExampleValueObject(string exampleString, bool exampleBool)
        {
            ExampleString = exampleString;
            ExampleBool = exampleBool;
        }

        public override IEnumerable<object> GetComponentValues()
        {
            yield return ExampleString;
            yield return ExampleBool;
        }
    }
}
```

### Entity
Entities are identified by their individual Id value, which may not change, but their remaining component values may be altered. The default equality between entities is defined via their Id value and their remaining component values are ignored. Entities must be provided a handler for publishing their own domain events so that the appropriate effects may take place after the domain is finished. This technique exist in-place of integrating the Foundations with any other specific pattern or dependency to handle this operation.

```csharp
using System;
using Foundations.Core;

namespace Domain.Entities
{
    public class ExampleEntity : Entity<Guid>
    {
        public ExampleEntity(Guid id, Action<DomainEvent> domainEventPublisher) : base(id, domainEventPublisher)
        {
            // ...
        }
    }
}
```

### Enumeration
Instances of a derived Enumeration type must exist as static values, but they may exist as individual properties on their derived type or exist as an IEnumerable of their derived type; this exists to allow for defining preknown instances of the derived type, as well as, unknown instances that may need to be loaded later or generated. 

```csharp
using Foundations.Core;

namespace Domain.Enumerations
{
    public class ExampleEnumeration
    {
        public static readonly ExampleEnumeration PropertyExample = 
            new ExampleEnumeration(1, "PropertyExample");
        
        public static ExampleEnumeration BackedPropertyExample { get; } = 
            new ExampleEnumeration(2, "BackedPropertyExample");
        
        public static readonly IEnumerable<ExampleEnumeration> PropertyEnumerationEnumerableExample = 
            new List<ExampleEnumeration>
            {
                new ExampleEnumeration(3, "PropertyEnumerationEnumerableExample1"),
                new ExampleEnumeration(4, "PropertyEnumerationEnumerableExample2"),
                new ExampleEnumeration(5, "PropertyEnumerationEnumerableExample3")
            };
            
        public static IEnumerable<ExampleEnumeration> BackedPropertyEnumerationEnumerableExample { get; } = 
            new List<ExampleEnumeration>
            {
                new ExampleEnumeration(6, "BackedPropertyEnumerationEnumerableExample1"),
                new ExampleEnumeration(7, "BackedPropertyEnumerationEnumerableExample2"),
                new ExampleEnumeration(8, "BackedPropertyEnumerationEnumerableExample3")
            };
        
        public ExampleEnumeration(int id, string name) : base(id, name)
        {
        }
    }
}
```
