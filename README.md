# WalletApp Backend API

A robust .NET Core Web API backend for a digital wallet application, providing secure account management and financial transaction services with SQL Server integration.

## 🏗️ Architecture Overview

The backend follows a clean architecture pattern with clear separation of concerns:

- **WalletApp.WebAPI** - ASP.NET Core Web API layer handling HTTP requests and business logic
- **WalletApp.DataLayer** - Entity Framework Core data access layer with SQL Server integration

## 🚀 Features

- **System Authentication** - Admin login validation for system access
- **Account Management** - Create new wallet accounts with customizable initial balances
- **Account Listing** - Retrieve all accounts with comprehensive owner information
- **Balance Inquiry** - Get real-time account balance with transaction history
- **Money Transfer** - Secure transfers between accounts with comprehensive validation
- **Transaction Integrity** - Automatic balance updates and transaction logging
- **Data Validation** - Input validation and business rule enforcement

## 🛠️ Technology Stack

- **.NET 8.0** - Modern C# framework with latest language features
- **ASP.NET Core Web API** - RESTful API development framework
- **Entity Framework Core 9.0** - Object-Relational Mapping with SQL Server
- **SQL Server** - Robust database management system
- **Swagger/OpenAPI** - Comprehensive API documentation and testing interface

## 📋 Prerequisites

- **.NET 8.0 SDK** or later
- **SQL Server** (Express, Developer, or full version)
- **Visual Studio 2022** or **VS Code** (recommended IDEs)

## ⚙️ Installation & Setup

### 1. Database Configuration

Update the connection string in `WalletApp.WebAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=WALLET;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

Also update the hardcoded connection string in `WalletApp.DataLayer/Models/WalletDbContext.cs`:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer("Server=YOUR_SERVER_NAME;Database=WALLET;Trusted_Connection=true;TrustServerCertificate=true;");
```

### 2. Database Setup

Create the WALLET database and ensure you have the following tables with admin data:

```sql
-- Sample admin user for system login
INSERT INTO Admins (AdminName, AdminTaz, AdminPassword) 
VALUES ('System Admin', 'admin123', 'password123');
```

### 3. Project Setup

**Clone and restore packages:**
```bash
git clone <repository-url>
cd WalletApp
dotnet restore
```

**Build the solution:**
```bash
dotnet build
```

**Run the API:**
```bash
cd WalletApp.WebAPI
dotnet run
```

### 4. Access Points

The API will be available at:
- **HTTP:** `http://localhost:5178`
- **HTTPS:** `https://localhost:7298`
- **Swagger UI:** `https://localhost:7298/swagger`

## 🔗 API Endpoints

| Method | Endpoint | Description | Parameters |
|--------|----------|-------------|------------|
| GET | `/api/SystemLogin` | Admin authentication | `adminTaz`, `adminPassword` |
| POST | `/api/CreateAccount` | Create new wallet account | Request body (JSON) |
| GET | `/api/GetAccounts` | Retrieve all accounts | None |
| GET | `/api/GetBalanceAccount` | Get account balance | `accountId` |
| POST | `/api/CreateTransaction` | Transfer money between accounts | Request body (JSON) |

### API Request/Response Examples

#### System Login
```http
GET /api/SystemLogin?adminTaz=admin123&adminPassword=password123
```

**Response:**
```json
true
```

#### Create Account
```http
POST /api/CreateAccount
Content-Type: application/json

{
  "ownerName": "John Doe",
  "ownerTaz": "123456789",
  "ownerPassword": "securePassword",
  "initialBalance": 1000.00
}
```

**Response:**
```json
true
```

#### Get All Accounts
```http
GET /api/GetAccounts
```

**Response:**
```json
[
  {
    "ownerName": "John Doe",
    "ownerTaz": "123456789",
    "accountID": 1,
    "accountName": "John DoeOsh"
  }
]
```

#### Get Account Balance
```http
GET /api/GetBalanceAccount?accountId=1
```

**Response:**
```json
{
  "balanceValue": 1000.00,
  "balanceTime": "2025-06-25T10:30:00Z"
}
```

#### Create Transaction
```http
POST /api/CreateTransaction
Content-Type: application/json

{
  "accountPayID": 1,
  "accountGetID": 2,
  "amount": 150.00
}
```

**Response:**
```json
true
```

## 📊 Database Schema

### Core Entities

**Admins**
- `AdminID` (Primary Key)
- `AdminName`, `AdminTaz`, `AdminPassword`

**Owners**
- `OwnerID` (Primary Key)
- `OwnerName`, `OwnerTaz`, `OwnerPassword`

**Accounts**
- `AccountID` (Primary Key)
- `OwnerID` (Foreign Key)
- `AccountName` (Auto-generated as OwnerName + "Osh")

**Balances**
- `BalanceID` (Primary Key)
- `AccountID` (Foreign Key)
- `BalanceValue`, `BalanceTime`, `TransactionID`

**Transactions**
- `TransactionID` (Primary Key)
- `AccountPayID`, `AccountGetID` (Foreign Keys)
- `TransactionAmount`, `TransactionTime`

### Entity Relationships
- One Owner → Many Accounts
- One Account → Many Balances
- One Account → Many Transactions (as payer or receiver)
- One Transaction → Two Balance records (for both accounts)

## 🔒 Security & Validation Features

### Business Logic Validation
- **Account Creation:** Prevents negative initial balances
- **Transactions:** Validates sufficient funds before transfer
- **Account Validation:** Prevents transfers to the same account
- **Data Integrity:** Ensures both accounts exist before transaction

### Security Measures
- Admin authentication required for system access
- Input validation on all endpoints
- Exception handling with appropriate error responses
- CORS configuration for cross-origin requests

## 🚦 Development

### Running in Development Mode
```bash
cd WalletApp.WebAPI
dotnet watch run
```

This enables hot reload for development changes.

### Building for Production
```bash
dotnet publish -c Release -o ./publish
```

### Testing with Swagger
Navigate to `https://localhost:7298/swagger` to test all endpoints interactively.

## 📁 Project Structure

```
WalletApp/
├── WalletApp.WebAPI/                 # Web API Layer
│   ├── Controllers/
│   │   ├── SystemLoginController.cs       # Admin authentication
│   │   ├── CreateAccountController.cs     # Account creation
│   │   ├── GetAccountsController.cs       # Account listing
│   │   ├── GetBalanceAccountController.cs # Balance inquiry
│   │   └── CreateTransactionController.cs # Money transfers
│   ├── Program.cs                    # Application configuration
│   ├── appsettings.json             # Configuration settings
│   └── WalletApp.WebAPI.csproj      # Project file
├── WalletApp.DataLayer/             # Data Access Layer
│   ├── Models/
│   │   ├── WalletDbContext.cs       # EF Core DbContext
│   │   ├── Admin.cs                 # Admin entity
│   │   ├── Owner.cs                 # Owner entity
│   │   ├── Account.cs               # Account entity
│   │   ├── Balance.cs               # Balance entity
│   │   └── Transaction.cs           # Transaction entity
│   └── WalletApp.DataLayer.csproj   # Project file
└── WalletApp.sln                    # Solution file
```

## 🔧 Configuration

### CORS Policy
The API includes CORS configuration for frontend integration:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

### Entity Framework Configuration
- SQL Server integration with connection pooling
- Automatic model configuration through conventions
- Foreign key relationships properly mapped

## 🐛 Troubleshooting

### Common Issues

**Database Connection Errors:**
```
Solution: 
- Verify SQL Server is running
- Check connection string format
- Ensure WALLET database exists
- Verify Windows Authentication or SQL credentials
```

**Port Already in Use:**
```
Solution:
- Modify ports in Properties/launchSettings.json
- Default ports: 5178 (HTTP), 7298 (HTTPS)
```

**Entity Framework Issues:**
```
Solution:
- Ensure connection string is correct in both appsettings.json and WalletDbContext.cs
- Verify database schema matches entity models
- Check for proper NuGet package references
```

## 📝 License

This project is licensed under the MIT License.

---

**Built with .NET 8.0 and Entity Framework Core**
