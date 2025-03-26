Book Finder API - .NET Backend
A RESTful API for the Book Finder application, built with ASP.NET Core 8. The API features JWT Authentication, complete CRUD operations for book management, and a favorites system. The technology stack includes ASP.NET Core 8, Entity Framework Core with repository pattern, MS SQL Server for database, JWT Authentication, and Swagger/OpenAPI documentation.

Installation & Setup

Clone the repository

Update the MS SQL connection string in appsettings.json

Run database migrations: dotnet ef database update

Launch the application: dotnet run
The API will be available at http://localhost:7259 with Swagger documentation accessible at /swagger endpoint during development.

Project Structure
The solution follows clean architecture principles with:

Controllers: API endpoints
Data: DbContext and database configurations
DTOs: Data transfer objects
Interfaces: Repository contracts
Models: Entity classes
Repositories: Data access layer (Repository pattern implementation)
Services: Business logic layer
