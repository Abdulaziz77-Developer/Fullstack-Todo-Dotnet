# Minimal API ToDo Project (Advanced Version)

A robust ToDo management application built with **ASP.NET Core Minimal API**, featuring a secure authentication system and persistent data storage.

## 🌟 Key Features
- **JWT Authentication**: Secure login system and protected endpoints using JSON Web Tokens.
- **Entity Framework Core 9**: Full integration with **SQL Server** for persistent data management.
- **BCrypt Security**: Industry-standard password hashing to ensure user data protection.
- **User Data Isolation**: Built-in logic ensuring users can only access and manage their own tasks.
- **Interactive Documentation**: Integrated Swagger UI for seamless API testing and exploration.

## 🛠 Tech Stack
- **Runtime**: .NET 9
- **Database ORM**: Entity Framework Core 9 (SQL Server)
- **Security**: JWT Bearer Authentication, BCrypt.Net-Next
- **API Documentation**: Swashbuckle (Swagger)

## 📌 API Endpoints

### Authentication (Public)
- `POST /auth/login` — Authenticate user and receive a JWT Token.
  - *Seed Account:* `sergey@example.com` / `Password123!`

### ToDo Management (Protected - Token Required)
- `GET /todos` — Retrieve all tasks for the authenticated user.
- `GET /todos/{id}` — Get a specific task by ID.
- `POST /todos` — Create a new task (automatically linked to the current user).
- `PUT /todos/{id}/toggle` — Toggle the `isCompleted` status of a task.
- `DELETE /todos/{id}` — Remove a task from the database.

## ⚙️ Setup and Installation

1. **Clone the repository:**
   ```bash 
   git clone [https://github.com/YOUR_USERNAME/MinimalApiTodo.git](https://github.com/YOUR_USERNAME/MinimalApiTodo.git)