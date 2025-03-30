# Shared Database Architecture with Range-Based Partitioning in ASP.NET Core (ID-Based)

## Overview

This project demonstrates a shared database architecture with range-based partitioning using ASP.NET Core, PostgreSQL, and a Movies API.  In this scenario, the partitioning is based on the `Id` of the `Movie` entity. This might be used to ensure data is distributed evenly across partitions, particularly if the `Id` values are generated in a sequential manner.

## Architecture

### Shared Database
*   Multiple "logical tenants" (data segments based on ID ranges) share the same database instance.
*   Data segregation is managed via partitioning based on an `Id` range.

### Range-Based Partitioning (PostgreSQL)
*   Tables are partitioned based on ranges of `Movie.Id` values (UUID in this case).
*   Each partition contains movies whose `Id` falls within a specific range.
*   This partitioning aims to optimize query performance and manage large datasets.

### ASP.NET Core API
*   Provides RESTful endpoints for managing movies data.
*   The API must be aware of the ID ranges for different partitions when inserting or updating data.
*   Dynamic connection string generation is *not* typically required, but the database context needs to properly target the correct partition for operations.
*   Data access layer (e.g., Entity Framework Core) interacts with the partitioned tables.

### PostgreSQL Database
*   Database server hosting the partitioned movie data.
*   Partitions managed using PostgreSQL's built-in partitioning features.

## Technologies Used

*   **ASP.NET Core:** Web framework for building APIs and backend services.
*   **C#:** Programming language for .NET development.
*   **Entity Framework Core (EF Core):** ORM for database interaction.
*   **PostgreSQL:** Relational database management system.
*   **Npgsql:** .NET data provider for PostgreSQL.
*   **Dependency Injection (DI):** For managing application dependencies.
*   **Middleware:** Not used for tenant resolution, but may be used for other context setup.

## Prerequisites

1.  **.NET SDK:** Download and install the .NET SDK ([https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)).
2.  **PostgreSQL:** Install PostgreSQL database server ([https://www.postgresql.org/download/](https://www.postgresql.org/download/)).
3.  **pgAdmin (Optional):** Install pgAdmin for PostgreSQL administration.
4.  **IDE:** Visual Studio or Visual Studio Code with C# extension.

## Setup and Configuration

### 1. Clone the Repository

```bash
git clone [repository-url]
cd [project-directory]
