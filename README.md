# Project Management API with ASP.NET Core
A simple REST API to create and list the ultimate tasks that will skyrocket your project performance.

## How to run the application
You only need to install SDK for .NET Core 3.1 to build and run the application. Download it [here](https://dotnet.microsoft.com/download).

Open a terminal in the Api folder, run the command `dotnet build`, then run the command `dotnet run`.

The application starts with ASP.NET Core's default web server, accessible at http://localhost:5000/ in your favorite Web broser.

## How to use
The API is not running live anymore, but the endpoints where:
* `GET /api/tasks` returns all the tasks in a single page.
* `POST /api/tasks` creates a task.
* `GET /api/task-overlaps` returns all the tasks that overlap for the same assignee.

Send a request from your command line with cURL to try it out:
```bash
curl --location --request GET '[your_url]/api/tasks'
```
Or [download](Docs/pm-api.postman_collection.json) and import the collection in [Postman]([https://www.getpostman.com/downloads/](https://www.getpostman.com/downloads/)).

## What this API needs
The API would be even better with:

* A `links` object in responses that navigating between objects, ie retrieve all the overlaps related to a task.
* Pagination.
* Logs.
* More integration tests.
