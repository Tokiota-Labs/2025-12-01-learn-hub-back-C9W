# Instrucciones para generar la estructura de carpetas en src

Por favor, genera la siguiente estructura de carpetas dentro de la carpeta `learn-hub-back` para organizar el proyecto de manera eficiente. Esta estructura incluye carpetas para el nivel de API, Application, Domain, Host, Infrastructure y Tests.

## Estructura de carpetas

Dentro de la carpeta `learn-hub-back`, crea las siguientes subcarpetas:

```
src/
├── Api/             	# Proyecto con la lógica del API
├── Application/        # Proyecto para los handlers
├── Domain/            	# Proyecto para las entidades de dominio
├── Host/          		# Proyecto que contiene el archivo host de la aplicación
├── Infrastructure/     # Proyecto para la infraestructura
├── Tests/          	# Proyecto para test
```

## Descripción de cada carpeta

1. **Api/**: Aquí se deben colocar los controladores que se usarán en toda la aplicación, además de todos los archivos de configuración referentes a estos controladores. También estarán los middleware que tuviera la aplicación o los archivos referentes a la autorización del API.

2. **Application/**: En esta carpeta se colocan los handlers y request de la aplicación, así como los archivos de respuesta y DTOs de entrada de los controladores y sus validaciones.

3. **Domain/**: Esta carpeta contiene todas las entidades de base de datos que tendremos en la aplicación. También contendrá los enumerados o constantes de la aplicación.

4. **Host/**: Aquí irá el propio proyecto de Host con la clase inicial 'Program' y todos los archivos de configuración de la aplicación. También estarán los archivos appsettings con la configuración de la aplicación.

5. **Infrastructure/**: En esta carpeta estarán las migraciones de la aplicación. También los posibles 'Commands', 'Queries' y servicios de acceso a la base de datos además de sus interfaces.

6. **Tests/**: Esta carpeta contendrá los archivos de tests, tanto unitarios como funcionales, de la aplicación.

## Cómo generar las carpetas

Para crear esta estructura de carpetas, simplemente usa el siguiente comando en la terminal, si estás en el directorio raíz del proyecto:

```
mkdir -p src/Api src/Application src/Domain src/Host src/Infrastructure src/Tests
```

SOLO cuando ya hayas creado la estructura de carpetas anterior, quiero que crees un proyecto en las siguientes carpetas:

src/Api
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Api
```

src/Application
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Application
```

src/Domain
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Domain
```

src/Host
Usa el siguiente comando en la terminal:
```
dotnet new webapp -n LearnHub.Back.Host
dotnet sln add LearnHub.Back.Host
```

src/Infrastructure
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Infrastructure
```

src/Tests
Usa el siguiente comando en la terminal:
```
dotnet new nunit -n LearnHub.Back.Tests
dotnet sln add LearnHub.Back.Tests
dotnet add reference ../LearnHub.Back/LearnHub.Back.csproj
```