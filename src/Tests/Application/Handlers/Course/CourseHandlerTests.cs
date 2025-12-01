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
        public async Task GetCoursesWithLeastDemand_ShouldReturnCoursesOrderedByEnrollmentCount()
        {
            // Arrange
            var instructor = new Domain.Instructor
            {
                Name = "Test Instructor",
                Biography = "Test Biography"
            };
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            var course1 = new Domain.Course
            {
                Title = "Course with 2 enrollments",
                Description = "Description 1",
                Price = 50m,
                Duration = 4,
                Category = "Category",
                Certification = "Certification",
                IncludedMaterials = "Materials",
                Location = "Location",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            var course2 = new Domain.Course
            {
                Title = "Course with 0 enrollments",
                Description = "Description 2",
                Price = 75m,
                Duration = 3,
                Category = "Category",
                Certification = "Certification",
                IncludedMaterials = "Materials",
                Location = "Location",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            var course3 = new Domain.Course
            {
                Title = "Course with 1 enrollment",
                Description = "Description 3",
                Price = 100m,
                Duration = 5,
                Category = "Category",
                Certification = "Certification",
                IncludedMaterials = "Materials",
                Location = "Location",
                Modality = "Online",
                Prerequisites = "None",
                InstructorId = instructor.Id
            };

            _context.Courses.AddRange(course1, course2, course3);
            await _context.SaveChangesAsync();

            // Add enrollments
            var student1 = new Domain.Student 
            { 
                FullName = "Student 1", 
                Email = "s1@test.com",
                PhoneNumber = "123456789",
                PostalAddress = "Address 1",
                EducationLevel = "Bachelor",
                CurrentOccupation = "Developer",
                PreviousExperience = "5 years"
            };
            var student2 = new Domain.Student 
            { 
                FullName = "Student 2", 
                Email = "s2@test.com",
                PhoneNumber = "987654321",
                PostalAddress = "Address 2",
                EducationLevel = "Master",
                CurrentOccupation = "Manager",
                PreviousExperience = "10 years"
            };
            _context.Students.AddRange(student1, student2);
            await _context.SaveChangesAsync();

            var payment1 = new Domain.Payment { PaymentAmount = 50m, PaymentMethod = "Credit Card", CardNumber = "1234567890123456", CVV = "123" };
            var payment2 = new Domain.Payment { PaymentAmount = 50m, PaymentMethod = "Credit Card", CardNumber = "1234567890123457", CVV = "124" };
            var payment3 = new Domain.Payment { PaymentAmount = 100m, PaymentMethod = "Credit Card", CardNumber = "1234567890123458", CVV = "125" };
            _context.Payments.AddRange(payment1, payment2, payment3);
            await _context.SaveChangesAsync();

            // course1 has 2 enrollments
            _context.Enrollments.Add(new Domain.Enrollment { StudentId = student1.Id, CourseId = course1.Id, Status = "Approved", PaymentId = payment1.Id, SchedulePreference = "Morning" });
            _context.Enrollments.Add(new Domain.Enrollment { StudentId = student2.Id, CourseId = course1.Id, Status = "Approved", PaymentId = payment2.Id, SchedulePreference = "Morning" });
            // course3 has 1 enrollment
            _context.Enrollments.Add(new Domain.Enrollment { StudentId = student1.Id, CourseId = course3.Id, Status = "Approved", PaymentId = payment3.Id, SchedulePreference = "Morning" });
            // course2 has 0 enrollments
            await _context.SaveChangesAsync();

            var handler = new GetCoursesWithLeastDemandQueryHandler(_context, _mapper);
            var query = new GetCoursesWithLeastDemandQuery { Count = 10 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result[0].Title.Should().Be("Course with 0 enrollments"); // 0 enrollments
            result[1].Title.Should().Be("Course with 1 enrollment");  // 1 enrollment
            result[2].Title.Should().Be("Course with 2 enrollments"); // 2 enrollments
        }

        [Test]
        public async Task GetCoursesWithLeastDemand_WithCountParameter_ShouldLimitResults()
        {
            // Arrange
            var instructor = new Domain.Instructor
            {
                Name = "Test Instructor",
                Biography = "Test Biography"
            };
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            for (int i = 0; i < 5; i++)
            {
                var course = new Domain.Course
                {
                    Title = $"Course {i}",
                    Description = $"Description {i}",
                    Price = 50m,
                    Duration = 4,
                    Category = "Category",
                    Certification = "Certification",
                    IncludedMaterials = "Materials",
                    Location = "Location",
                    Modality = "Online",
                    Prerequisites = "None",
                    InstructorId = instructor.Id
                };
                _context.Courses.Add(course);
            }
            await _context.SaveChangesAsync();

            var handler = new GetCoursesWithLeastDemandQueryHandler(_context, _mapper);
            var query = new GetCoursesWithLeastDemandQuery { Count = 3 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}