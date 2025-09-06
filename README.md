# Tea Leaves Production Forecast 🌱

A web-based system designed to predict tea leaves production using **Machine Learning**, built with **.NET 8 Web API** (backend) and **Angular** (frontend).  
The project helps farmers and stakeholders make data-driven decisions by providing accurate yield forecasts based on historical production data.

---

## 📌 Features
- 🔐 **User Authentication** – JWT-based login and registration  
- 📊 **Prediction Module** – Forecast tea production using a trained ML model  
- 👨‍💻 **User Management** – Manage users and roles efficiently  
- 🧪 **Unit Testing** – Backend (xUnit + Moq), Frontend (Jasmine + Karma)  
- 📑 **Manual Testing** – Structured test plan and test cases to validate functionality  

---

## 🛠️ Tech Stack

### Frontend
- Angular 18/19  
- TypeScript  
- Bootstrap 5  

### Backend
- .NET 8 Web API  
- Entity Framework Core  
- MS SQL Server 2022  
- JWT Authentication  

### Machine Learning
- Python (Scikit-learn, Pandas, NumPy)  
- Pre-trained model files (`.pkl`)  

### Testing
- Backend: xUnit, Moq  
- Frontend: Jasmine, Karma  

---

## 📂 Project Structure
# Tea Leaves Production Forecast

## 📝 Project Overview
This project predicts tea leaves production (in kg) from tea plants using machine learning. 
It is built as a web system with a **.NET 8 Web API backend** and **Angular frontend**. 
Users can register, log in, and view production predictions.  

## 🎯 Features
- User registration and login with JWT authentication  
- ML-based tea leaves production prediction  
- Dashboard to display predicted values  
- RESTful APIs with .NET 8 Web API  
- Angular frontend for interactive UI  

## 🛠 Technology Stack

| Layer     | Technology                       |
|----------|----------------------------------|
| Frontend  | Angular 19, Bootstrap 5         |
| Backend   | .NET 8 Web API, C#              |
| Database  | MS SQL Server 2022               |
| Machine Learning | Python 3.10, scikit-learn, pickle |
| Authentication | JWT (JSON Web Tokens)        |

## ⚙️ Project Setup

### Backend Setup
```bash
cd Backend
dotnet restore
dotnet build
dotnet run
