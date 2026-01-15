# 🌲 TreeApp — REST API for Independent Trees with Exception Journal

A clean, layered ASP.NET Core 8 backend application that implements **independent tree structures** and a **secure exception journal**, fully compliant with the provided technical specification.

Built using **Domain-Driven Design (DDD)**, **CQRS via MediatR**, **Entity Framework Core**, and **PostgreSQL**.

---

## ✅ Technical Specification Compliance

| Requirement | Implemented | Details |
|------------|:-----------:|--------|
| **Database**: PostgreSQL (code-first) | ✅ | EF Core + migrations |
| **Independent trees** | ✅ | Each node has `TreeName`; all descendants inherit it |
| **Mandatory `name` field** | ✅ | Non-nullable `string Name` |
| **Node name uniqueness among siblings** | ✅ | Enforced in business logic |
| **Delete node → delete entire subtree** | ✅ | `ON DELETE CASCADE` in DB + validation |
| **Custom `SecureException`** | ✅ | Defined in `Domain`, handled globally |
| **Exception journal** | ✅ | Logs `EventId`, `Timestamp`, query/body params, stack trace |
| **Error response format** | ✅ | Matches spec exactly:<br>`{"type":"Secure","id":"...","data":{"message":"..."}}` |
| **Exact API routes from Swagger** | ✅ | All endpoints use `POST` as specified |
| **Authentication (optional)** | ✅ | `/api.user.partner.rememberMe` returns dummy JWT |

> 🔒 **Note**: Authentication is optional per spec. No endpoints are protected with `[Authorize]`, which is **explicitly allowed**.

---

## 🏗️ Architecture Overview

Follows strict **clean architecture** principles with full separation of concerns:


### Layer Responsibilities

- **Domain**: Pure business logic, no dependencies.
- **Application**: Use cases, DTOs, MediatR pipeline.
- **Infrastructure**: Persistence, external services.
- **API**: HTTP concerns, DI setup, error handling.

---

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker & Docker Compose](https://www.docker.com/products/docker-desktop)

---

## ▶️ How to Run

### 1. Start PostgreSQL
```bash
docker-compose up -d

dotnet ef database update `
  --project src/TreeApp.Infrastructure/TreeApp.Infrastructure.csproj `
  --startup-project src/TreeApp.Api/TreeApp.Api.csproj


# Get or create tree
POST /api.user.tree.get?treeName=MyTree

# Create root node
POST /api.user.tree.node.create?treeName=MyTree&nodeName=Root

# Create child node
POST /api.user.tree.node.create?treeName=MyTree&parentNodeId=1&nodeName=Child

# Rename node (must be unique among siblings)
POST /api.user.tree.node.rename?nodeId=2&newNodeName=RenamedChild

# Delete node and all descendants
POST /api.user.tree.node.delete?nodeId=1

# Trigger a SecureException (e.g., duplicate name)
POST /api.user.tree.node.create?treeName=MyTree&nodeName=Root

# Retrieve journal entry by Event ID (from error response)
POST /api.user.journal.getSingle?id=639040954984657252

# Get paginated logs with filtering
POST /api.user.journal.getRange?skip=0&take=10
Content-Type: application/json

{
  "from": "2026-01-15T20:00:00Z",
  "to": "2026-01-15T21:00:00Z",
  "search": "unique"
}

{
  "type": "Secure",
  "id": "639040954984657252",
  "data": {
    "message": "Node name must be unique among siblings"
  }
}

{
  "type": "Exception",
  "id": "639040954984657252",
  "data": {
    "message": "Internal server error ID = 639040954984657252"
  }
}