# Supermarket_API

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

## Technologies used

- [ASP.NET Core 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- C#
- [MongoDb](https://www.mongodb.com) with MongoDb.[driver](https://www.mongodb.com/docs/languages/csharp/)
- [Swagger](www.google.com/search?client=opera-gx&q=swagger&sourceid=opera&ie=UTF-8&oe=UTF-8) for API documentation
- [Serilog](https://serilog.net) for logging
- [Node.js](https://nodejs.org/)