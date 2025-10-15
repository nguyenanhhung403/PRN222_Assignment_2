┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
║                  CarStore.WebUI                             ║
║              (Presentation Layer)                           ║
║  ┌─────────────────────────────────────────────────────┐   ║
║  │ Razor Pages:                                        │   ║
║  │  - Pages/Cars/Index.cshtml                          │   ║
║  │  - Pages/Orders/MyOrders.cshtml                     │   ║
║  │  - Pages/Users/Login.cshtml                         │   ║
║  │                                                      │   ║
║  │ Admin Area:                                         │   ║
║  │  - Areas/Admin/Pages/Cars/*                         │   ║
║  │  - Areas/Admin/Pages/Orders/*                       │   ║
║  └─────────────────────────────────────────────────────┘   ║
║  using CarStore.BO;        ← Import entities             ║
║  using CarStore.BLL.Services; ← Call services            ║
┗━━━━━━━━━━━━━━━━━━┳━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
                   │
                   │ Project Reference: BLL
                   │ (BO comes transitively)
                   ▼
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
║                 CarStore.BLL                                ║
║            (Business Logic Layer)                           ║
║  ┌─────────────────────────────────────────────────────┐   ║
║  │ Services:                                           │   ║
║  │  - CarService.cs                                    │   ║
║  │  - OrderService.cs                                  │   ║
║  │  - UserService.cs                                   │   ║
║  │  - TestDriveService.cs                              │   ║
║  │                                                      │   ║
║  │ Business Rules:                                     │   ║
║  │  - Validation                                       │   ║
║  │  - Business logic                                   │   ║
║  │  - Authorization logic                              │   ║
║  └─────────────────────────────────────────────────────┘   ║
║  using CarStore.BO;             ← Use entities           ║
║  using CarStore.DAL.Repositories; ← Call repositories    ║
║  Packages: NONE ❌                                        ║
┗━━━━━━━━━━━━━━━━━━┳━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
                   │
                   │ Project Reference: DAL
                   │ (BO comes transitively)
                   ▼
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
║                 CarStore.DAL                                ║
║             (Data Access Layer)                             ║
║  ┌─────────────────────────────────────────────────────┐   ║
║  │ DbContext:                                          │   ║
║  │  - CarStoreDbContext.cs                             │   ║
║  │                                                      │   ║
║  │ Repositories:                                       │   ║
║  │  - IGenericRepository.cs                            │   ║
║  │  - GenericRepository.cs                             │   ║
║  │  - CarRepository.cs                                 │   ║
║  │  - OrderRepository.cs                               │   ║
║  │  - UserRepository.cs                                │   ║
║  │  - TestDriveRepository.cs                           │   ║
║  └─────────────────────────────────────────────────────┘   ║
║  using CarStore.BO;        ← Use entities                ║
║  Packages: ✅                                             ║
║   • EF Core SqlServer                                     ║
║   • EF Core Tools                                         ║
║   • EF Core Design                                        ║
┗━━━━━━━━━━━━━━━━━━┳━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
                   │
                   │ Project Reference: BO (Direct)
                   ▼
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
║                  CarStore.BO                                ║
║            (Business Objects / Entities)                    ║
║  ┌─────────────────────────────────────────────────────┐   ║
║  │ POCO Classes:                                       │   ║
║  │  - Car.cs                                           │   ║
║  │  - Order.cs                                         │   ║
║  │  - User.cs                                          │   ║
║  │  - TestDrive.cs                                     │   ║
║  │                                                      │   ║
║  │ Pure C# classes, no logic                           │   ║
║  └─────────────────────────────────────────────────────┘   ║
║  Dependencies: NONE ❌                                    ║
║  Packages: NONE ❌                                        ║
┗━━━━━━━━━━━━━━━━━━┳━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
                   │
                   ▼
           ┌───────────────────┐
           │  SQL Server DB    │
           │   (CarStoreDB)    │
           └───────────────────┘
