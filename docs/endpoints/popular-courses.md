# Endpoint: Cursos Más Demandados

## Descripción
Este endpoint devuelve los cursos más demandados basado en el número de inscripciones (enrollments), ordenados de mayor a menor demanda.

## URL
```
GET /api/course/popular
```

## Parámetros de Consulta
| Parámetro | Tipo | Requerido | Valor por Defecto | Descripción |
|-----------|------|-----------|-------------------|-------------|
| `count` | integer | No | 10 | Número de cursos a retornar (máximo) |

## Ejemplos de Uso

### Obtener los 10 cursos más populares (por defecto)
```http
GET /api/course/popular
```

### Obtener los 5 cursos más populares
```http
GET /api/course/popular?count=5
```

## Respuesta Exitosa
**Código HTTP:** `200 OK`

**Estructura de la Respuesta:**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Advanced Programming",
    "description": "Learn advanced programming concepts and best practices",
    "price": 500.00,
    "category": "Technology",
    "duration": 40,
    "modality": "Online",
    "totalEnrollments": 25,
    "activeEnrollments": 23,
    "availableSeats": 15,
    "instructorName": "Dr. Smith",
    "startDate": "2025-07-15T00:00:00Z",
    "endDate": "2025-09-15T00:00:00Z"
  },
  {
    "id": "2fa85f64-5717-4562-b3fc-2c963f66afa7",
    "title": "Business Strategy Fundamentals",
    "description": "Master the fundamentals of business strategy",
    "price": 400.00,
    "category": "Business",
    "duration": 30,
    "modality": "Presencial",
    "totalEnrollments": 18,
    "activeEnrollments": 16,
    "availableSeats": 7,
    "instructorName": "Prof. Johnson",
    "startDate": "2025-07-20T00:00:00Z",
    "endDate": "2025-08-20T00:00:00Z"
  }
]
```

## Campos de Respuesta

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | string (UUID) | Identificador único del curso |
| `title` | string | Nombre del curso |
| `description` | string | Descripción detallada del curso |
| `price` | decimal | Precio del curso |
| `category` | string | Categoría del curso (ej: Technology, Business) |
| `duration` | integer | Duración en horas |
| `modality` | string | Modalidad (Online, Presencial, Híbrido) |
| `totalEnrollments` | integer | Número total de inscripciones |
| `activeEnrollments` | integer | Número de inscripciones aprobadas |
| `availableSeats` | integer | Asientos disponibles restantes |
| `instructorName` | string | Nombre del instructor |
| `startDate` | datetime | Fecha de inicio del curso |
| `endDate` | datetime | Fecha de finalización del curso |

## Lógica de Ordenamiento
Los cursos se ordenan por:
1. **Criterio principal:** `totalEnrollments` (descendente)
2. Los cursos con más inscripciones aparecen primero
3. Se incluyen inscripciones en todos los estados (Approved, Pending, Rejected)

## Casos de Uso
- **Dashboard administrativo:** Mostrar los cursos más populares en métricas
- **Página principal:** Destacar cursos con alta demanda para estudiantes
- **Análisis de negocio:** Identificar tendencias en la demanda de cursos
- **Marketing:** Promocionar cursos exitosos

## Consideraciones
- El endpoint es eficiente para consultas frecuentes
- Los datos se calculan en tiempo real desde la base de datos
- No requiere autenticación (datos públicos)
- Soporta paginación implícita mediante el parámetro `count`

## Ejemplos de Integración

### JavaScript/Fetch
```javascript
// Obtener los 5 cursos más populares
const response = await fetch('/api/course/popular?count=5');
const popularCourses = await response.json();

popularCourses.forEach(course => {
  console.log(`${course.title}: ${course.totalEnrollments} inscripciones`);
});
```

### C# HttpClient
```csharp
using var httpClient = new HttpClient();
var response = await httpClient.GetFromJsonAsync<List<PopularCourseDto>>(
    "https://api.learnhub.com/api/course/popular?count=10"
);

foreach (var course in response)
{
    Console.WriteLine($"{course.Title}: {course.TotalEnrollments} enrollments");
}
```

### cURL
```bash
# Obtener los 3 cursos más populares
curl -X GET "https://api.learnhub.com/api/course/popular?count=3" \
     -H "accept: application/json"
```
