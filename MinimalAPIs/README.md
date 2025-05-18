# README

Minimal APIs implementation of the FastTickets API.

## Running the application

To be able to run the application locally make sure that API is set up as a startup project.

Refer to the [FastTickets.http](FastTickets.http) file for some sample requests to see the API in action.

The persistence is done in SQLite, both for the application and the integration tests. But both of them use different databases. The names for the different databases are taken from the [appsettings.json](API/appsettings.json) file.

## Seed data

A different set of data is seeded for the application and the integration tests. See [SeedData](Persistence/Data/SeedData.cs) used for the application and [SeedTestData](Persistence/Data/SeedTestData.cs) used for tests.

## Tests

Both, the API and Service tests create their own instance of the test database. But bear in mind that the databases are not clened or re seeded during test runs (see comment in the `ClearData` method of [FastTicketsDBFactoryBase](Persistence/DBFactories/FastTicketsDBFactoryBase.cs)).
