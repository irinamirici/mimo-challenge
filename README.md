# mimo-challenge

# Requirements
Visual Studio 2017 (developed with 15.7.4)
net core 2.1.302

# Running the Api
Checkout project
Build with Visual Studio
Set Mimo.Api as Startup Project
Start in debug with IIS Express

The project will create a sqlite db named mimoapi.db in src\Mimo.Api (It can be browsed with DB Browser for Sqlite)
Migrations are applied automatically at every startup.
2 test users are seeded in db by InitialSeed migration, along with 3 test courses and achievement types.

Add /swagger to the base url, and you will get a swagger UI.
Pick an endpoint and send a request.

Endpoints which manipulate course structure require an Authorization header for a ContentCreator user.
Use: Basic Y29udGVudGNyZWF0b3I6aGFzaGVkcHdk

CompleteLesson endpoint requires and Authorization header for a Client user.
Use: Basic bWltb3VzZXI6aGFzaGVkcHdk

GET endpoints work for both user types.

# Running the Tests
Open TestExplorer and run the tests.
IntegrationTests use a TestServer, which applies test migrations, and creates a separate test db in 
test\Mimo.Api.IntegrationTests\bin\Debug\netcoreapp2.1

# Adding a migration
Open Package Manager Console
Select src\Mimo.Persistence as Default project from the above dropdown
cd src\Mimo.Persistence
dotnet ef migrations add MyMigration -s ..\Mimo.Api

You can apply migrations with 
dotnet ef database update -s ..\Mimo.Api
but its not necessary. They will be applied when application or tests start.