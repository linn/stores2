# Stores2
Form and reporting application for data relating to Stores, Logistics etc. A more modern extension of [stores](https://github.com/linn/stores) - code from that repo could be migrated to this one over time.

## Solution summary
* Service.Host project provides forms and reporting for existing data and code in the LinnApps Oracle database.
* Messaging.Host project is a Hosted service for consuming messages (using Rabbit MQ).
* Scheduling.Host project provides utilities for scheduling tasks.

## Component technologies
* The backend services are dotnet core C# apps with minimal third party dependencies.
* Service.Host uses the .NET minimal API web framework
* The javascript client app is built with React and managed with npm and webpack.
* Javacript tests are run with Jest. React components are testing using [React Testing Library](https://testing-library.com/docs/react-testing-library/intro/)
* Local debugging of the client should be performed using node v20 for best results.
* Persistence is to an Oracle database via EF Core.
* Continuous deployment via Docker container to AWS ECS using Travis CI.
* Messaging.Host uses the RabbitMQ C# client to interact with Rabbit Messages Queues 
* Scheduling.Host runs .NET Core Hosted Services/Background Tasks

## Local running and Testing
### C# service
* Restore nuget packages.
* Run C# tests as preferred.
* run or debug the Service.Host project to start the backend.

### Client
* `npm i` to install npm packages.
* `npm start` to run client locally on port 3000.
* `npm test` to run javascript tests.

### Routes
With the current configuration, all requests to 
* app.linn.co.uk/stores2* 
* app.linn.co.uk/requisitions*

will be sent to this app via traefik
