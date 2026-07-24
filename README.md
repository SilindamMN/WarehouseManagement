# Inventory Management API

> Built as a technical assessment for a job interview.

A RESTful API built with .NET for managing products, warehouses, and inventory movement between warehouses. Supports listing/creating products and warehouses, creating orders that move stock from one warehouse to another, and querying on-hand quantities by product or warehouse.

---

## 📑 Table of Contents
- [Project Overview](#project-overview)
- [Data Model](#data-model)
- [Technologies Used](#technologies-used)
- [Setup Instructions](#setup-instructions)
- [API Endpoints](#api-endpoints)
  - [Products](#products)
  - [Warehouses](#warehouses)
  - [Orders](#orders)
  - [Stock](#stock)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Project Structure](#project-structure)

---

## 📦 Project Overview
This project implements a simple **Inventory Management System**:
- **Products** — list and create (`code` unique, `description`).
- **Warehouses** — list and create (`code` unique, `name`).
- **Orders** — move a quantity of a product from a source warehouse to a destination warehouse; decrements source stock, increments destination stock, and rejects the move if the source doesn't have enough stock.
- **Stock query** — list on-hand quantities per product per warehouse, filterable by product code or warehouse code.

---

## Data Model

| Entity | Fields |
|---|---|
| **Product** | `Id`, `Code` (unique), `Description` |
| **Warehouse** | `Id`, `Code` (unique), `Name` |
| **Stock** | `Id`, `ProductId`, `WarehouseId`, `Quantity` — one row per product/warehouse combination |
| **Order** | `Id`, `ProductId`, `SourceWarehouseId`, `DestinationWarehouseId`, `Quantity`, `CreatedAt` |

Stock is never edited directly — it only changes as a side effect of placing an `Order`, keeping the movement history auditable via the `Order` table.

---

## 🛠️ Technologies Used
- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** (Code-First) as ORM
- **SQLite** for local development (swappable for SQL Server/MySQL)
- **C#**

---

## 🚀 Setup Instructions

### 1. Restore & build
```bash
dotnet restore
dotnet build
```

### 2. Apply migrations
```bash
dotnet ef migrations add InitialCreate -s ./API -p ./Persistence
dotnet ef database update -s ./API -p ./Persistence
```

### 3. Seed order
There is no required creation order enforced by the API beyond referential integrity, but a natural demo flow is:
1. Create warehouses
2. Create products
3. Create an order to move stock into a warehouse for the first time (a "receipt" can be modeled as an order from a virtual/default warehouse, or seeded directly in stock — see [Notes](#notes) below)

---

## API Endpoints

### Products
| Method | Route | Description |
|---|---|---|
| GET | `/api/products` | List all products |
| POST | `/api/products` | Create a product |

**POST /api/products**
```json
{
  "code": "P001",
  "description": "Widget A"
}
```

### Warehouses
| Method | Route | Description |
|---|---|---|
| GET | `/api/warehouses` | List all warehouses |
| POST | `/api/warehouses` | Create a warehouse |

**POST /api/warehouses**
```json
{
  "code": "WH001",
  "name": "Main Warehouse"
}
```

### Orders
| Method | Route | Description |
|---|---|---|
| POST | `/api/orders` | Move a quantity of a product from one warehouse to another |

**POST /api/orders**
```json
{
  "productCode": "P001",
  "sourceWarehouseCode": "WH001",
  "destinationWarehouseCode": "WH002",
  "quantity": 10
}
```
- Rejects the request (400) if the source warehouse doesn't hold enough of the product.
- On success, decrements `Stock.Quantity` for the source and increments (or creates) it for the destination.

### Stock
| Method | Route | Description |
|---|---|---|
| GET | `/api/stock?productCode=P001` | On-hand quantity of a product across all warehouses |
| GET | `/api/stock?warehouseCode=WH001` | On-hand quantities of all products in a warehouse |

---

## Running the Application
```bash
cd API
dotnet watch run
```
Swagger UI is available at `https://localhost:<port>/swagger` for exploring and calling the endpoints directly.

---

## Testing
- Use Swagger UI or Postman to exercise the endpoints.
- Suggested manual test flow:
  1. Create two warehouses.
  2. Create a product.
  3. Seed initial stock into warehouse A.
  4. Create an order moving some quantity from A to B.
  5. Query stock by product code — confirm split across A and B sums correctly.
  6. Attempt to move more than what's in the source warehouse — confirm it's rejected.

---

## Project Structure
```
Inventory.sln
├── API/              # Controllers, Program.cs, DTOs
├── Application/      # Business logic / handlers
├── Domain/           # Entities: Product, Warehouse, Stock, Order
└── Persistence/       # DbContext, EF Core migrations
```

---

## Notes
- Since the spec doesn't define a dedicated "stock receipt" endpoint, initial stock is seeded directly via the database or via an order from a placeholder/default warehouse — call this out during the walkthrough as a deliberate scope decision.
