# Entity Framework Core .NET Command-line

## Useful Links

- [Using EF Core in a Separate Class Library](https://dotnetthoughts.net/using-ef-core-in-a-separate-class-library/)

---

## Adding and Applying a Migration

### Updating dotnet-ef tooling
1. If you have `dotnet-ef` tooling installed, you should make sure you're updated to the latest tool version.
2. In terminal execute following command:
   1. ```
       dotnet tool update -g dotnet-ef
      ```
3. Otherwise, you can install the `dotnet ef` command line tool by following this document [Entity Framework Core Tools CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

### Adding a Migration

1. cd into `SoarBeyond` root folder
2. In terminal execute following command:
    - Note: replace `<migration-name>` with a `name` for the migration

    1. ```
       dotnet ef migrations add <migration-name> --project src\SoarBeyond.Data --startup-project src\SoarBeyond.Web --verbose
       ```

    - Example:
        ```
        dotnet ef migrations add "InitialMigration" --project src\SoarBeyond.Data --startup-project src\SoarBeyond.Web --verbose
        ```
3. If all has gone well, your migration should have been added in the "Migrations" folder in `SoarBeyond.Data`

### Updating the Database / Applying a Migration

1. cd into `SoarBeyond` root folder
2. In terminal execute following command:
    1. ```
       dotnet ef database update --project src\SoarBeyond.Data --startup-project src\SoarBeyond.Web --verbose
       ```
3. If all has gone well, your most recent migration should have been applied to your database

---