# 🧩 Todo API (ASP.NET Core)

A simple RESTful API built with ASP.NET Core for managing tasks (Todo items).  
This project demonstrates backend fundamentals such as handling HTTP requests, implementing CRUD operations, validation, and structuring application logic.

---

## 🚀 Features

- Get all tasks  
- Get a task by ID  
- Create a new task  
- Update a task  
- Delete a task  
- Mark task as done / not done  
- Filter tasks (done / not done)  
- Input validation (Title cannot be empty)

---

## 📌 Endpoints

### GET
- /tasks → Get all tasks  
- /tasks/{id} → Get task by ID  
- /tasks/done → Get completed tasks  
- /tasks/notdone → Get incomplete tasks  

### POST
- /tasks → Create a new task  

### PUT
- /tasks/{id} → Update task  

### PATCH
- /tasks/{id}/done → Mark task as done  
- /tasks/{id}/undone → Mark task as not done  

### DELETE
- /tasks/{id} → Delete task  

---

## 🧠 What I learned

- How to build a REST API using ASP.NET Core  
- How HTTP methods work (GET, POST, PUT, PATCH, DELETE)  
- How to handle incoming requests and return responses  
- How to work with objects and collections in C#  
- How to validate user input  
- How to structure backend logic  

---

## ▶️ How to run

dotnet run

Then open Swagger in your browser:

https://localhost:xxxx/swagger

---

## 📷 Example

{
  "id": 1,
  "title": "Learn C#",
  "isDone": false
}

---

## 💬 Summary

This project represents my understanding of backend development using C# and ASP.NET Core.  
It focuses on building a functional API, handling data, and implementing real-world logic in a clean and structured way.
