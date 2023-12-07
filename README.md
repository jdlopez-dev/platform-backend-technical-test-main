# Analyticalways backend technical test

- Date created: 08/05/2022
- Autor of the project: Juan Lopez

## First of all

Thinking about the future of the application in its expansion and maintenance. I have considered that the best thing to do is to apply a **clean architecture**.

This will add complexity to the project but will bring some advantages such as dependencies and unit tests.

But the most important thing is that it will allow us to **decouple the business logic** from the technology we apply and this will allow us to be more **flexible to change**.

Personally, I prefer to have a defined architecture that is flexible to change from the beginning than to have a project with a lot of complexity and without a **clean architecture**.

---

### Configuration environment and tools for development:

1. `winget install -e --id Microsoft.VisualStudioCode --override '/SILENT /mergetasks="!runcode,addcontextmenufiles,addcontextmenufolders`
2. `winget install -e --id Microsoft.VisualStudio.2022.Community-Preview`
3. `winget install -e --id GitHub.GitHubDesktop`
4. `winget install -e --id Git.Git`
5. `winget install -e --id Google.Chrome`
6. `winget install -e --id Microsoft.SQLServerManagementStudio`
7. `git config --global init.defaultBranch main`
8. `git config --global user.email email`
9. `git config --global user.name "user"`

### Creation of the necessary layers for our clean architecture.

0. `src/AcmeCorporationApi/AcmeCorporationApi.csproj`
1. `dotnet new classlib -o src/AcmeCorporation.Core --framework net5.0`
2. `dotnet new classlib -o src/AcmeCorporation.Infrastructure --framework net5.0`
3. `dotnet new classlib -o src/AcmeCorporation.Services --framework net5.0`

Add projects to the solution.

1. `dotnet sln add src/AcmeCorporation.Core/AcmeCorporation.Core.csproj --in-root`
2. `dotnet sln add src/AcmeCorporation.Infrastructure/AcmeCorporation.Infrastructure.csproj --in-root`
3. `dotnet sln add src/AcmeCorporation.Services/AcmeCorporation.Services.csproj --in-root`

Create references between them

1. `dotnet add src/AcmeCorporationApi/AcmeCorporationApi.csproj reference src/AcmeCorporation.Core/AcmeCorporation.Core.csproj src/AcmeCorporation.Services/AcmeCorporation.Services.csproj`
2. `dotnet add src/AcmeCorporation.Infrastructure/AcmeCorporation.Infrastructure.csproj reference src/AcmeCorporation.Core/AcmeCorporation.Core.csproj`
3. `dotnet add src/AcmeCorporation.Services/AcmeCorporation.Services.csproj reference src/AcmeCorporation.Core/AcmeCorporation.Core.csproj`
4. `dotnet add src/AcmeCorporationApi/AcmeCorporationApi.csproj reference src/AcmeCorporation.Infrastructure/AcmeCorporation.Infrastructure.csproj`

Configure the database and entity framework

1. `dotnet tool install --global dotnet-ef`
2. `dotnet add src/AcmeCorporation.Infrastructure/AcmeCorporation.Infrastructure.csproj package Microsoft.EntityFrameworkCore --version 5.0.16`
3. `dotnet add src/AcmeCorporation.Infrastructure/AcmeCorporation.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 5.0.16`
4. `dotnet add src/AcmeCorporation.Infrastructure/AcmeCorporation.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.16`

AutoMapper

1. `dotnet add src/AcmeCorporationApi/AcmeCorporationApi.csproj package AutoMapper`
2. `dotnet add src/AcmeCorporationApi/AcmeCorporationApi.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection`

FluentValidation

1. `dotnet add src/AcmeCorporation.Services/AcmeCorporation.Services.csproj package FluentValidation`

### Testing

Crete projects for testing.

1. `dotnet new xunit -o src/AcmeCorporationApi.Tests --framework net5.0`
2. `dotnet new xunit -o src/AcmeCorporation.Services.Tests --framework net5.0`

Add projects to the solution.

1. `dotnet sln add src/AcmeCorporationApi.Tests/AcmeCorporationApi.Tests.csproj --in-root`
2. `dotnet sln add src/AcmeCorporation.Services.Tests/AcmeCorporation.Services.Tests.csproj --in-root`

Add references.

1. `dotnet add src/AcmeCorporationApi.Tests/AcmeCorporationApi.Tests.csproj reference src/AcmeCorporationApi/AcmeCorporationApi.csproj`

Add necessary dependencies to the projects.

1. `dotnet add src/AcmeCorporationApi.Tests/AcmeCorporationApi.Tests.csproj package Microsoft.AspNetCore.TestHost --version 5.0.16`
2. `dotnet add src/AcmeCorporation.Services.Tests/AcmeCorporation.Services.Tests.csproj package Moq`

### Conclusiones

Para detallar mejor, optaré por escribir en español las conclusiones de mi trabajo.

Comenzaré destacando los aspectos más relevantes de la arquitectura. Desde el principio, diseñé varias capas para la aplicación, pensando en su futura expansión y mantenimiento.

Cada capa posee características específicas:

- La API ha sido optimizada, eliminando dependencias como la base de datos, y configurada para actuar como un proveedor de acceso, inyectando los servicios necesarios.

- La capa de infraestructura se encarga del acceso y manejo de la base de datos.

- En la capa de servicios se centraliza la lógica de negocio y las validaciones.

- La capa Core tiene el propósito de aglutinar todos los contratos (interfaces) y entidades centrales de la aplicación.

Definidas las capas y sus responsabilidades, el siguiente paso es iterar y comenzar a crear las funcionalidades requeridas. En este caso, la obtención de datos relacionados con personas y sus tipos de documento.

He seguido los principios SOLID, en especial el principio de responsabilidad única. Por ello, creé el servicio "DocumentTypeService", que mediante expresiones regulares asigna el tipo de documento a una persona. Fue un requisito pensado también para futuras expansiones, donde, al añadir nuevos tipos de documentos, este servicio pueda ser reemplazado fácilmente.

---

> Finalmente, parece que el método `POST` del `PersonsController` permite enviar el campo `id` de la Persona, lo cual no es correcto y debería modificarse.

Para abordar el problema mencionado, decidí crear un tipo de dato `Record` que excluyera el campo `id`:

public record PersonSaveModel(string Name, int Age, string Document);


---

### Pruebas de Integración y Unitarias

En cuanto a las pruebas de integración, me basé en la siguiente premisa:

> Lo que realmente queremos es probar la API como una caja negra, consumiéndola como un usuario final.

Así, inicié la generación de varias pruebas, viendo la necesidad de agregar un método de `borrado` para mantener la integridad de la base de datos y realizar diversos tipos de pruebas.

Creo que es posible invertir más tiempo en refactorizar la pila de pruebas realizadas. Estos tests también me han brindado nuevas ideas para mejorar la lógica de negocio.

### .NET 5 vs .NET 6

El proyecto API proporcionado estaba en la versión .NET 5. Consideré migrar a .NET 6, pero esto implicaría una serie de cambios que requerirían más tiempo y dedicación.

Fue algo que tuve que analizar desde el inicio, dado que se menciona la importancia de personalizar el código. Personalmente, me gusta intentar utilizar la versión más estable del código, evaluando si es factible hacer la actualización.

## Consultas SQL
