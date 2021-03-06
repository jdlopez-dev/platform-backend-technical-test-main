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

### Conclusions

Para expresarme con m??s detalle voy escribir en espa??ol las conclusiones de mi trabajo.

Voy a empezar por los aspectos m??s importantes de la arquitectura. Decid?? crear var??as capas para la aplicaci??n desde un principio pensado en la futura expansi??n y mantenimiento de la aplicaci??n.

Cada capa tiene la siguiente caracter??stica:

- La Api, la he limpiado y quitado dependecias como la base de datos. Dej??ndola como proveedor de acceso e inyect??ndole los servicios que necesarios.

- La capa de infraestructura es la que encarga del acceso y manejo de la base de datos.

- La capa de servicios es donde centraremos nuestra l??gica de negocio y validaciones.

- La capa Core tiene como objetivo recoger todos los contratos (interfaces) y entidades centrales de la aplicaci??n.

Una vez definidas las capas y responsabilidades de cada una solo queda iterar y empezar a crear las funcionalidades solicitadas. En este caso la obtenci??n de datos relacionados con personas y su tipo de documento.

He intentado seguir los principios SOLID y sobre todo el principio de responsabilidad ??nica por ello decid?? crear un servicio de "DocumentTypeService" que se encarga mediante expresiones regulares de asignar el tipo de documento a una persona era un requisito y tambi??n pensado en una futura expansi??n donde se agreguen nuevo tipos de documentos este servicio pueda ser remplazado por otro.

---

> Finally, it seems that the `POST` method of the `PersonsController` allows you to send the Person `id` field and that's not right, you should do something about it.

Para la cuesti??n anterior citada decide crear un tipo de dato Record que no incluyera el campo `id`.

```
public record PersonSaveModel(string Name, int Age, string Document);
```

---

### Pruebas de integraci??n y unitarias

En la parte de integraci??n decid?? basarme en el siguiente punto:

> really want is to test the API as a black box, consuming it as an end user.

Por ello empece a generar varias pruebas donde vi la necesidad de agregar un m??todo de `borrado` para poder conservar la integridad de la base de datos y realizar diversos tipo de pruebas.

Creo que puedo invertir m??s tiempo y realizar un refactor sobre la base de pila de pruebas que realice y al mismo tiempo estos test me daban nuevas ideas de mejorar la l??gica de negocio.

### .NET5 VS NET6

El proyecto api aportado estaba en la versi??n de .Net 5, pense en realizar una migraci??n a la versi??n de .Net 6 pero esto llevar??a una serie de cambios que implicar??an mas tiempo y dedicaci??n a la prueba.

Es algo que tuve que analizar desde al empezar la prueba dado que mencion??is de hacer el c??digo nuestro y es algo que mi en lo personal me gusta intentar utilizar la versi??n mas estable de c??digo pero evaluando si es posible hacerlo.

## Consultas SQL

Para las consultas SQL las que han representado un reto para mi ha sido la 2 y 3.

Es un tema que tengo pendiente de investigar y aprender m??s pero mi perfil de programador me he orientado m??s aun full stack y aunque no sea mi fuerte la gesti??n y optimizaci??n de base de datos he trabajado creando vistas, procedimientos y funciones para la gesti??n de datos.
