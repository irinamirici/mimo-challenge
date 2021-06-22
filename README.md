# mimo-challenge

# Requirements
Visual Studio 2019 (developed with 15.7.4)

net core 2.1.302

# Running the Api
Checkout project

Build with Visual Studio

Set Mimo.Api as Startup Project

Start in debug with IIS Express

The project will create a sqlite db named mimoapi.db in src\Mimo.Api (It can be browsed with DB Browser for Sqlite)
Migrations are applied automatically at every startup.
2 test users are seeded in db by InitialSeed migration, along with 3 test courses and achievement types.

Open https://localhost:44306/swagger and accept self signed certificate.
Pick an endpoint and send a request.

Endpoints which manipulate course structure require an Authorization header for a ContentCreator user.
Use: **Basic Y29udGVudGNyZWF0b3I6aGFzaGVkcHdk**

CompleteLesson endpoint requires and Authorization header for a Client user.
Use: **Basic bWltb3VzZXI6aGFzaGVkcHdk**

GET endpoints work for both user types.

Optionally application can be started with 
```
cd src/Mimo.Api/

dotnet run
```
It will start on https://localhost:5001. Open https://localhost:5001/swagger in browser and accept self signed certificate

# Running the Tests
Open TestExplorer and run the tests.

IntegrationTests use a TestServer to run the API and apply test migrations. A separate test db is created in 
test\Mimo.Api.IntegrationTests\bin\Debug\netcoreapp2.1

Optionally integration tests can be started with 
```
cd test\Mimo.Api.IntegrationTests

dotnet test
```
or unit tests with
```
cd test\Mimo.Api.UnitTests

dotnet test
```
# Adding a migration
Open Package Manager Console

Select src\Mimo.Persistence as Default project from the above dropdown
```
cd src\Mimo.Persistence

dotnet ef migrations add MyMigration -s ..\Mimo.Api
```
You can apply migrations with 
```
dotnet ef database update -s ..\Mimo.Api
```
but it is not necessary. They will be applied when application or tests start.
