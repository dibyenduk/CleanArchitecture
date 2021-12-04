# Clean Architecture

## Introduction
---

Clean Architecture is a book by author Robert Martin. It defines patterns to address function, component separation and data management by structuring classes and components within your code so that they are easy to change, maintain and deploy. 

The source code in this [repository](https://github.com/dibyenduk/CleanArchitecture/tree/main/src) is my take or attempt to implement the same.

Vertical Slice Architecture is another architecture by Jimmy Bogard. I had the opportunity to work with Jimmy on a project and learn about this architecture from him.

I like the approaches in both the patterns and hence this my attempt to combine them to pick up the theorotical aspects from Clean architecture and the implementation aspect from Vertical slice architecture while maintaining the abstraction and layer approach talked about in Clean architecture book.

For this example, I have selected a manufacturing execution system. This is a system used in manufacturing to track and document the transformation of raw materials to finished goods. MES works in real time to enable the control of multiple elements of the production process (e.g. inputs, outputs, personnel, machines and support services)

## Clean Architecture
---

As part of laying out the architecture for an application, the responsibility of an architect is to be able to provide a shape to the system so that it can be easily developed, maintained and released. Additionally it should be flexible enough for changes that show up during the lifecycle of the application. The shape of application is in the division of the system into components, arrangement of those components and interaction between the components.

Clean architecture suggests to divide the components in high level and low level components and draw boundaries between the two using the dependency inversion principle. Idea is that source code dependency always flows from a low level component to a higher level component and there should not be any dependency in the other direction. This allows the higher level component to be independent of the low level components, allowing the lower level components to act as plugins to the higher component.

![](src/images/CleanArchitecture.jpg)

Source - [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## Vertical Slice Architecture
---

In the vertical slice architecture, the application is built around requests, handlers and responses. Each request satisfies a use case of the system. All requests are broken into queries and commands providing pattern like CQRS.

The architecture talks about removing all the horizontal layers in an n tier, onion or layered architecture and replacing it with vertical layers encompassing UI, Business and Data. Instead of coupling across layers, the coupling is within the vertical slice.

![](src/images/VerticalSliceArchitecture.png)

Source - [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/)

## Project Structure
---

[**MES.Entities**](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.Entities) -

This project contains the entities of the system. It contains the highest level policy or the enterprise business critical rules. Since our application is not an enterprise wide application shared by multiple other applications, this project just provides the business objects for the application.

[**MES.Core**](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.Core) -

This project contains the application logic for our application. It depends on the MES.Entities project for business objects and uses abstractions or interfaces to talk to any other infrastructure dependent tools. . All infrastructure tools can be replaced as plugins within the architecture with alternate tools. StructureMap is used as a container for enabling dependency injection for these tools. Infrastructure elements are injection through the constructors of the handlers. 

One exception to this is the persistence layer. I took the decision early on to avoid abstracting the entity framework into another repository layer as entity framework DBContext itself is an abstraction over databases like Oracle, SQL Server etc. To help with unit testing, I made use of the Entity Framework Effort initiative which allows using entity framework for an in memory database. This makes it possible to treat entity framework as just a plugin for the architecture.

All use cases are treated as individual requests. The requests are also aware about the response for the request. Handler handles the request and returns the response. Data validations are handled using Fluent validations. MediatR is used for routing the requests to the handlers. Retreiving or Saving data is handled using MES.Persistence and any other infrastructure elements are accessed through interfaces or abstractions.

[**MES.Persistence**](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.Persistence) -

This project handles retrieving from and saving data in database. Configuration for the database is done on the UI or Test project. 

The DBContext class contains two constructors, one takes connectionstring for the database and the other takes an existing connection. The one with the connectionstring is used by the user interface project [MES.UI](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.UI) whereas the constructor with existing connection is used by the test project [MES.Core.Tests](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.Core.Tests). The constructor to use for MESDBContext is determined in the UI and Test project using dependency injection container defined by Structuremap.

[**MES.UI**](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.UI)

[**MES.Infrastructure**](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.Infrastructure)

[**MES.SAP**](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.SAP)

[**MES.Core.Tests**](https://github.com/dibyenduk/CleanArchitecture/tree/main/src/MES.Core.Tests)
