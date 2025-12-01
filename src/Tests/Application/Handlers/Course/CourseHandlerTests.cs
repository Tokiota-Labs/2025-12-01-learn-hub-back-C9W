using AutoMapper;
using FluentAssertions;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Tests.Application.Handlers.Course
{
    [TestFixture]
    public class CourseHandlerTests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);
            _context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<CreateCourseCommand, Domain.Course>();
                cfg.CreateMap<UpdateCourseCommand, Domain.Course>();
                cfg.CreateMap<Domain.Course, CourseDto>();
            });
            
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task CreateCourse_WithValidData_ShouldCreateAndReturnDto()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Title = "Test Course",
                Description = "Test Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Duration = 5,
                Price = 99.99m,
                Prerequisites = "None",
                InstructorId = Guid.NewGuid(),
                Modality = "Online",
                IncludedMaterials = "None",
                Certification = "Certificate of Completion",
                AvailableSeats = 20,
                Location = "Online",
                Category = "Technology"
            };
            
            var handler = new CreateCourseCommandHandler(_mapper, _context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(command.Title);
            result.Description.Should().Be(command.Description);
            result.Price.Should().Be(command.Price);
            result.Duration.Should().Be(command.Duration);
        }

        [Test]
        public async Task UpdateCourse_WithValidData_ShouldUpdateAndReturnUnit()
        {
            // Arrange
            var course = new Domain.Course
            {
                Title = "Original Title",
                Description = "Original Description",
                Price = 50m,
                Duration = 4,
                Category = "Sample Category",
                Certification = "Sample Certification",
                IncludedMaterials = "Sample Materials",
                Location = "Sample Location",
                Modality = "Sample Modality",
                Prerequisites = "Sample Prerequisites",
                InstructorId = Guid.NewGuid() // Assuming you have a valid InstructorId
            };
            
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var command = new UpdateCourseCommand
            {
                Id = course.Id,
                Title = "Updated Title",
                Description = "Updated Description",
                Price = 75m,
                Duration = 2
            };

            var handler = new UpdateCourseCommandHandler(_mapper, _context);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedCourse = await _context.Courses.FindAsync(course.Id);
            updatedCourse.Should().NotBeNull();
            updatedCourse.Title.Should().Be(command.Title);
            updatedCourse.Description.Should().Be(command.Description);
            updatedCourse.Price.Should().Be(command.Price);
            updatedCourse.Duration.Should().Be(command.Duration);
        }

        [Test]
        public async Task DeleteCourse_WithExistingId_ShouldRemoveAndSaveChanges()
        {
            // Arrange
            var course = new Domain.Course
            {
                Title = "Test Course",
                Description = "Test Description",
                Price = 99.99m,
                Duration = 2,
                Prerequisites = "None",
                Modality = "Online",
                IncludedMaterials = "None",
                Certification = "None",
                Location = "Test Location",
                Category = "Test Category"
            };
            
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var handler = new DeleteCourseCommandHandler(_context);
            var command = new DeleteCourseCommand { Id = course.Id };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedCourse = await _context.Courses.FindAsync(course.Id);
            deletedCourse.Should().BeNull();
        }

        [Test]
        public async Task DeleteCourse_WithNonExistingId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var handler = new DeleteCourseCommandHandler(_context);
            var command = new DeleteCourseCommand { Id = Guid.NewGuid() };

            // Act & Assert
            await handler.Invoking(h => h.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [Test]
        public async Task GetTop10MostSuccessfulCoursesQueryHandler_ShouldReturnCoursesOrderedByApprovedEnrollments()
        {
            // Arrange
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<GetTop10MostSuccessfulCoursesQueryHandler>();
            var handler = new GetTop10MostSuccessfulCoursesQueryHandler(_context, _mapper, logger);
            
            // Create test data - Instructors
            var instructor1 = new Domain.Instructor 
            { 
                Id = Guid.NewGuid(), 
                Name = "John Doe", 
                Email = "john@test.com",
                Courses = new List<Domain.Course>()
            };
            var instructor2 = new Domain.Instructor 
            { 
                Id = Guid.NewGuid(), 
                Name = "Jane Smith", 
                Email = "jane@test.com",
                Courses = new List<Domain.Course>()
            };
            
            await _context.Instructors.AddRangeAsync(instructor1, instructor2);
            
            // Create test data - Students
            var students = new List<Domain.Student>();
            for (int i = 0; i < 15; i++)
            {
                students.Add(new Domain.Student 
                { 
                    Id = Guid.NewGuid(), 
                    Name = $"Student {i}", 
                    Email = $"student{i}@test.com",
                    Enrollments = new List<Domain.Enrollment>()
                });
            }
            await _context.Students.AddRangeAsync(students);
            
            // Create test data - Courses
            var course1 = new Domain.Course 
            { 
                Id = Guid.NewGuid(), 
                Title = "Most Popular Course",
                Description = "Description 1",
                InstructorId = instructor1.Id,
                Instructor = instructor1,
                Price = 100m,
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(40),
                Duration = 30,
                Enrollments = new List<Domain.Enrollment>()
            };
            
            var course2 = new Domain.Course 
            { 
                Id = Guid.NewGuid(), 
                Title = "Second Most Popular",
                Description = "Description 2",
                InstructorId = instructor2.Id,
                Instructor = instructor2,
                Price = 150m,
                StartDate = DateTime.Now.AddDays(15),
                EndDate = DateTime.Now.AddDays(45),
                Duration = 25,
                Enrollments = new List<Domain.Enrollment>()
            };
            
            var course3 = new Domain.Course 
            { 
                Id = Guid.NewGuid(), 
                Title = "Least Popular Course",
                Description = "Description 3",
                InstructorId = instructor1.Id,
                Instructor = instructor1,
                Price = 75m,
                StartDate = DateTime.Now.AddDays(20),
                EndDate = DateTime.Now.AddDays(50),
                Duration = 20,
                Enrollments = new List<Domain.Enrollment>()
            };
            
            await _context.Courses.AddRangeAsync(course1, course2, course3);
            await _context.SaveChangesAsync();
            
            // Create enrollments - Course 1: 5 approved, 2 pending
            for (int i = 0; i < 5; i++)
            {
                await _context.Enrollments.AddAsync(new Domain.Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[i].Id,
                    CourseId = course1.Id,
                    Status = "Approved",
                    EnrollmentDate = DateTime.Now.AddDays(-i)
                });
            }
            for (int i = 5; i < 7; i++)
            {
                await _context.Enrollments.AddAsync(new Domain.Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[i].Id,
                    CourseId = course1.Id,
                    Status = "Pending",
                    EnrollmentDate = DateTime.Now.AddDays(-i)
                });
            }
            
            // Create enrollments - Course 2: 3 approved, 1 rejected
            for (int i = 7; i < 10; i++)
            {
                await _context.Enrollments.AddAsync(new Domain.Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[i].Id,
                    CourseId = course2.Id,
                    Status = "Approved",
                    EnrollmentDate = DateTime.Now.AddDays(-i)
                });
            }
            await _context.Enrollments.AddAsync(new Domain.Enrollment
            {
                Id = Guid.NewGuid(),
                StudentId = students[10].Id,
                CourseId = course2.Id,
                Status = "Rejected",
                EnrollmentDate = DateTime.Now.AddDays(-10)
            });
            
            // Create enrollments - Course 3: 1 approved
            await _context.Enrollments.AddAsync(new Domain.Enrollment
            {
                Id = Guid.NewGuid(),
                StudentId = students[11].Id,
                CourseId = course3.Id,
                Status = "Approved",
                EnrollmentDate = DateTime.Now.AddDays(-11)
            });
            
            await _context.SaveChangesAsync();
            
            var query = new GetTop10MostSuccessfulCoursesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            
            // Should be ordered by approved enrollments count (descending)
            result[0].Title.Should().Be("Most Popular Course");      // 5 approved
            result[1].Title.Should().Be("Second Most Popular");     // 3 approved  
            result[2].Title.Should().Be("Least Popular Course");    // 1 approved
            
            // Verify instructor information is included
            result[0].Instructor.Should().NotBeNull();
            result[0].Instructor.Name.Should().Be("John Doe");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}