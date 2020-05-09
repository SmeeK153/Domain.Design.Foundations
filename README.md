Domain.Design
=======

![Status](https://img.shields.io/github/workflow/status/smeek153/Domain.Design.Foundations/build)
![C#](https://img.shields.io/github/languages/top/smeek153/Domain.Design.Foundations)
![Size](https://img.shields.io/github/repo-size/smeek153/Domain.Design.Foundations)
![License](https://img.shields.io/github/license/smeek153/Domain.Design.Foundations)
![Nuget](https://img.shields.io/nuget/v/Domain.Design.Foundations)
![Coverage Status](https://coveralls.io/repos/github/SmeeK153/Domain.Design.Foundations/badge.svg)

Simple Domain Driven Design and Behavior Driven Development module implementations in .NET. 

## Introduction

Domain.Design was created to fulfill the need of having the seedwork components of a proper Domain Driven Design (DDD) implementation while
integrating the benefits of Behavioral Driven Development (BDD) in order to make designs more accessible to a greater audience of technical backgrounds.
The vision behind this endeavour is to create something that allows people from various backgrounds to collaborate more closely together to
create better software designs and systems. 

### Organization

The modules fall into one of two main categories, either DDD or BDD. Within the BDD category, there are four subcategories: unit, behavioral, contract and 
infrastructure; each sub-category of BDD serves a distinct and exclusive purpose. The


## Installing

Domain.Design is composed of multiple disparate projects in order to maintain a proper separation of concerns between modules; each must be 
installed separately. They may each be installed via either the Package Manager Console (or a package manager of your choice), or the .NET Core CLI.

The installation should be as follows with the corresponding package: 

__Package Manager Console__

    Install-Package Domain.Design.<Package Name>

    
__.NET Core CLI__

    dotnet add package Domain.Design.<Package Name>

The following packages are available:

* Domain.Design.Foundations - description
* Domain.Design.Testing.Infrastructure.Email - description
* Domain.Design.Testing.Behavioral.Moq - description

## Domain.Design.Foundations

Foundations is designed to work without external dependencies to leave domain implementations free to incorporate different technologies and patterns on top of them. Here are some quick examples to get started:

### ValueObject
#### Identity
ValueObjects are identified by their component values, which may not be altered once the instance is created.
The values used in the identity are defined via `GetComponentValues()` in order to allow all, or less than all,
component values to be included in the identity.

#### Equality
The same component values that comprise the identity of the ValueObject also serve as the basis for instance equality.

#### Effects
ValueObjects may not create effects on their domain.

#### Example
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
#### Identity
Entities are identified by their individual Id value, which may not change, but their remaining component values may be altered.
The underlying identity determination of an entity may not be changed.

#### Equality
The default equality between entities is defined via their Id value and their remaining component values are ignored.

#### Effects
In order for the effects (via `DomainEvent`s) created by the entity to be consumed, a DomainEventObserver must be instantiated to listen for them
within its respective context (i.e., sub-domain, infrastructure, etc.)

#### Example
```csharp
using System;
using Foundations.Core;

namespace Domain.Entities
{
    public class ExampleEntity : Entity<Guid>
    {
        public ExampleEntity(Guid id) : base(id)
        {
            // ...
        }
    }
}
```

### Enumeration
#### Identity
Entities are identified by their Id and Name values, which may not change.
The underlying identity determination of an enumeration may not be changed.
Must exist as a static value, either as an individual property or part of an enumerable of enumerations;
the enumerable exists to provide the means to dynamically load enumerations, if needed.

#### Equality
The default equality between enumerations is defined via their Id and Name values.

#### Effects
Enumerations may not create effects on their domain.

#### Example
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

## Domain.Design.Testing.Infrastructure.Email

Provides different clients for incorporating testing email integrations within the infrastructure of a system

## Domain.Design.Testing.Behavioral.Moq


