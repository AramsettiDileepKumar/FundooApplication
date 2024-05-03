# FunDooNotes

FunDooNotes is a web-based note-taking application designed to help users organize their thoughts, tasks, and ideas effectively. It offers a range of features, including password management, collaboration, trash notes, and more.

## Features

- **User Authentication**: Secure user registration and login functionality.
- **Password Management**: Ability to forget and reset passwords securely via email.
- **Note Management**: Create, update, delete, and archive notes with ease.
- **Collaboration**: Share notes with other users for collaborative editing.
- **Trash Notes**: Move unwanted notes to the trash for later deletion.

## Folder Structure

- **BusinessLayer**: Contains business logic for the application.
  - **Interfaces**: Contracts defining service interfaces.
  - **Services**: Implementation of business logic services.
- **FunDooNotes**: Main project directory.
  - **Controllers**: HTTP request handling controllers.
  - **Properties**: Application properties and configurations.
- **ModelLayer**: Data models used throughout the application.
  - **Models**: Data transfer objects and entity models.
- **RepositoryLayer**: Data access logic for interacting with the database using Dapper ORM.
  - **Context**: Database connection management.
  - **Interfaces**: Repository interfaces.
  - **Services**: Implementation of repository services.

## Getting Started

1. Clone the repository.
2. Set up the database connection in the appsettings.json file.
3. Run any database migrations or setup scripts required for your database.
4. Build and run the application.

## Technologies Used

- **ASP.NET Core**: Backend framework for building web applications.
- **Dapper**: Micro ORM for data access in .NET applications.
- **Swagger**: API documentation tool for testing and documenting APIs.
- **JWT Authentication**: Secure token-based authentication mechanism.
- **SMTP**: Email delivery service for sending password reset emails.

## Contributing

Contributions to FunDooNotes are welcome! Please fork the repository and submit a pull request with your changes. Make sure to follow the project's coding standards and conventions.
