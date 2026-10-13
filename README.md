# ASP.NET Core Minimal API CRUD with JWT Authentication

A RESTful CRUD API built using **ASP.NET Core Minimal API**, **Entity Framework Core (EF Core)**, and **Microsoft SQL Server**. This project demonstrates how to implement Create, Read, Update, and Delete (CRUD) operations, along with **JWT (JSON Web Token) authentication** to secure API endpoints.

Users can register an account, log in with their credentials, receive a JWT, and access protected lead-management endpoints using a bearer token. API endpoints are tested using Postman.

## 🚀 Technologies Used

- C#
- ASP.NET Core Minimal API
- Entity Framework Core (EF Core)
- Microsoft SQL Server
- JWT Bearer Authentication
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- ASP.NET Core Identity password hashing
- Postman
- Visual Studio
- SQL Server migrations

## ✨ Features

### Lead Management (CRUD)

- Create new lead records
- Retrieve all leads
- Retrieve a specific lead by ID
- Update existing lead records
- Delete lead records
- SQL Server database integration
- Entity Framework Core migrations
- Seed sample data
- HTTP status code handling

### JWT Authentication

- User registration
- User login using email and password
- Secure password hashing with `PasswordHasher<User>`
- JWT token generation after successful login
- JWT signature, issuer, audience, and expiration validation
- Bearer token authentication
- Authorization for protected CRUD endpoints
- Appropriate HTTP responses for authentication failures

## 📁 Project Structure

```text
MinimalApiCURD/
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   └── AuthDTOs.cs
├── Models/
│   ├── Lead.cs
│   └── User.cs
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

Update the connection string in `appsettings.json` with your SQL Server instance details.

```json
{
  "ConnectionStrings": {
    "db": "Server=YOUR_SERVER_NAME;Database=MinimalApiCURD;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

**Important:** The connection string key (`db`) must match the key used in `Program.cs`. If your application uses a different name, update the configuration accordingly.

### 3. Configure JWT Settings

Add a `JwtSettings` section to `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY",
    "Issuer": "MyApi",
    "Audience": "MyApiUsers",
    "ExpirationInMinutes": 60
  }
}
```

The settings have the following purposes:

- **SecretKey:** Used to sign and validate JWT tokens.
- **Issuer:** Identifies the system that issues the token.
- **Audience:** Identifies the intended API or token audience.
- **ExpirationInMinutes:** Specifies how long the token remains valid.

Use a long, random secret key suitable for HMAC-SHA256 signing. Do not commit production secrets to GitHub. Store sensitive configuration in .NET User Secrets or a secure environment-specific configuration provider.

Keep the database connection string and JWT settings together in your existing `appsettings.json`; the separate snippets above illustrate the relevant sections.

### 4. Install Required Packages

If the packages are not already installed, run these commands from the directory containing the project file:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.Extensions.Identity.Core
```

Use package versions compatible with your target .NET version.

### 5. Create and Apply Database Migrations

The application uses Entity Framework Core migrations to create and update database tables for leads and users.

Open the Package Manager Console in Visual Studio:

**Tools → NuGet Package Manager → Package Manager Console**

If you have not created an initial migration, run:

```powershell
Add-Migration InitialMigration
Update-Database
```

If migrations already exist and you have added the `User` model and `Users` DbSet, create a new migration:

```powershell
Add-Migration AddUserTable
Update-Database
```

If the migration already exists, apply pending migrations with:

```powershell
Update-Database
```

Use a unique migration name that matches your project's existing migration history. Ensure the project builds successfully before running migrations.

### 6. Run the Application

Run the following command from the project directory:

```bash
dotnet run
```

The API will start at the local URL displayed in the terminal.

For local development, the URL might look like:

```text
http://localhost:5110
```

Use the actual URL and port displayed by your application.

## 🔐 Authentication Workflow

The JWT authentication process follows these steps:

1. **Register:** A user submits a username, email, and password.
2. **Hash Password:** The application hashes the password before storing the user in SQL Server.
3. **Login:** The user submits their email and password.
4. **Verify Credentials:** The application finds the user and verifies the submitted password against the stored password hash.
5. **Generate Token:** If the credentials are valid, the application generates a signed JWT.
6. **Send Token:** The API returns the JWT to the client.
7. **Access Protected Endpoints:** The client sends the token in the `Authorization` header.
8. **Validate Token:** ASP.NET Core validates the token before allowing access to protected endpoints.

### Authentication Header

Protected requests must include:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

Replace `YOUR_JWT_TOKEN` with the token returned by the login endpoint.

## 🔗 API Endpoints

### Authentication Endpoints

| HTTP Method | Endpoint | Description | Authentication |
|---|---|---|---|
| POST | `/api/register` | Register a new user | Not required |
| POST | `/api/login` | Log in and receive a JWT | Not required |

### Lead CRUD Endpoints

The following endpoints are intended to require a valid JWT bearer token.

| HTTP Method | Endpoint | Description |
|---|---|---|
| GET | `/api/leads` | Retrieve all leads |
| GET | `/api/leads/{id}` | Retrieve a lead by ID |
| POST | `/api/leads` | Create a new lead |
| PUT | `/api/leads/{id}` | Update an existing lead |
| DELETE | `/api/leads/{id}` | Delete a lead |

**Note:** Each lead endpoint must be configured with `.RequireAuthorization()` or placed in a route group that requires authorization. The table describes the intended configuration; it does not automatically secure the endpoints.

## 🧪 Testing with Postman

### 1. Register a User

Create a new POST request:

```text
POST http://localhost:5110/api/register
```

Select **Body → raw → JSON** and enter:

```json
{
  "userName": "Rahim",
  "email": "rahim@example.com",
  "password": "A-Strong-Unique-Password-123!"
}
```

Expected response: `201 Created`, containing the new user's ID, username, and email.

### 2. Log In

Create another POST request:

```text
POST http://localhost:5110/api/login
```

Select **Body → raw → JSON** and enter:

```json
{
  "email": "rahim@example.com",
  "password": "A-Strong-Unique-Password-123!"
}
```

Expected response: `200 OK`, containing the JWT token and user information.

Example response:

```json
{
  "token": "YOUR_GENERATED_JWT_TOKEN",
  "userName": "Rahim",
  "email": "rahim@example.com"
}
```

Copy the actual token returned by the API.

### 3. Access a Protected Endpoint

Create a GET request:

```text
GET http://localhost:5110/api/leads
```

In Postman:

1. Open the **Authorization** tab.
2. Select **Bearer Token** as the authentication type.
3. Paste the JWT returned by the login endpoint.
4. Click **Send**.

Expected response: `200 OK`, provided the token is valid and the database request succeeds.

### 4. Test Unauthorized Access

Send the same request without a token.

Expected response: `401 Unauthorized`.

This confirms that the endpoint requires valid authentication, assuming the endpoint has been configured with `.RequireAuthorization()`.

### 5. Test CRUD Operations

For POST and PUT requests, select **Body → raw → JSON** and provide the required lead data. Include the bearer token for protected requests.

Verify the response status codes and confirm that the database reflects the changes.

## 📊 HTTP Status Codes

| Status Code | Meaning |
|---|---|
| `200 OK` | Request completed successfully |
| `201 Created` | A new resource was created |
| `400 Bad Request` | Invalid or missing request data |
| `401 Unauthorized` | Authentication is missing or invalid |
| `403 Forbidden` | Authenticated caller lacks permission |
| `404 Not Found` | Requested resource does not exist |
| `409 Conflict` | A conflict occurred, such as an already registered email |

The exact status code depends on the validation and error-handling logic implemented in each endpoint.

## 🔒 Security Considerations

- Never store plain-text passwords in the database.
- Use `PasswordHasher<User>` consistently for registration and login verification.
- Keep the JWT secret key private and out of source control.
- Use HTTPS outside local development.
- Validate the JWT signature, issuer, audience, and expiration.
- Require authorization on all endpoints that should be protected.
- Do not include passwords or password hashes in JWT claims or API responses.
- Remember that JWT authentication establishes identity; role-based or policy-based authorization is a separate concern.

## 📚 Learning Objectives

- Understanding ASP.NET Core Minimal API
- Implementing RESTful CRUD operations
- Integrating SQL Server with Entity Framework Core
- Managing database migrations
- Working with entity models and DTOs
- Implementing user registration and login
- Hashing and verifying passwords
- Generating and validating JWT tokens
- Securing endpoints using bearer authentication
- Testing authentication and CRUD operations with Postman
- Understanding HTTP status codes and API security basics

## 👨‍💻 Author

**Boktiar Mahamud**

GitHub: [boktiarmahamud](https://github.com/boktiarmahamud)

Project Repository: [minimal-api](https://github.com/boktiarmahamud/minimal-api)

## 📄 License

This project is intended for learning and educational purposes.
