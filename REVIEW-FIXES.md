# Regression checks

Credential passwords are stored as plain text, as requested. ASP.NET Core Identity continues to handle account login passwords normally.

Run the regression checks from the solution directory:

    dotnet run --project ../TPass.Checks/TPass.Checks.csproj

The checks use a disposable in-memory database. They do not access the application's database. They cover owner isolation, soft deletion, Create/Edit behavior, plaintext credential storage, invalid models, and inherited Identity properties. Razor views are checked by the application build.
