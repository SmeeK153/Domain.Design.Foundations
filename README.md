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



## Domain.Design.Testing.Infrastructure.Email

Provides different clients for incorporating testing email integrations within the infrastructure of a system

## Domain.Design.Testing.Behavioral.Moq


