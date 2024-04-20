# ConCareConnect
ConCareConnect is a laboratory panel that is used to manage the laboratory tests and results of patients. It is a desktop application that is developed using WPF and C#. The backend is developed using C# and the frontend is developed using WPF. The project is developed using Visual Studio 2022.


## Table of Contents
1. [Features](#features)
2. [Technologies](#technologies)
3. [Tools](#tools)
4. [Directory Structure](#directory-structure)
5. [Installation](#installation)
6. [Basic Usage](#basic-usage)
7. [Screenshots](#screenshots)
8. [Contributing](#contributing)
9. [License](#license)


## Features
1. Manage Patients
2. Manage Appointments
3. Manage Pathology Tests
4. Generate Reports
5. Generate Bills
6. Manage Financial Transactions

## Technologies

**1. [.Net 8.0 - C#](https://docs.microsoft.com/en-us/dotnet/csharp/)** - For Overall Development
**2. [WPF](https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-wpf?view=vs-2022)** - Windows Presentation Foundation for the frontend (As most of the users are using Windows OS)
**3. ASP.Net Core** - For backend development
**4. Postgres** - For database management [![Postgres](https://img.shields.io/badge/Postgres-13.4-blue)](https://www.postgresql.org/)
**5. Entity Framework Core** - For database operations [![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-8.0.3-blue)](https://docs.microsoft.com/en-us/ef/core/)
**6. iTextSharp** - For generating PDF reports [![iTextSharp](https://img.shields.io/badge/iText7-8.0.3-blue)](https://www.nuget.org/packages/iTextSharp/)
**7. WPF UI** - For designing the user interface [![WPF](https://img.shields.io/badge/WP%20UI-3.0.1-blue)](https://www.nuget.org/packages/WPF-UI)


## Tools
**1. Visual Studio 2022** - For development
**2. Postman** - For API testing
**3. Swagger** - For API documentation


## Directory Structure
```bash
├── README.md
├── .gitignore
├── LaboratoryPanelWPF - Main project folder using WPF
│   ├── LaboratoryPanelWPF.sln
│   ├── LaboratoryPanelWPF
│── PatholofyBackendCS - Backend project folder using C#
│   ├── PatholofyBackendCS.sln
│   ├── PatholofyBackendCS
```

## Installation
**Backend**
```bash
## Go to the PatholofyBackendCS folder
dotnet restore
dotnet build
dotnet run

## If you have not database created, run the following command to create the 
dotnet ef database update
```

**Frontend**
```bash
## Go to the LaboratoryPanelWPF folder
dotnet restore
dotnet build
dotnet run
```


## Basic Usage
1. Run the PatholofyBackendCS project first or Host the backend project in server.
2. Run the LaboratoryPanelWPF project or generate the exe file and run it.

## Screenshots

<img src="./images/Screenshot%202024-03-22%20224322.png" width="300" height="200">
<img src="./images/Screenshot%202024-03-22%20224350.png" width="300" height="200">
<img src="./images/Screenshot%202024-03-22%20224403.png" width="300" height="200">
<img src="./images/Screenshot%202024-03-22%20224415.png" width="300" height="200">

## Contributing
1. Fork the repository.
2. Clone the repository.
3. Create a new branch.
4. Make necessary changes and commit those changes.
5. Push the changes to the repository.
6. Submit your changes for review.

## License
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.


