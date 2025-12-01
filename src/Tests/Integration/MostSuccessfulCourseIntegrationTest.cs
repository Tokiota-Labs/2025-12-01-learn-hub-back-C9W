using FluentAssertions;
using LearnHub.Back.Api.Controllers;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Moq;

namespace LearnHub.Back.Tests.Integration
{
    [TestFixture]
    public class MostSuccessfulCourseIntegrationTests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private CourseController _controller;
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
                cfg.CreateMap<Domain.Course, CourseDto>();
                cfg.CreateMap<Domain.Enrollment, EnrollmentDto>();
                cfg.CreateMap<Domain.Student, StudentDto>();
            });
            
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task FullIntegrationTest_MostSuccessfulCourseEndpoint_ShouldReturnCorrectResult()
        {
            // Arrange - Create a real-world scenario
            var instructorId = Guid.NewGuid();
            var instructor = new Domain.Instructor
            {
                Id = instructorId,
                Name = "John Smith",
                Biography = "Experienced software developer and instructor"
            };
            _context.Instructors.Add(instructor);

            // Create 3 courses
            var course1 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "C# Fundamentals",
                Description = "Learn the basics of C# programming",
                Price = 199.99m,
                Duration = 40,
                StartDate = DateTime.UtcNow.AddDays(7),
                EndDate = DateTime.UtcNow.AddDays(47),
                Prerequisites = "Basic programming knowledge",
                Modality = "Online",
                IncludedMaterials = "Video lectures, PDFs, code samples",
                Certification = "Certificate of Completion",
                AvailableSeats = 50,
                Location = "Online",
                Category = "Programming",
                InstructorId = instructorId
            };

            var course2 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Advanced JavaScript",
                Description = "Master advanced JavaScript concepts",
                Price = 299.99m,
                Duration = 60,
                StartDate = DateTime.UtcNow.AddDays(14),
                EndDate = DateTime.UtcNow.AddDays(74),
                Prerequisites = "Intermediate JavaScript",
                Modality = "Hybrid",
                IncludedMaterials = "Interactive exercises, live sessions",
                Certification = "Professional Certificate",
                AvailableSeats = 30,
                Location = "Madrid/Online",
                Category = "Web Development",
                InstructorId = instructorId
            };

            var course3 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Python for Data Science",
                Description = "Use Python for data analysis and machine learning",
                Price = 399.99m,
                Duration = 80,
                StartDate = DateTime.UtcNow.AddDays(21),
                EndDate = DateTime.UtcNow.AddDays(101),
                Prerequisites = "Basic Python knowledge",
                Modality = "Online",
                IncludedMaterials = "Jupyter notebooks, datasets, libraries",
                Certification = "Data Science Certificate",
                AvailableSeats = 25,
                Location = "Online",
                Category = "Data Science",
                InstructorId = instructorId
            };

            _context.Courses.AddRange(course1, course2, course3);
            await _context.SaveChangesAsync();

            // Create students
            var students = new List<Domain.Student>();
            for (int i = 1; i <= 7; i++)
            {
                students.Add(new Domain.Student
                {
                    Id = Guid.NewGuid(),
                    FullName = $"Student {i}",
                    Email = $"student{i}@example.com",
                    PhoneNumber = $"123456{i:D3}",
                    PostalAddress = $"Address {i}",
                    EducationLevel = "Bachelor",
                    CurrentOccupation = "Software Developer",
                    PreviousExperience = "Some programming experience"
                });
            }
            _context.Students.AddRange(students);
            await _context.SaveChangesAsync();

            // Create enrollments: course1 (2 enrollments), course2 (5 enrollments), course3 (1 enrollment)
            var enrollments = new List<Domain.Enrollment>
            {
                // Course 1 enrollments (2 students)
                new() { Id = Guid.NewGuid(), StudentId = students[0].Id, CourseId = course1.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Approved", 
                        SchedulePreference = "Morning", PaymentId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), StudentId = students[1].Id, CourseId = course1.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Approved", 
                        SchedulePreference = "Evening", PaymentId = Guid.NewGuid() },

                // Course 2 enrollments (5 students) - THIS SHOULD BE THE MOST SUCCESSFUL
                new() { Id = Guid.NewGuid(), StudentId = students[2].Id, CourseId = course2.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Approved", 
                        SchedulePreference = "Morning", PaymentId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), StudentId = students[3].Id, CourseId = course2.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Approved", 
                        SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), StudentId = students[4].Id, CourseId = course2.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Approved", 
                        SchedulePreference = "Evening", PaymentId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), StudentId = students[5].Id, CourseId = course2.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Pending", 
                        SchedulePreference = "Morning", PaymentId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), StudentId = students[6].Id, CourseId = course2.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Approved", 
                        SchedulePreference = "Afternoon", PaymentId = Guid.NewGuid() },

                // Course 3 enrollments (1 student)
                new() { Id = Guid.NewGuid(), StudentId = students[0].Id, CourseId = course3.Id, 
                        EnrollmentDate = DateTime.UtcNow, Status = "Approved", 
                        SchedulePreference = "Weekend", PaymentId = Guid.NewGuid() }
            };

            _context.Enrollments.AddRange(enrollments);
            await _context.SaveChangesAsync();

            // Create the controller with MediatR
            var handler = new GetMostSuccessfulCourseQueryHandler(_context, _mapper);
            var mediator = new Mock<MediatR.IMediator>();
            mediator.Setup(x => x.Send(It.IsAny<GetMostSuccessfulCourseQuery>(), It.IsAny<CancellationToken>()))
                    .Returns<GetMostSuccessfulCourseQuery, CancellationToken>((query, ct) => handler.Handle(query, ct));

            _controller = new CourseController(mediator.Object);

            // Act - Call the endpoint
            var result = await _controller.GetMostSuccessful();

            // Assert - Verify the response
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
            
            var okResult = (OkObjectResult)result.Result!;
            var response = (MostSuccessfulCourseDto)okResult.Value!;

            // Verify it's the correct course (course2 with 5 enrollments)
            response.Should().NotBeNull();
            response.Course.Id.Should().Be(course2.Id);
            response.Course.Title.Should().Be("Advanced JavaScript");
            response.Course.Description.Should().Be("Master advanced JavaScript concepts");
            response.Course.Price.Should().Be(299.99m);
            response.Course.Category.Should().Be("Web Development");
            response.EnrollmentCount.Should().Be(5);

            // Verify the instructor is included
            response.Course.Instructor.Should().NotBeNull();
            response.Course.Instructor.Name.Should().Be("John Smith");

            Console.WriteLine($"Most Successful Course: {response.Course.Title}");
            Console.WriteLine($"Number of Enrollments: {response.EnrollmentCount}");
            Console.WriteLine($"Course Price: ${response.Course.Price}");
            Console.WriteLine($"Course Category: {response.Course.Category}");
            Console.WriteLine($"Instructor: {response.Course.Instructor.Name}");
            
            // Additional verification - ensure the counts are correct in the database
            var courseEnrollmentCounts = await _context.Courses
                .Include(c => c.Enrollments)
                .Select(c => new { c.Title, EnrollmentCount = c.Enrollments.Count })
                .ToListAsync();

            foreach (var courseCount in courseEnrollmentCounts)
            {
                Console.WriteLine($"Course: {courseCount.Title}, Enrollments: {courseCount.EnrollmentCount}");
            }
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}