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
                cfg.CreateMap<Domain.Course, CourseDto>()
                    .ForMember(dest => dest.Enrollments, opt => opt.Ignore()); // Ignore to avoid circular reference in test
                cfg.CreateMap<Domain.Enrollment, EnrollmentDto>()
                    .ForMember(dest => dest.Student, opt => opt.Ignore())
                    .ForMember(dest => dest.Course, opt => opt.Ignore());
                cfg.CreateMap<Domain.Student, StudentDto>();
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
        public async Task GetTopSuccessfulCourses_ShouldReturnCoursesOrderedByApprovedEnrollmentsDesc()
        {
            // Arrange
            var instructor = new Domain.Instructor
            {
                Id = Guid.NewGuid(),
                Name = "Test Instructor",
                Biography = "Test Bio"
            };
            _context.Instructors.Add(instructor);

            var course1 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Course 1",
                Description = "Description 1",
                Price = 100m,
                Duration = 5,
                Prerequisites = "None",
                Modality = "Online",
                IncludedMaterials = "Materials",
                Certification = "Cert",
                Location = "Online",
                Category = "Tech",
                InstructorId = instructor.Id
            };

            var course2 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Course 2",
                Description = "Description 2",
                Price = 200m,
                Duration = 10,
                Prerequisites = "Basic",
                Modality = "Hybrid",
                IncludedMaterials = "Materials 2",
                Certification = "Cert 2",
                Location = "Campus",
                Category = "Business",
                InstructorId = instructor.Id
            };

            var course3 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Course 3",
                Description = "Description 3",
                Price = 150m,
                Duration = 7,
                Prerequisites = "Intermediate",
                Modality = "Online",
                IncludedMaterials = "Materials 3",
                Certification = "Cert 3",
                Location = "Online",
                Category = "Design",
                InstructorId = instructor.Id
            };

            _context.Courses.AddRange(course1, course2, course3);

            // Course 1: 3 approved enrollments
            var enrollment1 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course1.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() };
            var enrollment2 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course1.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() };
            var enrollment3 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course1.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Evening", PaymentId = Guid.NewGuid() };

            // Course 2: 5 approved enrollments
            var enrollment4 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() };
            var enrollment5 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() };
            var enrollment6 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Evening", PaymentId = Guid.NewGuid() };
            var enrollment7 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() };
            var enrollment8 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course2.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() };

            // Course 3: 1 approved enrollment and 2 rejected (should not count rejected ones)
            var enrollment9 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course3.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() };
            var enrollment10 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course3.Id, StudentId = Guid.NewGuid(), Status = "Rejected", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() };
            var enrollment11 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = course3.Id, StudentId = Guid.NewGuid(), Status = "Rejected", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Evening", PaymentId = Guid.NewGuid() };

            _context.Enrollments.AddRange(enrollment1, enrollment2, enrollment3, enrollment4, enrollment5, enrollment6, enrollment7, enrollment8, enrollment9, enrollment10, enrollment11);
            await _context.SaveChangesAsync();

            var handler = new GetTopSuccessfulCoursesQueryHandler(_context, _mapper);
            var query = new GetTopSuccessfulCoursesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            
            // Should be ordered by approved enrollments count descending
            result[0].Title.Should().Be("Course 2"); // 5 approved enrollments
            result[1].Title.Should().Be("Course 1"); // 3 approved enrollments
            result[2].Title.Should().Be("Course 3"); // 1 approved enrollment
        }

        [Test]
        public async Task GetTopSuccessfulCourses_WithFewerThan10Courses_ShouldReturnOnlySuccessfulOnes()
        {
            // Arrange
            var instructor = new Domain.Instructor
            {
                Id = Guid.NewGuid(),
                Name = "Test Instructor",
                Biography = "Test Bio"
            };
            _context.Instructors.Add(instructor);

            // Create 2 courses - one successful, one unsuccessful
            var successfulCourse = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Successful Course",
                Description = "Description",
                Price = 100m,
                Duration = 5,
                Prerequisites = "None",
                Modality = "Online",
                IncludedMaterials = "Materials",
                Certification = "Cert",
                Location = "Online",
                Category = "Tech",
                InstructorId = instructor.Id
            };

            var unsuccessfulCourse = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Unsuccessful Course",
                Description = "Description",
                Price = 200m,
                Duration = 10,
                Prerequisites = "Basic",
                Modality = "Hybrid",
                IncludedMaterials = "Materials",
                Certification = "Cert",
                Location = "Campus",
                Category = "Business",
                InstructorId = instructor.Id
            };

            _context.Courses.AddRange(successfulCourse, unsuccessfulCourse);

            // Successful course: 2 approved enrollments
            var enrollment1 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = successfulCourse.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() };
            var enrollment2 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = successfulCourse.Id, StudentId = Guid.NewGuid(), Status = "Approved", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() };

            // Unsuccessful course: only rejected enrollments
            var enrollment3 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = unsuccessfulCourse.Id, StudentId = Guid.NewGuid(), Status = "Rejected", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Morning", PaymentId = Guid.NewGuid() };
            var enrollment4 = new Domain.Enrollment { Id = Guid.NewGuid(), CourseId = unsuccessfulCourse.Id, StudentId = Guid.NewGuid(), Status = "Pending", EnrollmentDate = DateTime.UtcNow, SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() };

            _context.Enrollments.AddRange(enrollment1, enrollment2, enrollment3, enrollment4);
            await _context.SaveChangesAsync();

            var handler = new GetTopSuccessfulCoursesQueryHandler(_context, _mapper);
            var query = new GetTopSuccessfulCoursesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1); // Only the successful course should be returned
            result[0].Title.Should().Be("Successful Course");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}