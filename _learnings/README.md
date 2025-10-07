# .NET CRUD API Learning Guide

This guide walks you through building a complete CRUD (Create, Read, Update, Delete) API in ASP.NET Core from scratch. It's based on the Hotel Booking API project, using C#, Entity Framework Core, and PostgreSQL.

## Prerequisites

- .NET 8 SDK
- PostgreSQL database
- Visual Studio Code or Visual Studio
- Basic knowledge of C# and REST APIs

## Table of Contents

1. [Project Setup](01-Project-Setup.md)
2. [Creating Models](02-Creating-Models.md)
3. [Setting Up Database Context](03-Database-Context.md)
4. [Creating Controllers](04-Creating-Controllers.md)
5. [Adding DTOs](05-Adding-DTOs.md)
6. [Entity Framework Migrations](06-Migrations.md)
7. [Running and Testing the API](07-Running-API.md)
8. [Adding Authentication (Optional)](08-Authentication.md)

Each file contains code examples and explanations. Follow them in order to build a working API.

## What You'll Build

A simple Book management API with endpoints for:

- GET /api/books (list all books)
- GET /api/books/{id} (get book by ID)
- POST /api/books (create new book)
- PUT /api/books/{id} (update book)
- DELETE /api/books/{id} (delete book)

## Tips

- Run `dotnet build` after each major change to check for errors.
- Use Swagger UI (available at `/swagger` when running) to test endpoints.
- Refer to the existing project files for real examples.

Last Updated: October 7, 2025
