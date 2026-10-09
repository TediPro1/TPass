# 🔐 TPass

> **Password Manager: Security Optional™**

Stop storing passwords in `notes.txt`.

Your passwords deserve a home.
TPass gives them the home responsible credentials deserve.

*Whether that home is secure remains an implementation detail.*

## About

TPass is a simple password manager built with **ASP.NET Core MVC**.

Users can create an account and store their credentials in one place, with saved credentials associated with their individual account.

## Features

* 👤 User registration and authentication
* 🔑 Store account credentials
* ✏️ Edit existing credentials
* 🗑️ Delete credentials
* 👁️ View your saved credentials
* 🔒 Credentials separated by user account
* 📄 Slightly more organized than `notes.txt`

## Built With

* ASP.NET Core MVC
* ASP.NET Core Identity
* Entity Framework Core
* Microsoft SQL Server
* Bootstrap

## Security

TPass is currently a learning/project application and **should not be trusted with real passwords or sensitive credentials**.

Proper credential encryption and additional security measures may be implemented as the project develops.

In other words:

```text
Security Optional™
Please do not put your bank password in here.
```

## Running the Project

1. Clone the repository.
2. Configure `DefaultConnection` in `appsettings.json`.
3. Apply the Entity Framework migrations.
4. Run the application.

```bash
dotnet ef database update
dotnet run
```

## License

Use it, modify it, learn from it.

Just don't blame `TPass` when `password123` turns out to be a bad password.
