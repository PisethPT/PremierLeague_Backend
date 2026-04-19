# Premier League Backend - Workspace Instructions

## Project Overview

This is an ASP.NET Core 9.0 web application for managing Premier League football data. It uses a traditional MVC architecture with ADO.NET (raw SQL via stored procedures) for data access.

## Architecture

### Technology Stack
- **Framework**: ASP.NET Core 9.0
- **Language**: C# with nullable reference types enabled
- **Database**: SQL Server (using ADO.NET with stored procedures)
- **Authentication**: Cookie-based authentication
- **Frontend**: Razor Views with Tailwind CSS

### Project Structure

```
/
├── Controllers/          # MVC Controllers (route prefix: /en/)
├── Data/
│   ├── AppDbContext.cs   # Singleton database context (ADO.NET)
│   └── Repositories/     # Repository pattern implementation
│       ├── Interfaces/
│       └── Implementations/
├── Models/
│   ├── DTOs/            # Data Transfer Objects
│   ├── Entities/        # Domain entities
│   ├── Enums/           # Enumerations
│   ├── SelectListItems/ # Dropdown/select list models
│   └── ViewModels/      # MVC ViewModels
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Helper/
│   ├── SqlCommands/     # SQL stored procedure names
│   └── Validation/      # Custom validation logic
├── Middleware/          # Custom middleware
├── Startup/
│   └── DependenciesConfig.cs  # DI registration
└── wwwroot/
    ├── css/
    ├── js/
    └── json/            # Static JSON data (e.g., nationality flags)
```

## Development Guidelines

### Repository Pattern
- All database access goes through repositories in `Data/Repositories/`
- Repositories use `IExecute` service for running SQL commands
- SQL stored procedure names are defined in `Helper/SqlCommands/`
- Example: `PlayerCommands.AddPlayerCommand` → "PL_AddPlayer"

### Adding a New Feature

1. **Define the DTO** in `Models/DTOs/`
2. **Create the Repository Interface** in `Data/Repositories/Interfaces/`
3. **Implement the Repository** in `Data/Repositories/Implementations/`
4. **Add SQL Command constants** in `Helper/SqlCommands/`
5. **Register in DI** in `Startup/DependenciesConfig.cs`
6. **Create the Controller** in `Controllers/` with `[Route("en/entity")]` attribute

### Controller Conventions
- Use `[Route("en/{entity}")]` prefix for all controllers
- Inject repositories via constructor
- Return `ActionResult` from action methods
- Use async/await for all database operations

### Database Access Pattern

```csharp
// Repository example
public async Task<bool> AddPlayerAsync(PlayerDto dto)
{
    var cmd = new SqlCommand { CommandText = PlayerCommands.AddPlayerCommand };
    cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
    // ... more parameters
    return await execute.ExecuteScalarAsync<bool>(cmd);
}
```

### Authentication
- Cookie-based authentication with 5-minute sliding expiration
- Login path: `/en/auth/login`
- Access denied path: `/en/auth/access-denied`
- Use `[Authorize]` attribute on protected controllers/actions

## Build & Run

```bash
# Build the project
dotnet build

# Run the application
dotnet run

# The application will be available at:
# - HTTP: http://localhost:5000
# - HTTPS: https://localhost:5001
```

## Key Files

| File | Purpose |
|------|---------|
| `Program.cs` | Application entry point, service configuration |
| `Startup/DependenciesConfig.cs` | Dependency injection registration |
| `Data/AppDbContext.cs` | Singleton database connection manager |
| `appsettings.json` | Connection strings and configuration |
| `Helper/SqlCommands/*.cs` | Stored procedure name constants |

## Common Tasks

### Adding a New Repository
1. Create interface in `Data/Repositories/Interfaces/I{Entity}Repository.cs`
2. Create implementation in `Data/Repositories/Implementations/{Entity}Repository.cs`
3. Register in `Startup/DependenciesConfig.cs`: `builder.Services.AddScoped<I{Entity}Repository, {Entity}Repository>();`

### Adding a New Controller
1. Create controller class inheriting from `Controller`
2. Add `[Route("en/{plural-entity}")]` attribute
3. Inject required repositories via constructor
4. Implement CRUD actions with async signatures

### Adding SQL Commands
1. Add stored procedure name constant in `Helper/SqlCommands/{Entity}Commands.cs`
2. Follow naming convention: `PL_{Action}{Entity}` (e.g., `PL_AddPlayer`)

## Notes

- This project uses **raw ADO.NET with stored procedures**, not Entity Framework
- The `AppDbContext` is a singleton managing `SqlConnection` instances
- File uploads are stored in `wwwroot/upload/`
- Nationality flags are loaded from `wwwroot/json/nationality.json`
