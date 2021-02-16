# 📦 Stock Management API

Clean architecture template of a .NET 5 monolithic REST API using
IdentityServer, Mediatr, EF Core and FluentValidation.

> This implementation heavily relies on
> [the awesome work](https://github.com/jasontaylordev/CleanArchitecture) of
> [Jason Taylor](https://github.com/jasontaylordev), credits to him for a large
> part of this project !

## Technologies

- ASP.NET Core 5
- Swagger
- Entity Framework 5
- MediatR
- AutoMapper
- IdentityServer
- FluentValidation
- Docker (🏗 WiP)
- xUnit and xBehave (🏗 WiP)

## Getting Started

This project is exposing a simple web API with JWT authentication and roles to
manage users and very basic products.

Due to the simplicity of the examples provided out of the box, you can easily
remove the logic that you don't need to build your own on the top of the
existing mechanisms.

To start using this repository as a foundation for your API:

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
1. Navigate to `Infrastructure/` and execute the script [`tools/Install-ef.ps1`](./tools/Install-ef.ps1)
1. Create your initial migration, from `Infrastructure/` by running the script [`tools/Create-Migration.ps1`](./tools/Create-Migration.ps1) which will create your migration and apply it
1. Once you're set up, heads of to `WebApi/` and run `dotnet run` to run the back-end. You will also see the logs displayed in the console

> 📑 Note: when running the program in the dev environment, the database used
> will be in-memory by default. You are free to change it by modifying the value
> of `"UseInMemoryDatabase"` in `WebApi/appsettings.json`

## Architecture

For an overview of the architecture, or a deep dive in it, please refer to the
dedicated [ARCHITECTURE.md](./ARCHITECTURE.md)

## License and credits

This projet is under the [MIT License](./LICENSE).

- This project heavily relies on
  [the awesome work](https://github.com/jasontaylordev/CleanArchitecture)
  of [Jason Taylor](https://github.com/jasontaylordev), credits to him for a
  large part of this project
