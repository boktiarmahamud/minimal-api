# ASP.NET Core Minimal API CRUD

A RESTful CRUD API built using **ASP.NET Core Minimal API**, **Entity Framework Core (EF Core)**, and **Microsoft SQL Server**. This project demonstrates how to perform Create, Read, Update, and Delete (CRUD) operations using lightweight API endpoints. API endpoints are tested using **Postman**.

## 🚀 Technologies Used

- C#
- ASP.NET Core Minimal API
- Entity Framework Core (EF Core)
- Microsoft SQL Server
- Postman
- Visual Studio

## ✨ Features

- Create new lead records
- Retrieve all leads
- Retrieve a specific lead by ID
- Update existing lead records
- Delete lead records
- SQL Server database integration
- Entity Framework Core migrations
- Seed sample data
- API endpoint testing using Postman
- HTTP status code handling

## 📁 Project Structure

```text
MinimalApiCURD/
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   └── Lead.cs
├── Migrations/
├── appsettings.json
├── Program.cs
└── MinimalApiCURD.csproj
```

## ⚙️ Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/boktiarmahamud/minimal-api.git
```

Navigate to the project directory:

```bash
cd minimal-api
```

Open the project in Visual Studio or navigate to the directory containing the `.csproj` file.

### 2. Configure the Database

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=MinimalApiCRUD;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER` with your SQL Server instance name.

**Important:** Ensure that the connection string key (`DefaultConnection`) matches the key used in `Program.cs`. If your application uses a different key, such as `dbcs`, update the configuration accordingly.

### 3. Install Required Packages

If these packages are not already installed, run the following commands from the directory containing your project file:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### 4. Create and Apply Database Migrations

Open the Package Manager Console in Visual Studio:

**Tools → NuGet Package Manager → Package Manager Console**

If you have not created a migration yet, run:

```powershell
Add-Migration InitialMigration
Update-Database
```

If migrations already exist, apply them using:

```powershell
Update-Database
```

These commands create and update the database schema based on your Entity Framework Core models.

### 5. Run the Application

Run the following command from the project directory:

```bash
dotnet run
```

The API will start at the local URL displayed in the terminal.

## 🔗 API Endpoints

| HTTP Method | Endpoint | Description |
|---|---|---|
| GET | `/api/leads` | Retrieve all leads |
| GET | `/api/leads/{id}` | Retrieve a lead by ID |
| POST | `/api/leads` | Create a new lead |
| PUT | `/api/leads/{id}` | Update an existing lead |
| DELETE | `/api/leads/{id}` | Delete a lead |

**Note:** The POST, PUT, and DELETE endpoints must be implemented in `Program.cs` for these operations to work. The table lists the intended CRUD endpoints.

## 🧪 Testing with Postman

All implemented API endpoints can be tested using Postman.

1. Start the application.
2. Copy the local API URL from the terminal.
3. Open Postman and create a new HTTP request.
4. Select the appropriate HTTP method.
5. Enter the endpoint URL.
6. For POST and PUT requests, select **Body → raw → JSON** and provide the request data.
7. Click **Send** and verify the response and HTTP status code.



## 📚 Learning Objectives

- Understanding ASP.NET Core Minimal API
- Implementing RESTful CRUD operations
- Working with Entity Framework Core
- Integrating SQL Server with an ASP.NET Core application
- Managing database migrations
- Seeding initial database records
- Testing API endpoints using Postman

## 👨‍💻 Author

**Boktiar Mahamud**

GitHub: [boktiarmahamud](https://github.com/boktiarmahamud)

Project Repository: [minimal-api](https://github.com/boktiarmahamud/minimal-api)

## 📄 License

This project is intended for learning and educational purposes.
