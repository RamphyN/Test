Proyect ASP.NET Core Web App (Model-View-Controller)
.net 8

instalar 

dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Oracle.EntityFrameworkCore --version 8.21.121
dotnet add package Oracle.ManagedDataAccess.Core --version 3.21.130

Crear Carpeta Data

En Carpeta Data, Crear ApplicationDbContext.cs


En program.cs, agregar
using Microsoft.EntityFrameworkCore;
using test1.Data;

 builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseOracle("User Id=SYS;Password=ramphynunez;Data Source=//localhost:1521/xe;DBA Privilege=SYSDBA"));

click derecho en controllers > MVC Controller with views, using Entity Framework.

Seleccionar model actual y ApplicationDbContext