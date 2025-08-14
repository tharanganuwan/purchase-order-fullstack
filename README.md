# purchase-order-fullstack

Purchase Order Management Application
Overview

This project is a Purchase Order Management system demonstrating a full-stack solution using .NET Core Web API for the backend and Angular for the frontend. The application allows users to create, view, and manage purchase orders.

Approach and Design Decisions
Backend

Framework: .NET Core Web API for lightweight, cross-platform server-side development.

Architecture: Followed Clean Architecture principles:

Controllers handle HTTP requests.

Services contain business logic.

Repositories abstract data access with EF Core.

Entity Framework Core: Code-First approach used for database migrations.

DTOs & AutoMapper: Decoupled database entities from API responses for better maintainability.

Error Handling: Try-catch blocks implemented in services to ensure robust error reporting.

Frontend

Framework: Angular 19 (Standalone Components for simplicity and modularity).

Structure:

Components: OrderListComponent, OrderFormComponent.

Services: PurchaseOrderService for API interaction.

Reactive Forms: Used for form handling and validation.

Environment Configuration: API URLs configurable via environment.ts.

Assumptions

SQL Server is used and accessible from the development environment.

No authentication/authorization implemented for the demo.

Backend runs on https://localhost:5113 and frontend on http://localhost:4200.

Some seed data is assumed for testing purposes.

Setup Instructions
Backend
# Restore packages
dotnet restore

# Apply migrations and create database
dotnet ef database update

# Run the API
dotnet run

Frontend
# Navigate to project folder
cd PurchaseOrderClient

# Install dependencies
npm install

# Run the frontend
ng serve


Open the browser at http://localhost:4200.


# Testing

Backend: dotnet test
Frontend: (tests not implemented)