
## Kiến trúc Hệ thống
```
┌─────────────────────────────────────────────┐
│         CarStore.WebUI                      │
│       (Presentation Layer)                  │
│ • Razor Pages                               │
│ • Authentication & Authorization            │
└──────────────┬──────────────────────────────┘
               │ references, calls Services
               ▼
┌─────────────────────────────────────────────┐
│         CarStore.BLL                        │
│      (Business Logic Layer)                 │
│ • Services (Car, Order, User, TestDrive)    │
│ • Business Rules & Validation               │
└──────────────┬──────────────────────────────┘
               │ references, calls Repositories
               ▼
┌─────────────────────────────────────────────┐
│         CarStore.DAL                        │
│       (Data Access Layer)                   │
│ • DbContext (CarStoreDbContext)             │
│ • Repositories                              │
└──────────────┬──────────────────────────────┘
               │ references, maps to/from
               ▼
┌─────────────────────────────────────────────┐
│         CarStore.BO                         │
│    (Business Objects / Entities)            │
│ • POCO Classes                              │
│ • Entities: Car, Order, User, TestDrive     │
└──────────────┬──────────────────────────────┘
               │ stored in, mapped to
               ▼
┌─────────────────────────────────────────────┐
│      SQL Server Database (CarStoreDB)       │
│ • Tables: Cars, Orders, Users, TestDrives   │
└─────────────────────────────────────────────┘
```
