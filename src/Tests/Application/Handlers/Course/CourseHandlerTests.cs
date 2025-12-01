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
                cfg.AddProfile<LearnHub.Back.Application.Mappings.CourseProfile>();
                cfg.AddProfile<LearnHub.Back.Application.Mappings.EnrollmentProfile>();
                cfg.AddProfile<LearnHub.Back.Application.Mappings.StudentProfile>();
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
        public async Task GetTop10MostSuccessfulCourses_ShouldReturnCoursesOrderedByApprovedEnrollments()
        {
            // Arrange
            var instructor = new Domain.Instructor { Name = "John Doe", Biography = "Expert instructor" };
            await _context.Instructors.AddAsync(instructor);
            await _context.SaveChangesAsync();

            // Create courses with different numbers of approved enrollments
            var course1 = new Domain.Course 
            { 
                Title = "Course 1", 
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Duration = 5,
                Price = 100m,
                Prerequisites = "None",
                InstructorId = instructor.Id,
                Modality = "Online",
                IncludedMaterials = "Materials",
                Certification = "Certificate",
                AvailableSeats = 20,
                Location = "Online",
                Category = "Technology"
            };
            var course2 = new Domain.Course 
            { 
                Title = "Course 2", 
                Description = "Description 2",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Duration = 5,
                Price = 150m,
                Prerequisites = "None",
                InstructorId = instructor.Id,
                Modality = "Online",
                IncludedMaterials = "Materials",
                Certification = "Certificate",
                AvailableSeats = 20,
                Location = "Online",
                Category = "Technology"
            };

            await _context.Courses.AddRangeAsync(course1, course2);
            await _context.SaveChangesAsync();

            // Create students
            var student1 = new Domain.Student { FullName = "Student 1", Email = "student1@test.com", PhoneNumber = "123", PostalAddress = "Address", CurrentOccupation = "Student", EducationLevel = "High School", PreviousExperience = "None" };
            var student2 = new Domain.Student { FullName = "Student 2", Email = "student2@test.com", PhoneNumber = "124", PostalAddress = "Address", CurrentOccupation = "Student", EducationLevel = "High School", PreviousExperience = "None" };
            var student3 = new Domain.Student { FullName = "Student 3", Email = "student3@test.com", PhoneNumber = "125", PostalAddress = "Address", CurrentOccupation = "Student", EducationLevel = "High School", PreviousExperience = "None" };
            
            await _context.Students.AddRangeAsync(student1, student2, student3);
            await _context.SaveChangesAsync();

            // Add enrollments: Course2 should have more approved enrollments than Course1
            var enrollments = new[]
            {
                new Domain.Enrollment { StudentId = student1.Id, CourseId = course1.Id, Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() },
                new Domain.Enrollment { StudentId = student2.Id, CourseId = course2.Id, Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() },
                new Domain.Enrollment { StudentId = student3.Id, CourseId = course2.Id, Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() },
                new Domain.Enrollment { StudentId = student1.Id, CourseId = course2.Id, Status = "Pending", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() }, // Should not count
            };

            await _context.Enrollments.AddRangeAsync(enrollments);
            await _context.SaveChangesAsync();

            var handler = new GetTop10MostSuccessfulCoursesQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(new GetTop10MostSuccessfulCoursesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("Course 2"); // Should be first because it has 2 approved enrollments
            result[1].Title.Should().Be("Course 1"); // Should be second because it has 1 approved enrollment
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}