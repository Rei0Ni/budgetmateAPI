# BudgetMate API

**BudgetMate** is a personal expense tracker API built with C# and .NET Core 8. This API enables users to manage their personal finances by tracking income, expenses, It provides features for categorizing expenses, viewing transaction history, and generating financial summaries to help users stay on top of their financial health.

## Features

- **User Authentication**: Secure user registration and login using ASP.NET Identity and session cookies.
- **Expense Tracking**: Add, update, and delete expense records with support for categorization.
- **Income Management**: Record income entries and associate them with categories.
- **Transaction History**: View a detailed transaction history for income and expenses.
- **Financial Summary**: Get an overview of your financial situation, including current balance and spending summaries by category.

## Technologies Used

- **.NET Core 8**: Backend framework.
- **MongoDB**: NoSQL database for storing transaction, user, and budget data.
- **ASP.NET Identity**: Authentication and session management.
- **AutoMapper**: For mapping between domain entities and DTOs.
- **Dependency Injection**: For service management.
- **Swagger**: API documentation and testing.

## Getting Started

### Prerequisites

- [.NET Core 8 SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/try/download/community) (local or cloud)
- A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. Clone the repository:
```bash
   git clone https://github.com/Rei0Ni/budgetmateAPI.git
```
2. Navigate to the project directory:
```bash 
cd budgetmateAPI/BudgetMate.API
```
3. Restore the project dependencies:
```bash
dotnet restore
```
4. set up the `appsettings.json` file with your database connection
```json
{
   "Logging":{
      "LogLevel":{
         "Default":"Information",
         "Microsoft.AspNetCore":"Warning"
      }
   },
   "AllowedHosts":"*",
   "MongoDBConfig":{
      "ConnectionURI":"mongodb://localhost:27017",
      "DatabaseName":"BudgetMate"
   }
}
```
5. Run the application:
```bash
dotnet run
```
6. Once the API is running, you can access the Swagger UI for testing and viewing available endpoints:
```
http://localhost:5001/swagger
```