# AgileStockPro - Progress Update

## Architectural Changes
- **Refactoring**: Converted the Blazor Server application to a **Monolithic N-Layer Architecture** by directly referencing the `src` projects (`BusinessLogic`, `DataAccess`, `Contracts`, `SharedKernel`). This eliminates the need for a separate API project and simplifies the deployment and development workflow.
- **Dependency Injection**: Configured `Program.cs` to register all backend services using `AddBusinessLogic` and `AddDataAccess` extension methods.

## Security Upgrades
- **Password Hashing**: Upgraded the password hashing algorithm from SHA256 to **Argon2id** (winner of the Password Hashing Competition) using `Konscious.Security.Cryptography.Argon2`. This meets the requirement for "modern encryption".
- **Authentication**: Implemented a `CustomAuthenticationStateProvider` to handle user login and session management within the Blazor Server app.
- **Login Page**: Created a new `Login.razor` page using Fluent UI components.

## New Modules Implemented
### Suppliers (Proveedores)
- **Backend**: Created `SupplierDto`, `ISupplierRepository`, `SqlSupplierRepository`, `ISupplierService`, and `SupplierService`.
- **Frontend**: Updated `Proveedores.razor` to use `FluentDataGrid` and the new `ISupplierService`.

### Clients (Clientes)
- **Backend**: Created `ClientDto`, `IClientRepository`, `SqlClientRepository`, `IClientService`, and `ClientService`.
- **Frontend**: Updated `Clientes.razor` to use `FluentDataGrid` and the new `IClientService`.

### Purchases (Compras)
- **Backend**: Created `PurchaseDto`, `IPurchaseRepository`, `SqlPurchaseRepository`, `IPurchaseService`, and `PurchaseService`.
- **Frontend**: Updated `OrdenesCompra.razor` to use `FluentDataGrid` and the new `IPurchaseService`.

### Sales (Ventas)
- **Backend**: Created `SaleOrderDto`, `ISalesRepository`, `SqlSalesRepository`, `ISalesService`, and `SalesService`.
- **Frontend**: Updated `Ventas.razor` to use `FluentDataGrid` and the new `ISalesService`.

### Stock
- **Backend**: Updated `IStockService` and `SqlStockRepository` to include `GetStockAsync` for retrieving current stock levels. Created `StockItemDto`.
- **Frontend**: Updated `Stock.razor` to use `FluentDataGrid` and the new `IStockService`.

## Verification Guide

### 1. Database Setup
1.  Open SQL Server Management Studio (SSMS).
2.  Open `full_database_script.sql`.
3.  Execute the script to create the `login2` database and populate it with initial data.

### 2. Run Application
1.  Open a terminal in the solution root.
2.  Run the application:
    ```bash
    dotnet run --project blazor-server/AgileStockPro.Web
    ```
3.  Open your browser and navigate to `https://localhost:7130` (or the URL shown in the terminal).

### 3. Manual Testing Steps
#### Login
- Use one of the demo users (e.g., `admin` / `admin123` or check `seed_demo_users.sql` for credentials).

#### Suppliers & Clients
- Navigate to **Proveedores** and create a new supplier.
- Navigate to **Clientes** and create a new client.

#### Purchases (Compras)
- Navigate to **Órdenes de Compra**.
- Create a new Purchase Order.
- Verify it appears in the list.

#### Sales (Ventas)
- Navigate to **Ventas**.
- Create a new Sale.
- Verify it appears in the list.

#### Stock
- Navigate to **Stock**.
- Check the list of products and their stock levels.
- Verify that stock levels reflect the initial seed data or any movements created.

## Conclusion
The System-Stock C# project has been successfully refactored and extended with the following modules:
- **Login / User Management**
- **Suppliers**
- **Clients**
- **Purchases**
- **Sales**
- **Stock**

The database scripts have been consolidated into `full_database_script.sql`.
The application is ready for final testing and deployment.

## Configuration
- Created `appsettings.json` in `AgileStockPro.Web` with the database connection string.
