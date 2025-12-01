# 🚀 LearnHub Backend - Copilot Instructions

## 📋 Resumen del Proyecto

Este es un proyecto backend en .NET 8 para una plataforma educativa llamada "LearnHub". Implementa una **arquitectura limpia** (Clean Architecture) con patrones **CQRS** usando **MediatR**, diseñado para gestionar cursos, estudiantes, instructores, inscripciones y pagos.

### 🏗️ Stack Tecnológico Principal
- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core** - ORM y acceso a datos
- **SQL Server** - Base de datos
- **MediatR** - Patrón Mediador/CQRS
- **FluentValidation** - Validación de datos
- **AutoMapper** - Mapeo de objetos
- **Swagger/OpenAPI** - Documentación de API

---

## 🎯 Principios y Patrones de Desarrollo

### ✅ Principios SOLID
Aplica estrictamente los principios SOLID en todo el código:

1. **Single Responsibility Principle (SRP)**: Cada clase debe tener una única razón para cambiar
2. **Open/Closed Principle (OCP)**: Abierto para extensión, cerrado para modificación
3. **Liskov Substitution Principle (LSP)**: Los objetos derivados deben ser sustituibles por sus bases
4. **Interface Segregation Principle (ISP)**: Interfaces específicas mejor que una interfaz general
5. **Dependency Inversion Principle (DIP)**: Depender de abstracciones, no de concreciones

### 🏛️ Arquitectura Limpia (Clean Architecture)

```
src/
├── Domain/              # Entidades de dominio, Value Objects, Interfaces
├── Application/         # Casos de uso, Commands/Queries, DTOs, Handlers
├── Infrastructure/      # Implementación de persistencia, servicios externos
├── Api/                # Controladores, DTOs de API, Middleware
└── Host/               # Punto de entrada, configuración, Program.cs
```

**Reglas de dependencias:**
- Domain no depende de nada
- Application solo depende de Domain
- Infrastructure depende de Application y Domain
- Api depende de Application y Domain
- Host orquesta todas las capas

### 📊 Patrón CQRS con MediatR

**Commands (Escritura):**
```csharp
// ✅ Comando bien estructurado
public class CreateCourseCommand : IRequest<CourseDto>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    // ... más propiedades
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseDto>
{
    // Implementación del handler
}
```

**Queries (Lectura):**
```csharp
// ✅ Query bien estructurada
public class GetCourseByIdQuery : IRequest<CourseDto>
{
    public Guid Id { get; set; }
}

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    // Implementación del handler
}
```

---

## 💎 Reglas de Clean Code

### 🔤 Nomenclatura y Convenciones

**C# Naming Conventions:**
```csharp
// ✅ Clases: PascalCase
public class StudentService { }

// ✅ Métodos: PascalCase
public async Task<Student> GetStudentAsync(Guid id) { }

// ✅ Propiedades: PascalCase
public string FullName { get; set; }

// ✅ Variables locales y parámetros: camelCase
var studentId = Guid.NewGuid();
public void ProcessStudent(Student student) { }

// ✅ Constantes: PascalCase
public const int MaxStudentsPerCourse = 30;

// ✅ Interfaces: IPascalCase
public interface IStudentRepository { }

// ✅ Archivos: Mismo nombre que la clase principal
// StudentService.cs contiene class StudentService
```

**Nombres Descriptivos:**
```csharp
// ❌ Evitar nombres genéricos
var data = GetData();
var list = new List<object>();

// ✅ Nombres específicos y descriptivos
var activeStudents = GetActiveStudents();
var enrolledCourses = new List<Course>();
```

### 📏 Estructura y Formato

**Métodos pequeños y enfocados:**
```csharp
// ✅ Método con responsabilidad única
public async Task<bool> IsStudentEligibleForCourseAsync(Guid studentId, Guid courseId)
{
    var student = await _studentRepository.GetByIdAsync(studentId);
    var course = await _courseRepository.GetByIdAsync(courseId);
    
    return student.MeetsPrerequisites(course.Prerequisites) && 
           course.HasAvailableSeats();
}
```

**Evitar anidamiento excesivo:**
```csharp
// ❌ Demasiado anidamiento
public void ProcessEnrollment(Enrollment enrollment)
{
    if (enrollment != null)
    {
        if (enrollment.IsActive)
        {
            if (enrollment.Course.HasAvailableSeats())
            {
                // lógica
            }
        }
    }
}

// ✅ Guard clauses y early returns
public void ProcessEnrollment(Enrollment enrollment)
{
    if (enrollment == null) return;
    if (!enrollment.IsActive) return;
    if (!enrollment.Course.HasAvailableSeats()) return;
    
    // lógica principal
}
```

### 🧼 Principios de Código Limpio

**DRY (Don't Repeat Yourself):**
```csharp
// ❌ Código duplicado
public void ValidateStudent(Student student)
{
    if (string.IsNullOrEmpty(student.Email) || !student.Email.Contains("@"))
        throw new ArgumentException("Invalid email");
}

public void ValidateInstructor(Instructor instructor)
{
    if (string.IsNullOrEmpty(instructor.Email) || !instructor.Email.Contains("@"))
        throw new ArgumentException("Invalid email");
}

// ✅ Extraer lógica común
public static class EmailValidator
{
    public static bool IsValid(string email)
    {
        return !string.IsNullOrEmpty(email) && email.Contains("@");
    }
}
```

**KISS (Keep It Simple, Stupid):**
```csharp
// ❌ Complejidad innecesaria
public bool IsEligible(Student student, Course course)
{
    return student.EducationLevel switch
    {
        "Beginner" => course.Prerequisites.Count == 0,
        "Intermediate" => course.Prerequisites.Count <= 2,
        "Advanced" => true,
        _ => false
    } && course.AvailableSeats > 0;
}

// ✅ Lógica clara y simple
public bool IsEligible(Student student, Course course)
{
    if (!course.HasAvailableSeats()) return false;
    
    return student.MeetsPrerequisites(course.Prerequisites);
}
```

---

## 🔧 Patrones de Implementación

### 🗃️ Repository Pattern
```csharp
// ✅ Interfaz del repositorio en Domain
public interface IStudentRepository
{
    Task<Student> GetByIdAsync(Guid id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student> AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

// ✅ Implementación en Infrastructure
public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;
    
    public StudentRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Student> GetByIdAsync(Guid id)
    {
        return await _context.Students
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    // ... más implementaciones
}
```

### 🎭 Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    IStudentRepository Students { get; }
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

### 🏭 Factory Pattern para entidades complejas
```csharp
public static class EnrollmentFactory
{
    public static Enrollment CreateEnrollment(
        Student student, 
        Course course, 
        string schedulePreference)
    {
        if (!course.HasAvailableSeats())
            throw new InvalidOperationException("Course is full");
        
        return new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course.Id,
            EnrollmentDate = DateTime.UtcNow,
            Status = "Active",
            SchedulePreference = schedulePreference
        };
    }
}
```

---

## ✅ Validación y Manejo de Errores

### 🔍 FluentValidation
```csharp
// ✅ Validador específico para cada comando
public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
            
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters");
            
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThan(10000).WithMessage("Price must be less than 10,000");
            
        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.Today).WithMessage("Start date must be in the future");
            
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
    }
}
```

### 🚨 Manejo de Excepciones Personalizado
```csharp
// ✅ Excepciones específicas del dominio
public class CourseNotFoundException : Exception
{
    public CourseNotFoundException(Guid courseId) 
        : base($"Course with ID {courseId} was not found") { }
}

public class EnrollmentCapacityExceededException : Exception
{
    public EnrollmentCapacityExceededException(string courseName) 
        : base($"Course '{courseName}' has reached maximum capacity") { }
}

// ✅ Middleware para manejo global de errores
public class ErrorHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            CourseNotFoundException => new { StatusCode = 404, error = exception.Message },
            ValidationException => new { StatusCode = 400, error = exception.Message },
            _ => new { StatusCode = 500, error = "An internal server error occurred" }
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
```

---

## 🌐 Controladores y API

### 📋 Estructura de Controladores
```csharp
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CoursesController> _logger;
    
    public CoursesController(IMediator mediator, ILogger<CoursesController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// Retrieves a course by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the course</param>
    /// <returns>Course details</returns>
    /// <response code="200">Course found successfully</response>
    /// <response code="404">Course not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
    {
        _logger.LogInformation("Getting course with ID: {CourseId}", id);
        
        var query = new GetCourseByIdQuery { Id = id };
        var course = await _mediator.Send(query);
        
        return course == null ? NotFound() : Ok(course);
    }
    
    /// <summary>
    /// Creates a new course
    /// </summary>
    /// <param name="command">Course creation details</param>
    /// <returns>Created course</returns>
    /// <response code="201">Course created successfully</response>
    /// <response code="400">Invalid course data</response>
    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseCommand command)
    {
        _logger.LogInformation("Creating new course: {CourseTitle}", command.Title);
        
        var course = await _mediator.Send(command);
        
        return CreatedAtAction(
            nameof(GetCourse), 
            new { id = course.Id }, 
            course);
    }
}
```

### 📄 DTOs y Mapeo
```csharp
// ✅ DTO específico para transferencia de datos
public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Duration { get; set; }
    public string Category { get; set; }
    public string Modality { get; set; }
    public InstructorDto Instructor { get; set; }
    public int AvailableSeats { get; set; }
}

// ✅ AutoMapper Profile
public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor));
            
        CreateMap<CreateCourseCommand, Course>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
```

---

## 🗄️ Acceso a Datos y Entity Framework

### 🏗️ Configuración de DbContext
```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Payment> Payments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // ✅ Configuración fluida de entidades
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // ✅ Configuración para logging de EF
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}
```

### ⚙️ Entity Configurations
```csharp
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(e => e.Description)
            .IsRequired();
            
        builder.Property(e => e.Price)
            .HasPrecision(18, 2);
            
        // ✅ Relaciones bien definidas
        builder.HasOne(e => e.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // ✅ Conversión de tipos complejos
        builder.Property(e => e.Schedule)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            );
    }
}
```

---

## 🧪 Testing y Calidad

### 🔬 Estructura de Tests
```
tests/
├── LearnHub.Back.UnitTests/          # Tests unitarios
│   ├── Domain/                       # Tests de lógica de dominio
│   ├── Application/                  # Tests de handlers y casos de uso
│   └── Infrastructure/               # Tests de repositorios
├── LearnHub.Back.IntegrationTests/   # Tests de integración
│   ├── Controllers/                  # Tests de API endpoints
│   └── Database/                     # Tests de acceso a datos
└── LearnHub.Back.ArchitectureTests/  # Tests de arquitectura
```

### ✅ Tests Unitarios
```csharp
[TestFixture]
public class CreateCourseCommandHandlerTests
{
    private Mock<ICourseRepository> _courseRepositoryMock;
    private Mock<IInstructorRepository> _instructorRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private CreateCourseCommandHandler _handler;
    
    [SetUp]
    public void Setup()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _instructorRepositoryMock = new Mock<IInstructorRepository>();
        _mapperMock = new Mock<IMapper>();
        
        _handler = new CreateCourseCommandHandler(
            _courseRepositoryMock.Object,
            _instructorRepositoryMock.Object,
            _mapperMock.Object);
    }
    
    [Test]
    public async Task Handle_ValidCommand_ShouldCreateCourse()
    {
        // Arrange
        var command = new CreateCourseCommand
        {
            Title = "Test Course",
            Description = "Test Description",
            Price = 100m,
            InstructorId = Guid.NewGuid()
        };
        
        var instructor = new Instructor { Id = command.InstructorId };
        var course = new Course { Id = Guid.NewGuid() };
        var courseDto = new CourseDto { Id = course.Id };
        
        _instructorRepositoryMock
            .Setup(x => x.GetByIdAsync(command.InstructorId))
            .ReturnsAsync(instructor);
            
        _mapperMock
            .Setup(x => x.Map<Course>(command))
            .Returns(course);
            
        _courseRepositoryMock
            .Setup(x => x.AddAsync(course))
            .ReturnsAsync(course);
            
        _mapperMock
            .Setup(x => x.Map<CourseDto>(course))
            .Returns(courseDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(courseDto.Id));
        
        _courseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Course>()), Times.Once);
    }
}
```

### 🏗️ Tests de Integración
```csharp
[TestFixture]
public class CoursesControllerIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task GetCourse_ExistingId_ShouldReturnCourse()
    {
        // Arrange
        var course = await SeedCourseAsync();
        
        // Act
        var response = await Client.GetAsync($"/api/courses/{course.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var courseDto = await response.Content.ReadFromJsonAsync<CourseDto>();
        courseDto.Should().NotBeNull();
        courseDto.Id.Should().Be(course.Id);
    }
}
```

---

## 📊 Logging y Monitoreo

### 📝 Structured Logging
```csharp
public class CourseService
{
    private readonly ILogger<CourseService> _logger;
    
    public async Task<CourseDto> CreateCourseAsync(CreateCourseCommand command)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["OperationId"] = Guid.NewGuid(),
            ["CourseTitle"] = command.Title
        });
        
        _logger.LogInformation(
            "Starting course creation for {CourseTitle} by instructor {InstructorId}",
            command.Title,
            command.InstructorId);
        
        try
        {
            var course = await ProcessCourseCreation(command);
            
            _logger.LogInformation(
                "Course created successfully with ID {CourseId}",
                course.Id);
                
            return course;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create course {CourseTitle}: {ErrorMessage}",
                command.Title,
                ex.Message);
            throw;
        }
    }
}
```

---

## 🔐 Seguridad y Configuración

### 🛡️ Configuración de Seguridad
```csharp
// Program.cs - Configuración de seguridad
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://learnhub.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ✅ Configuración de Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ApiPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });
});
```

### 🔧 Configuration Management
```csharp
// ✅ Strongly typed configuration
public class DatabaseSettings
{
    public const string SectionName = "Database";
    
    public string ConnectionString { get; set; }
    public int CommandTimeout { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
}

// Program.cs
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(DatabaseSettings.SectionName));
```

---

## 📚 Documentación y Swagger

### 📖 Documentación XML
```csharp
/// <summary>
/// Manages course-related operations including creation, retrieval, and updates
/// </summary>
/// <remarks>
/// This controller provides endpoints for:
/// - Creating new courses
/// - Retrieving course information
/// - Updating existing courses
/// - Managing course enrollments
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    /// <summary>
    /// Creates a new course in the system
    /// </summary>
    /// <param name="command">The course creation data</param>
    /// <returns>The created course with generated ID</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/courses
    ///     {
    ///         "title": "Advanced C# Programming",
    ///         "description": "Deep dive into advanced C# concepts",
    ///         "price": 299.99,
    ///         "startDate": "2024-01-15T09:00:00Z",
    ///         "endDate": "2024-03-15T17:00:00Z",
    ///         "instructorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// 
    /// </remarks>
    /// <response code="201">Course created successfully</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="404">Instructor not found</response>
    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseCommand command)
    {
        // Implementation
    }
}
```

---

## 🎨 Reglas de Estilo Específicas

### 🔍 Análisis Estático de Código
Configura EditorConfig y reglas de análisis:

```ini
# .editorconfig
root = true

[*.cs]
indent_style = space
indent_size = 4
end_of_line = crlf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

# Reglas de nomenclatura
dotnet_naming_rule.interfaces_should_be_prefixed_with_i.severity = error
dotnet_naming_rule.types_should_be_pascal_case.severity = error
dotnet_naming_rule.non_field_members_should_be_pascal_case.severity = error

# Reglas de formato
dotnet_sort_system_directives_first = true
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true
```

### 📐 Organización de Using Statements
```csharp
// ✅ Orden correcto de using statements
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using LearnHub.Back.Domain;
using LearnHub.Back.Application.Commands;
using LearnHub.Back.Application.DTOs;
```

### 🏷️ Comentarios y Documentación
```csharp
// ✅ Comentarios XML para APIs públicas
/// <summary>
/// Represents a student enrollment in a course
/// </summary>
public class Enrollment
{
    /// <summary>
    /// Gets or sets the unique identifier for the enrollment
    /// </summary>
    public Guid Id { get; set; }
    
    // ✅ Comentarios de código solo cuando sea necesario
    // This complex calculation handles edge cases for partial refunds
    var refundAmount = CalculateProRatedRefund(enrollment.EnrollmentDate, cancellationDate);
}
```

---

## ⚡ Performance y Optimización

### 🚀 Async/Await Best Practices
```csharp
// ✅ Uso correcto de async/await
public async Task<List<CourseDto>> GetPopularCoursesAsync(int count = 10)
{
    var courses = await _courseRepository
        .GetAllAsync()
        .ConfigureAwait(false);
    
    return courses
        .OrderByDescending(c => c.EnrollmentCount)
        .Take(count)
        .Select(c => _mapper.Map<CourseDto>(c))
        .ToList();
}

// ✅ Batch operations cuando sea posible
public async Task<bool> EnrollStudentsInBulkAsync(Guid courseId, List<Guid> studentIds)
{
    var enrollments = studentIds.Select(studentId => new Enrollment
    {
        CourseId = courseId,
        StudentId = studentId,
        EnrollmentDate = DateTime.UtcNow
    }).ToList();
    
    await _enrollmentRepository.AddRangeAsync(enrollments);
    return await _unitOfWork.SaveChangesAsync() > 0;
}
```

### 💾 Entity Framework Optimizations
```csharp
// ✅ Uso de Include para eager loading
public async Task<Course> GetCourseWithDetailsAsync(Guid id)
{
    return await _context.Courses
        .Include(c => c.Instructor)
        .Include(c => c.Enrollments)
            .ThenInclude(e => e.Student)
        .AsNoTracking() // Para consultas de solo lectura
        .FirstOrDefaultAsync(c => c.Id == id);
}

// ✅ Proyección para optimizar consultas
public async Task<List<CourseDto>> GetCoursesSummaryAsync()
{
    return await _context.Courses
        .Select(c => new CourseDto
        {
            Id = c.Id,
            Title = c.Title,
            Price = c.Price,
            InstructorName = c.Instructor.Name
        })
        .ToListAsync();
}
```

---

## 🚀 Deployment y DevOps

### 🐳 Docker Support
```dockerfile
# ✅ Multi-stage Dockerfile optimizado
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Host/LearnHub.Back.Host.csproj", "src/Host/"]
COPY ["src/Api/LearnHub.Back.Api.csproj", "src/Api/"]
COPY ["src/Application/LearnHub.Back.Application.csproj", "src/Application/"]
COPY ["src/Domain/LearnHub.Back.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/LearnHub.Back.Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/Host/LearnHub.Back.Host.csproj"
COPY . .
WORKDIR "/src/src/Host"
RUN dotnet build "LearnHub.Back.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearnHub.Back.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearnHub.Back.Host.dll"]
```

### 🔄 Health Checks
```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddDbContext<ApplicationDbContext>()
    .AddSqlServer(connectionString)
    .AddCheck<CourseServiceHealthCheck>("course-service");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

---

## 🎯 Reglas Finales

### ✅ DO's (Hacer)
- Usar async/await para operaciones I/O
- Implementar logging estructurado
- Validar todos los inputs
- Usar DTOs para transferencia de datos
- Implementar tests unitarios y de integración
- Seguir convenciones de nomenclatura de C#
- Usar dependency injection
- Implementar manejo de errores global
- Documentar APIs con XML comments
- Usar Entity Framework Core best practices

### ❌ DON'Ts (No hacer)
- No usar código síncrono para operaciones de base de datos
- No exponer entidades de dominio directamente en APIs
- No ignorar las excepciones
- No hardcodear strings de conexión
- No usar `var` cuando el tipo no sea obvio
- No crear métodos con más de 20 líneas
- No usar magic numbers o strings
- No mezclar responsabilidades en una clase
- No olvidar disposal de recursos
- No usar `async void` excepto en event handlers

---

## 📋 Checklist de Pull Request

Antes de crear un PR, verificar:

- [ ] ✅ Código sigue las convenciones de nomenclatura
- [ ] ✅ Tests unitarios escritos y pasando
- [ ] ✅ Documentación XML actualizada
- [ ] ✅ Validaciones implementadas con FluentValidation  
- [ ] ✅ Logging apropiado agregado
- [ ] ✅ Manejo de errores implementado
- [ ] ✅ DTOs usados para transferencia de datos
- [ ] ✅ Repository pattern seguido
- [ ] ✅ SOLID principles aplicados
- [ ] ✅ Async/await usado correctamente
- [ ] ✅ No hay código duplicado (DRY)
- [ ] ✅ Migrations de EF Core creadas si es necesario

---

*Este documento debe ser actualizado según evolucione el proyecto. Siempre prioriza la claridad, mantenibilidad y performance del código.*