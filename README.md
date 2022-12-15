![](res/banner.png?raw=true "SoarBeyond Logo")

# SoarBeyond
Keeping a lot in your head, can be a lot! Soar beyond all of the stuff that drags you down. This is a work-in-progress Journaling web app with the goal of being a place to record and jot how you are feeling, how your day is or went, or whatever may be on your mind. Sometimes, just having a place to write can be helpful, and SoarBeyond aims to be that tool for you.

---

## Project Setup

### **Resources**
- [PostgreSQL](https://www.postgresql.org/)
- [App Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)


---

### **Preview**
#### **Would definitely recommend installing [Visual Studio 2022 Preview!](https://visualstudio.microsoft.com/vs/preview/)**

---


### **Database**
SoarBeyond uses [PostgreSQL](https://www.postgresql.org/) for its backend database store. So if you don't have PostgreSQL installed, make sure to install it before trying to run the solution.


---


### **Configuration / Secrets Management**
- To manage storage of the `Database Connection String` and other important configuration/settings, the project makes use of an `appsettings.json` file.

#### **Default**
- When you clone the repo, before running the project, you'll need to edit the template `appsettings.json` file in the following directory: `src\SoarBeyond.Web`

1. For the **Database**, look for the `Persistence` section of the file, it will look something like this:
   - ```json 
      "Persistence": {
         "Host": "localhost",
         "Port": "5432",
         "Database": "soarbeyond",
         "Username": "postgres",
         "Password": ""
      }
     ```
   - From there, just give the `Password` key a value (password for the Postgres user). You can of course change the `Username` key to be a different user in your PostgreSQL logins (just be sure to update the `Password` value to the changed login user's password).

#### **App Secrets**
- The alternative, and more recommended way (but still **NOT** suitable for `Production`) for managing the configuration file is using `User Secrets`

- The advantage of using User Secrets, is that the file is stored in a separate location from the project tree. Because of this, those secrets aren't checked into source control.

- The location of the `secrets.json` file is different between Operating Systems, but it's stored in a system-protected user profile folder on your computer.
   - **Windows** 
     - `%APPDATA%\Microsoft\UserSecrets\CalebABG-SoarBeyond\secrets.json`
   - **Mac / Linux** 
     - `~/.microsoft/usersecrets/CalebABG-SoarBeyond/secrets.json`

1. In the `src\SoarBeyond.Web` project, a `UserSecretsId` property is defined in the `src\SoarBeyond.Web.csproj` file which looks like this:
   - ```xml
     <UserSecretsId>CalebABG-SoarBeyond</UserSecretsId>
     ```

2. The structure for the file is the same for the [Default](#default) - `appsettings.json` approach
   - Just copy what's in that template and paste it in the `secrets.json` file. Then fill in the needed pieces and you're good to go!

---

### Running the Project

#### **Docker**
- If you have `Docker` and `docker-compose` installed, then you can get up and running in **1** step!
- Run the following command in the root folder
   - ```
     docker-compose -f "docker-compose.yml" up
     ```
- To stop the docker-compose containers, run this command:
   - ```
     docker-compose -f "docker-compose.yml" down
     ``` 


#### **Getting Started**
- If you're starting fresh, just cloned the repo, then you'll need to make sure you've done the needed [Configuration](#configuration--secrets-management), before running or debugging the solution.

- If you're already using SoarBeyond, you may need to do any or all of the following to ensure that you have everything you need to run the latest and greatest!

#### **Update your Database**
1. If you have `dotnet ef` command line tools installed, you can run the following command in the root folder (`SoarBeyond`) to apply the latest migration.
   - ```
     dotnet ef database update --project src\SoarBeyond.Data --startup-project src\SoarBeyond.Web --verbose
     ``` 
   - You can install the `dotnet ef` command line tool by following this document [Entity Framework Core Tools CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

2. If you're using Visual Studio, you can use the `Package Manager Console`. **Note**: Make sure in the tool window, to change the `Default project` to `src\SoarBeyond.Data` , then execute the following command:
   - ```
     Update-Database
     ``` 
   - You can install the `Package Manager Console` tools by following this document [Entity Framework Core Tools - Package Manager Console in Visual Studio](https://docs.microsoft.com/en-us/ef/core/cli/powershell)

---

## Issues? Features?? More???

Create an `issue` for the repo if you encounter any errors or strange behavior/functionality with the application. 

---

## Closing Comments
Thank you for checking out this project. I hope it can provide even the smallest bit of help, in anyway for you! Please feel free to submit feature requests or suggestions for improvement!