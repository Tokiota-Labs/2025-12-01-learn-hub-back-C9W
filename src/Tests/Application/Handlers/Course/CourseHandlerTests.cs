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
                cfg.CreateMap<Domain.Enrollment, EnrollmentDto>()
                    .ReverseMap()
                    .ForMember(dest => dest.Course, opt => opt.Ignore())
                    .ForMember(dest => dest.Student, opt => opt.Ignore())
                    .ForMember(dest => dest.Payment, opt => opt.Ignore());
                cfg.CreateMap<Domain.Student, StudentDto>()
                    .ReverseMap()
                    .ForMember(dest => dest.Enrollments, opt => opt.Ignore());
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
        public async Task GetLeastDemandCourses_ShouldReturnCoursesOrderedByEnrollmentCount()
        {
            // Arrange
            var instructor = new Domain.Instructor { Name = "Test Instructor", Biography = "Test Bio" };
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            var course1 = new Domain.Course
            {
                Title = "Course with 3 enrollments",
                Description = "Test Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Duration = 5,
                Price = 99.99m,
                Prerequisites = "None",
                InstructorId = instructor.Id,
                Modality = "Online",
                IncludedMaterials = "None",
                Certification = "Certificate",
                AvailableSeats = 20,
                Location = "Online",
                Category = "Technology"
            };

            var course2 = new Domain.Course
            {
                Title = "Course with 1 enrollment",
                Description = "Test Description 2",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Duration = 5,
                Price = 79.99m,
                Prerequisites = "None",
                InstructorId = instructor.Id,
                Modality = "Online",
                IncludedMaterials = "None",
                Certification = "Certificate",
                AvailableSeats = 15,
                Location = "Online",
                Category = "Technology"
            };

            var course3 = new Domain.Course
            {
                Title = "Course with 0 enrollments",
                Description = "Test Description 3",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Duration = 5,
                Price = 89.99m,
                Prerequisites = "None",
                InstructorId = instructor.Id,
                Modality = "Online",
                IncludedMaterials = "None",
                Certification = "Certificate",
                AvailableSeats = 25,
                Location = "Online",
                Category = "Technology"
            };

            _context.Courses.AddRange(course1, course2, course3);
            await _context.SaveChangesAsync();

            var student = new Domain.Student
            {
                FullName = "Test Student",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                PostalAddress = "Test Address",
                EducationLevel = "High School",
                CurrentOccupation = "Student",
                PreviousExperience = "None"
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Add enrollments to courses
            var enrollments = new List<Domain.Enrollment>
            {
                new Domain.Enrollment { StudentId = student.Id, CourseId = course1.Id, Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" },
                new Domain.Enrollment { StudentId = student.Id, CourseId = course1.Id, Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Afternoon" },
                new Domain.Enrollment { StudentId = student.Id, CourseId = course1.Id, Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Evening" },
                new Domain.Enrollment { StudentId = student.Id, CourseId = course2.Id, Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning" }
            };

            _context.Enrollments.AddRange(enrollments);
            await _context.SaveChangesAsync();

            var handler = new GetLeastDemandCoursesQueryHandler(_context, _mapper);
            var query = new GetLeastDemandCoursesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result[0].Title.Should().Be("Course with 0 enrollments");
            result[1].Title.Should().Be("Course with 1 enrollment");
            result[2].Title.Should().Be("Course with 3 enrollments");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}