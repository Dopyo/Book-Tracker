# Simple Book Tracker

A straightforward web application for tracking books in a personal or small library setting, featuring a .NET backend API and a Blazor Server frontend.

## Project Demo

[![Simple Book Tracker Demo](https://img.youtube.com/vi/TNLm3vJwkNA/hqdefault.jpg)](https://www.youtube.com/watch?v=TNLm3vJwkNA)

## Overview

This project provides core functionalities for managing a book collection, including adding new books, viewing book details, and listing available books. It's built using modern .NET technologies, showcasing a RESTful API backend and an interactive Blazor Server UI.

## Features

**Backend API (ASP.NET Core):**

*   **CRUD Operations for Books:**
    *   `POST /books`: Add a new book (including details like ISBN, title, authors, genre, publication year, etc.).
    *   `GET /books`: Retrieve a list of all books (summary view).
    *   `GET /books/{id}`: Retrieve detailed information for a specific book.
    *   `DELETE /books/{id}`: Remove a book from the collection.
    *   *(Future Scope: PUT /books/{id} for updating book details)*
*   **Relational Data Model:** Manages books, authors, genres, patrons, and borrowing records.
*   **Data Persistence:** Uses SQL Server with Entity Framework Core.
*   **Validation:** Includes basic input validation for creating books.
  ![Image](https://github.com/user-attachments/assets/be3532e1-0d57-4648-87ee-c529630948eb)

**Frontend (Blazor Server):**

*   **Book Listing:** Displays a list of books from the backend.
*   **Book Detail View:** Shows comprehensive details for a selected book when navigating to `/book/{id}`.
*   **Interactive UI:** Leverges Blazor Server for dynamic updates and event handling.
*   **API Integration:** Communicates with the backend API to fetch and display data.

## Technologies Used

**Backend:**

*   **.NET 9** (or your current .NET version)
*   **ASP.NET Core Web API:** For building RESTful services.
*   **Entity Framework Core:** For ORM and database interaction.
*   **SQL Server:** As the relational database.
*   **Minimal APIs:** For concise endpoint definitions.

**Frontend:**

*   **.NET 9** (or your current .NET version)
*   **Blazor Server:** For building the interactive web UI with C#.
*   **Razor Components:** For UI structure and logic.
*   **Bootstrap:** (Likely used for basic styling, common with Blazor templates).

**Database Design (Conceptual):**

*   **Books:** ISBN, Title, Genre, PublicationYear, Publisher, Copies, etc.
*   **Authors:** Author details. (Many-to-Many with Books)
*   **Genres:** Genre names and descriptions.
*   **Patrons:** User/member details.
*   **BorrowingRecords:** Tracks book checkouts, due dates, returns.
*   **(Visualized using dbdiagram.io with provided DBML)**

## Project Structure (Conceptual)

*   **`BookTracker` (API Project - e.g., Solution Folder Name):**
    *   `Program.cs`: API setup, EF Core, endpoints.
    *   `Models/`: Entity classes (Book, Author, Genre, etc.).
    *   `Data/ApplicationDbContext.cs`: EF Core DbContext.
    *   `DTOs/`: Data Transfer Objects (CreateBookDto, BookDetailDto, etc.).
    *   `appsettings.json`: Connection strings, configuration.
*   **`LibraryManager` (Blazor Frontend Project):**
    *   `Program.cs`: Blazor Server setup, HttpClient configuration.
    *   `Components/Pages/`: Routable Razor components (e.g., `Home.razor`, `BookDetail.razor`).
    *   `Components/Shared/`: Shared UI components (e.g., `MainLayout.razor`, `NavMenu.razor`).
    *   `Services/BookApiService.cs`: Service for API communication.
    *   `_Imports.razor`: Common using directives.
    *   `appsettings.json`: API base URL configuration.

## Setup and Running

### Prerequisites

*   .NET SDK (version corresponding to the project, e.g., .NET 9 SDK)
*   SQL Server (Express, Developer, or any other edition)
*   A tool to manage SQL Server (e.g., SQL Server Management Studio (SSMS) or Azure Data Studio)
*   IDE (e.g., Visual Studio, VS Code)

### Backend API Setup

1.  **Clone the repository (or ensure you have the API project files).**
2.  **Database Setup:**
    *   Open the API project (e.g., `BookTracker.sln` or the API project folder).
    *   Update the `ConnectionStrings:DefaultConnection` in `appsettings.json` to point to your SQL Server instance and desired database name (e.g., `MyBookstoreDB`).
    *   **Apply Migrations:**
        *   Open a terminal in the API project directory.
        *   Run `dotnet ef database update`. This will create the database and tables based on the Entity Framework Core migrations.
        *   *(If migrations don't exist yet, you'd first run `dotnet ef migrations add InitialCreate`)*
    *   **(Optional) Seed Data:** You can use the provided SQL scripts (or create your own) to populate initial data for `Genres`, `Authors`, etc.
3.  **Run the API:**
    *   From the API project directory, run `dotnet run` or `dotnet watch run`.
    *   The API will typically start on a port like `http://localhost:5086` or `https://localhost:7XYZ`. Note this URL.

### Frontend Blazor App Setup

1.  **Clone the repository (or ensure you have the Blazor project files).**
2.  **Configure API URL:**
    *   Open the Blazor project (e.g., `LibraryManager.sln` or the Blazor project folder).
    *   In `appsettings.json` (and `appsettings.Development.json`), update the `ApiSettings:BaseUrl` to match the URL where your backend API is running (e.g., `http://localhost:5086`).
3.  **Run the Blazor App:**
    *   From the Blazor project directory, run `dotnet run` or `dotnet watch run`.
    *   The Blazor app will start on a different port (e.g., `https://localhost:7ABC` or `http://localhost:5ABC`).
    *   Open this URL in your browser.

## Usage

*   Navigate to the Blazor application's URL in your web browser.
*   The home page should display a list of books fetched from the API.
*   Click on a book (or a "View" button) to see its detailed information on the `/book/{id}` page.

## Future Enhancements (Potential)

*   Implement `PUT` endpoint for updating book details.
*   Add functionality for managing Authors and Genres via the UI.
*   Implement Patron management and book borrowing/return workflows.
*   Add user authentication and authorization.
*   Improve UI/UX with more advanced Blazor features or component libraries.
*   Implement searching and filtering for books.
*   Unit and integration tests.

---
