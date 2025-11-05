# Insurance Partner Management System

A modern ASP.NET Core MVC web application for managing insurance company partners and their policies.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-11.0-239120?style=flat-square&logo=c-sharp)
![Bootstrap](https://img.shields.io/badge/Bootstrap-4.6-7952B3?style=flat-square&logo=bootstrap)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat-square&logo=docker)

---

## 📋 Overview

This application provides a comprehensive solution for managing insurance partners and their policies with:
- Partner management (create, view, list)
- Policy management (add, view)
- Real-time highlighting for partners with 5+ policies or 5000+ total amount
- Responsive UI with Bootstrap 4
- Full validation (client and server-side)

---

## 🛠 Technology Stack

- **Framework:** ASP.NET Core MVC 9.0
- **Language:** C# 11.0
- **Data Access:** Dapper (Micro ORM)
- **Database:** SQL Server 2022
- **UI:** Bootstrap 4, jQuery
- **Migrations:** Entity Framework Core
- **Containerization:** Docker & Docker Compose

---

## 🚀 Getting Started

### Prerequisites

**Option 1: Docker (Recommended)**
- Docker Desktop

**Option 2: Local Development**
- .NET 9.0 SDK
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 or VS Code

### Installation

#### 🐳 Using Docker (Recommended)

1. **Clone the repository**
   ```bash
   git clone https://github.com/dkrizanic/insurance-app.git
   cd insurance-app
   ```

2. **Start the application with Docker**
   ```bash
   docker-compose up -d --build
   ```
   
   This will:
   - Start SQL Server 2022 in a container
   - Build and start the web application
   - Apply database migrations automatically
   - Make the app available at http://localhost:5000

3. **View logs**
   ```bash
   # View application logs
   docker-compose logs webapp -f
   
   # View all logs
   docker-compose logs -f
   ```

4. **Stop the application**
   ```bash
   docker-compose down
   ```

5. **Restart after code changes**
   ```bash
   docker-compose up -d --build
   ```

#### 💻 Local Development (Without Docker)

1. **Clone the repository**
   ```bash
   git clone https://github.com/dkrizanic/insurance-app.git
   cd insurance-app
   ```

2. **Update connection string**
   
   Edit `src/appsettings.json` to match your SQL Server:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InsuranceApp;Trusted_Connection=True"
     }
   }
   ```

3. **Run the application**
   ```bash
   cd src
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

4. **Open browser**
   
   Navigate to: `https://localhost:5001`

---

## � Docker Commands

### Basic Operations
```bash
# Start containers in background
docker-compose up -d

# Start containers and rebuild images
docker-compose up -d --build

# Stop containers (keeps data)
docker-compose down

# Stop containers and remove volumes (deletes data)
docker-compose down -v

# View running containers
docker-compose ps
```

### Logs & Debugging
```bash
# View all logs
docker-compose logs -f

# View only webapp logs
docker-compose logs webapp -f

# View only database logs
docker-compose logs sqlserver -f

# View last 50 lines
docker-compose logs webapp --tail=50
```

### Database Management
```bash
# Connect to SQL Server container
docker exec -it insurance-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -C

# Manually initialize database schema (if needed)
docker exec -i insurance-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -d master -i /docker-entrypoint-initdb.d/InitialSchema.sql

# Backup database volume
docker run --rm -v insurance-app_sqlserver_data:/data -v $(pwd):/backup alpine tar czf /backup/db-backup.tar.gz /data

# Restore database volume
docker run --rm -v insurance-app_sqlserver_data:/data -v $(pwd):/backup alpine tar xzf /backup/db-backup.tar.gz -C /
```

### Troubleshooting
```bash
# Restart a specific service
docker-compose restart webapp

# View resource usage
docker stats

# Clean up unused Docker resources
docker system prune -a
```

---

## �📊 Business Rules

### Partner Management
- Partner Number: exactly 20 digits
- External Code: unique, 10-20 characters
- Croatian PIN: optional, 11 digits
- All fields validated

### Policy Management
- Policy Number: unique, 10-15 characters
- Amount: must be greater than 0

### Highlighting
Partners are highlighted with `*` if:
- More than 5 policies, OR
- Total policy amount exceeds 5000

---

## 📁 Project Structure

```
src/
├── Controllers/       # HTTP handlers
├── Services/          # Business logic
├── Data/
│   ├── Repositories/  # Data access (Dapper)
│   └── Migrations/    # Database schema
├── Models/            # Domain models
├── Views/             # Razor views
└── wwwroot/           # Static files
```

---

## � Author

**Dario Krizanic**
- GitHub: [@dkrizanic](https://github.com/dkrizanic)

---
