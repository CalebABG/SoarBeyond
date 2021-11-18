# Using Identity Scaffolding

# Resources
- [There was an error running the selected code generator in .Net Core 5](https://clintmcmahon.com/there-was-an-error-running-the-selected-code-generator-in-net-core-5/)
- [Scaffold Identity in ASP.NET Core projects](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-5.0&tabs=netcore-cli#scaffold-identity-into-a-blazor-server-project-without-existing-authorization)
- [dotnet-aspnet-codegenerator](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-5.0)

## Todo: Add doc for this
- Run the below command in the `SoarBeyond.Web` folder
```
dotnet aspnet-codegenerator identity --files "Account.Register;Account.Login;Account.Logout"
```
- If need to overwrite existing files, add this flag at the end of the above command:
  - `--force`

- If issues, drop Nuget package version for `Microsoft.VisualStudio.Web.CodeGeneration.Design` to `3.1.0`