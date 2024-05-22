# What is this?

A project seed for a C# dotnet API ("PaylocityBenefitsCalculator").  It is meant to get you started on the Paylocity BackEnd Coding Challenge by taking some initial setup decisions away.

The goal is to respect your time, avoid live coding, and get a sense for how you work.

# Coding Challenge

**Show us how you work.**

Each of our Paylocity product teams operates like a small startup, empowered to deliver business value in
whatever way they see fit. Because our teams are close knit and fast moving it is imperative that you are able
to work collaboratively with your fellow developers. 

This coding challenge is designed to allow you to demonstrate your abilities and discuss your approach to
design and implementation with your potential colleagues. You are free to use whatever technologies you
prefer but please be prepared to discuss the choices you’ve made. We encourage you to focus on creating a
logical and functional solution rather than one that is completely polished and ready for production.

The challenge can be used as a canvas to capture your strengths in addition to reflecting your overall coding
standards and approach. There’s no right or wrong answer.  It’s more about how you think through the
problem. We’re looking to see your skills in all three tiers so the solution can be used as a conversation piece
to show our teams your abilities across the board.

Requirements will be given separately.

# Setup Instructions

## Data Layer
This project requires PostgreSQL to be install in order to run locally as well as to run tests.

Here is a link [PostgreSQL Download](https://www.postgresql.org/download/).

Note: the project expects to use default user and password for the connection string.

* User = postgres
* Password = postgres

After installing postgres two databases need to be created

* paylocity_benefits
* paylocity_benefits.test

Both are necessary to run the app and tests respectively.

After creating the databases go into the PaylocityChallengePostgreSQL folder and run the setup.sql file for both databases. 

Example below:

``psql -U postgres -d paylocity_benefits -f setup.sql``

### Data Seeder

In case of needing some mock data you can use the seeder file.

``psql -U postgres -d paylocity_benefits -f seeder.sql``


## App Setup

After setting up the database the App should be very plug and play.

The app has 2 main launch options:

* Api
* ApiTestEnv

The Api launch option is recommended to run the app locally for manual testing. Api uses `paylocity_benefits` database.

The ApiTestEnv launch option is meant to be an enviroment use only for integration tests to run as it needs data to be controlled for the tests. ApiTestEnv uses `paylocity_benefits.test` database.


### Note:
If the local user and password credentials for the database are not the ones provided above. Make sure to update the connection strings in `IntegrationTest.cs`, `appsettings.test.json` and `appsettings.json` accordingly.

## Runnning Integration Tests

When runnning the integration tests make sure to run a local instance of the App using the `ApiTestEnv` launch option. 


# Explaining Decisions

### Data Layer

PostgreSQL was used due to being a very common Database provider, being free, having built in functionality to handle relatioship limits like Employees only being able to have one Spouse or Domestic Partner, and SQL being the most common Database structure.

### Adding a second launch option and second database

By the nature of the tests it is benefitial to have a stand alone enviroment to execute them. Specially when testing get-all endpoints to be able to control data created and clean up without affecting dev/manual testing.

### Dividing business logic in services

Having the business logic in its own layer, creates an abstraction layer from an entry point like endpoints, as well as, from the data storage/acess point like the query repo.


