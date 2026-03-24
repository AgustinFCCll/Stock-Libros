# Stock de Libros

Sistema de gestión de inventario de libros personal con backend ASP.NET Core y frontend React.

## Tecnologías

### Backend
- ASP.NET Core 10.0
- Entity Framework Core 10.0
- SQLite
- AutoMapper
- Swagger/OpenAPI

### Frontend
- React 19
- Vite
- ESLint

## Requisitos

- .NET 10.0 SDK
- Node.js 18+

## Instalación y Ejecución Local

### Opción rápida (Recomendada)

1. Clonar el repositorio
2. Ejecutar el archivo `Iniciar.bat`:
   ```
   Doble clic en Iniciar.bat
   ```
   
   Esto automáticamente:
   - Verifica que .NET y Node.js estén instalados
   - Cierra procesos anteriores en los puertos
   - Inicia el backend en `http://localhost:5052`
   - Inicia el frontend en `http://localhost:5173`

### Opción manual

1. Clonar el repositorio
2. Instalar dependencias del frontend:
   ```bash
   cd frontend
   npm install
   ```

#### Backend
```bash
dotnet run
```
El API estará disponible en `http://localhost:5052`

#### Frontend
```bash
cd frontend
npm run dev
```
La aplicación estará disponible en `http://localhost:5173`

## API Endpoints

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | /api/libros | Obtener todos los libros |
| GET | /api/libros/{id} | Obtener libro por ID |
| POST | /api/libros | Crear nuevo libro |
| PUT | /api/libros/{id} | Actualizar libro |
| DELETE | /api/libros/{id} | Eliminar libro |

## Modelo de Datos

```csharp
public class Libro
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Imagen { get; set; }
    public bool Prestado { get; set; }
    public bool Leido { get; set; }
}
```

## Documentación

Swagger UI disponible en `/swagger` cuando el backend está en ejecución.
