# Minimal API Todo Management System

A modern, full-stack Task Management System built with **.NET 8**, featuring a decoupled architecture with a high-performance **Minimal API** backend and an interactive **Blazor WebAssembly/Server** frontend.



## 🚀 Features

* **Role-Based Access Control (RBAC):** Separate interfaces for Admins and regular Users.
* **JWT Authentication:** Secure identity management using JSON Web Tokens.
* **Admin Dashboard:** Full CRUD operations (Create, Read, Update, Delete) for managing tasks across all users.
* **User Workspace:** Personal task management area for tracking individual progress.
* **Modern UI:** Responsive design built with Bootstrap 5 and customized CSS.
* **Efficient Backend:** Lightweight Minimal API with Entity Framework Core and SQL Server.

## 🛠️ Tech Stack

**Backend:**
* ASP.NET Core 8 (Minimal API)
* Entity Framework Core
* SQL Server / SQLite
* JWT Authentication & Authorization

**Frontend:**
* Blazor (Interactive Server/WASM)
* Bootstrap 5 & Bootstrap Icons
* Blazored LocalStorage (for session management)

## 📋 Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server (or update ConnectionString in `appsettings.json` to use SQLite/InMemory)
* Visual Studio 2022 or VS Code

## ⚙️ Installation & Setup

1.  **Clone the repository:**
    ```bash
   (https://github.com/Abdulaziz77-Developer/Fullstack-Todo-Dotnet.git)
    cd minimal-api-todo
    ```

2.  **Database Migration:**
    Navigate to the Backend folder and run:
    ```bash
    dotnet ef database update
    ```

3.  **Run the Backend:**
    ```bash
    cd Backend
    dotnet run
    ```

4.  **Run the Frontend:**
    Open a new terminal, navigate to the Frontend folder and run:
    ```bash
    cd Frontend
    dotnet run
    ```

## 🔐 Default Credentials (Sample)
* **Admin:** `admin@example.com` / `Admin123!`
* **User:** `user@example.com` / `User123!`

## 📸 Screenshots
*(You can add your screenshots here later)*

---
Developed as a learning project for modern .NET web development.
