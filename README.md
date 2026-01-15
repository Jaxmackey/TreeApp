
# TreeApp — REST API for Independent Trees with Exception Journal

A clean, layered ASP.NET Core 8 backend application implementing:
- Independent tree structures with validation
- Global exception handling with secure journaling
- Full compliance with the provided technical specification

Built using **Domain-Driven Design (DDD)**, **CQRS via MediatR**, and **Entity Framework Core** with PostgreSQL.

---

## ✅ Technical Requirements Compliance

| Requirement | Status |
|------------|--------|
| **Database**: PostgreSQL (code-first) | ✅ |
| **Independent trees**: each node belongs to one tree | ✅ |
| **Node name uniqueness among siblings** | ✅ |
| **Custom `SecureException`** | ✅ |
| **Exception journal** (Event ID, timestamp, params, stack trace) | ✅ |
| **Error response format** per spec | ✅ |
| **Exact API routes** from Swagger | ✅ |
| **Authentication (optional)** | ✅ |

---

## 🏗️ Architecture

- **Domain Layer** (`TreeApp.Domain`)  
  Entities, exceptions (`SecureException`), repository interfaces.
- **Application Layer** (`TreeApp.Application`)  
  DTOs, MediatR requests/handlers, business logic.
- **Infrastructure Layer** (`TreeApp.Infrastructure`)  
  EF Core DbContext, repository implementations, PostgreSQL.
- **API Layer** (`TreeApp.Api`)  
  Controllers, middleware, dependency injection.

---

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker & Docker Compose](https://www.docker.com/products/docker-desktop)

---

## ▶️ How to Run

### 1. Start PostgreSQL
```bash
docker-compose up -d

2. Apply Migrations
dotnet ef database update `
  --project src/TreeApp.Infrastructure/TreeApp.Infrastructure.csproj `
  --startup-project src/TreeApp.Api/TreeApp.Api.csproj
  
3. Run the API
dotnet run --project src/TreeApp.Api

Tree Management
POST /api.user.tree.get?treeName=MyTree
POST /api.user.tree.node.create?treeName=MyTree&nodeName=Root
POST /api.user.tree.node.create?treeName=MyTree&parentNodeId=1&nodeName=Child
POST /api.user.tree.node.rename?nodeId=2&newNodeName=RenamedChild
POST /api.user.tree.node.delete?nodeId=1

Exception Journal
# Trigger a SecureException (e.g., duplicate name)
POST /api.user.tree.node.create?treeName=MyTree&nodeName=Root

# Get event by ID (use "id" from error response)
POST /api.user.journal.getSingle?id=639040954984657252

# Get paginated logs
POST /api.user.journal.getRange?skip=0&take=10
Content-Type: application/json
{
  "from": "2026-01-15T20:00:00Z",
  "to": "2026-01-15T21:00:00Z",
  "search": "unique"
}