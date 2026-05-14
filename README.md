# NovaCollege

This is an academic project in ASP.NET and Angular which it's main purpose is to show how to implement somes queries, angular components and css

## API Endpoints (Queries)

| Endpoint | Description | Type |
|----------|-------------|------|
| `GET /provincias/info` | Obtener las diferentes provincias a las que pertenecen los estudiantes y el número de estudiantes de cada provincia (SQL) | Stored Procedure |
| `GET /provincias/curso/{id}/estudiantes` | Obtener la provincia que tiene más estudiantes en el curso (curso debe ser un parámetro del procedimiento) y posteriormente como se ejecutaría en sqlserver ese procedimiento. | Stored Procedure |
| `GET /estudiantes` | Nos devuelva los estudiantes de una provincia (objetivo filtros api) | LINQ with Include |
| `GET /docentes` | Api que obtenga los datos y por LINQ haga lo siguiente: Tener una lista que por docente // nos de una lista de Información de Cursos , y para cada curso tengamos el listado de // provincias en las que tiene alumnos y para cada provincia la información de alumnos. El // objetivo es utilizar lo menos posible sentencias while, for o foreach.  | LINQ + SelectMany + GroupBy |
| `POST /estudiantes` | Insertar estudiantes | EF + FluentValidation |

## Angular Components

| Component | Description |
|-----------|-------------|
| `UserSettingsComponent` | Utilizando los componentes implementados en el ejercicio 1, crear una función en el componente hijo para incrementar la edad de un usuario. Mostrar la nueva edad en el componente padre mediante un console.log. |
| `UsersComponent` | Utilizar directivas estructurales de Angular para mostrar el nombre de cada personaje y si es mayor de edad |
| `CitizensComponent` | En el caso de que necesitemos primero obtener el id de usuario para luego enviar una consulta al mismo endpoint de cities para obtener las ciudades por id de usuario, ¿cómo lo implementarías en tu código para primero llamar a una api y luego a otra con el resultado de la primera?(cuando obtengas los usuarios, asume que todos viven en Belgium) |
| `CitizensComponent` (part 2) | ¿Cómo lo tenemos que hacer, con rxjs, para obtener los datos de ambos endpoints de esta API al mismo tiempo ? |
| `ElementOrderingComponent` | ¿Cómo harías, mediante css, para colocar el primer div debajo del segundo div? |

## Tech Stack

- Backend: ASP.NET Core 8 + FluentValidation + SQL Server + Entity Framework Core + EF Migrations + Docker + xUnit + TestContainers + Bogus
- Frontend: Angular 17 (standalone components, signals)
- Auth: Basic Authentication

Run `docker compose up` to start the full stack.