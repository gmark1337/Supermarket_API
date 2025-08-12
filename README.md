# postwall-api

## Contents 

- [Introduction](#introduction)
- [Features](#features)
- [Where can I see the actual website?](#website)
- [Documentation](#documentation)
- [Technologies used](#technologies-used)

## Introduction

This service built with **ASP.NET Core Web Api** that integrates with a **MongoDB** database to manage and store previous flyer's. \
Also, communicates with my [custom built](https://github.com/gmark1337/scraper) **Node.js** webscraper.\
The database is currently self hosted,but I'm considering migrating with the **MongoDb Atlas** cloud service.


>⚠️**Disclaimer:** 
>I understand, that I could've built this in *Node.js*, but for practice and challenge I'm writing this is C#.

## Features

- Fetch flyer data from an extrenal Node.js API.
- Store flyer data in MongoDb database.
- Retrieve flyers by SupermarketId.
- Prevents duplication inserts.
- Logging

## Website

You can find this data on the **[website](https://thankful-rock-027ccfc03.2.azurestaticapps.net)**.


## Documentation

**Currently**, my database availability is currently private, but on request I can grant access for the data.

# Classes

There is 2 classes which handles the data. 1 for the URL flyers and 1 for the PDF flyers. 

*FlyerPDF*

| Name    | Purpose |
| --------      | ------- |
| PdfID         | Creates an id for each object    |
| SupermarketID | Stores an id for each large supermarket brand|
| ActualDate    | Stores the actualitiy of the flyer    |
| FirstPageURL  | Stores the first page of the flyer, since it won't have URL's with pages    |
| PdfURL        | Stores the URL of the flyer PDF    |

*Flyer*

| Name    | Purpose |
| --------      | ------- |
| flyerID         | Creates an id for each object    |
| SupermarketID | Stores an id for each large supermarket brand|
| PageIndex    |  Stores the index of the page   |
| ImageURL  | Stores the URL page of the flyer    |
| ActualDate        | Stores the actualitiy of the flyer    |

>The flyer class is **wrapped** in a FlyerImages class which has an **ActualDate** and a *(List)* **Pages** object.They are both wrapped in a **NodeResponse** class, which contains a *(dictionary)* **data** object.
>This matches my json structure.

# Services

There is only 3 service registered, which is **ExternalPdfService**, **ExternalFlyerService** and **MongoDbService**. 

*ExternalPdfService*
Fetches data from a NodeJs api which matches the **FlyerPdf** class structure.
The supermarketId currently contains only the number "3", since it's the only supermarket with PDF content.

```csharp
FetchPdfFromNodeApi(string supermarketId)
```
*Returns an object with **FlyerPDF** data.*

*ExternalFlyerService*
Fetches data from a NodeJs api which matches the **Flyer** class structure.
The supermarketId currently contains "1" (lidl), "2" (spar) strings. 

```csharp
FetchFromNodeApi(string supermarketId)
```

*Returns a List with **Flyer** data.*

*MongoDbService*

Inserts the collected data, into the *Flyers* database. It has a filter, to match the correct markets. 

```csharp
SaveFlyersAsync(List<Flyer> flyers, string supermarketId)
```

Inserts the collected data, into the *FlyerPDF* database. It has a filter, to match the correct markets.

```csharp
SaveFlyerPdfAsync(FlyerPDF flyerObject, string supermarketId)
```

Filters out data by *actualDate*. SupermarketId is irrelevant, because one object only has 1 date.
```csharp
GetFlyerPdfbyActualDateASync(string actualDate)
```
*Returns an object with the filtered item.*

Filters out data by *actualDate*. SupermarketId is irrelevant, because one List only has 1 date.

```csharp
GetFlyersByActualDateAsync(string actualDate)
```

*Returns a list with the filtered items.*

Filters out data by *supermarketId*.

```csharp
GetFlyersAsync(string supermarketId)
```

*Returns a list with the filtered items.* 


Filters out data by *supermarketId*.

```csharp
GetFlyerPdfAsync(string supermarketId)
```

*Returns an object with **FlyerPDF** data.*

Filters data by **actualDate** and **supermarketId**.It is used to prevent any duplicates before insterting them into the database.

```csharp
FlyersExistAsync(string actualDate, string supermarketId)
```
*Returns a boolean according to the filter value.*

Filters data by **actualDate** and **supermarketId**.It is used to prevent any duplicates before insterting them into the database.

```csharp
FlyerPdfExistAsync(string supermarketId, string actualDate)
```
*Returns a boolean according to the filter value.*



## Technologies used

- [ASP.NET Core 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- C#
- [MongoDb](https://www.mongodb.com) with MongoDb.[driver](https://www.mongodb.com/docs/languages/csharp/)
- [Swagger](www.google.com/search?client=opera-gx&q=swagger&sourceid=opera&ie=UTF-8&oe=UTF-8) for API documentation
- [Serilog](https://serilog.net) for logging
- [Node.js](https://nodejs.org/)
- [Github actions](https://github.com/features/actions) providing the CI/CD pipeline.
- [Microsoft Azure](https://azure.microsoft.com/hu-hu) for providing web app service hosting.
