using LearnHub.Back.Api;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;

namespace LearnHub.Back.Tests.Integration
{
    public class PopularCoursesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public PopularCoursesIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Add an in-memory database for testing
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString());
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetPopularCourses_WithSampleData_ReturnsOrderedList()
        {
            // Arrange
            await SeedDatabaseAsync();

            // Act
            var response = await _client.GetAsync("/api/course/popular");

            // Assert
            response.EnsureSuccessStatusCode();
            var popularCourses = await response.Content.ReadFromJsonAsync<List<PopularCourseDto>>();

            Assert.NotNull(popularCourses);
            Assert.True(popularCourses.Count <= 10); // Should return max 10 courses
            
            // Verify they are ordered by enrollment count descending
            for (int i = 0; i < popularCourses.Count - 1; i++)
            {
                Assert.True(popularCourses[i].TotalEnrollments >= popularCourses[i + 1].TotalEnrollments);
            }
        }

        [Fact]
        public async Task GetPopularCourses_WithCustomCount_ReturnsCorrectNumber()
        {
            // Arrange
            await SeedDatabaseAsync();

            // Act
            var response = await _client.GetAsync("/api/course/popular?count=3");

            // Assert
            response.EnsureSuccessStatusCode();
            var popularCourses = await response.Content.ReadFromJsonAsync<List<PopularCourseDto>>();

            Assert.NotNull(popularCourses);
            Assert.Equal(3, popularCourses.Count);
        }

        [Fact]
        public async Task GetPopularCourses_IncludesCorrectStatistics()
        {
            // Arrange
            await SeedDatabaseAsync();

            // Act
            var response = await _client.GetAsync("/api/course/popular?count=1");

            // Assert
            response.EnsureSuccessStatusCode();
            var popularCourses = await response.Content.ReadFromJsonAsync<List<PopularCourseDto>>();

            Assert.NotNull(popularCourses);
            Assert.Single(popularCourses);

            var course = popularCourses.First();
            Assert.True(course.TotalEnrollments > 0);
            Assert.True(course.ActiveEnrollments <= course.TotalEnrollments);
            Assert.NotNull(course.Title);
            Assert.NotNull(course.Description);
            Assert.NotNull(course.InstructorName);
            Assert.True(course.Price > 0);
        }

        private async Task SeedDatabaseAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Clear existing data
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Create instructor
            var instructor = new Instructor
            {
                Id = Guid.NewGuid(),
                Name = "Test Instructor",
                Biography = "Experienced instructor"
            };
            context.Instructors.Add(instructor);

            // Create courses
            var courses = new List<Course>();
            for (int i = 1; i <= 5; i++)
            {
                courses.Add(new Course
                {
                    Id = Guid.NewGuid(),
                    Title = $"Course {i}",
                    Description = $"Description for course {i}",
                    StartDate = DateTime.Now.AddDays(30),
                    EndDate = DateTime.Now.AddDays(90),
                    Duration = 20 + i * 5,
                    Price = 100m * i,
                    Prerequisites = "None",
                    InstructorId = instructor.Id,
                    Modality = "Online",
                    IncludedMaterials = "Materials",
                    Certification = "Certificate",
                    AvailableSeats = 30,
                    Location = "Online",
                    Category = "Technology"
                });
            }
            context.Courses.AddRange(courses);

            // Create students
            var students = new List<Student>();
            for (int i = 1; i <= 15; i++)
            {
                students.Add(new Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = $"Student{i}",
                    LastName = $"Last{i}",
                    Email = $"student{i}@test.com",
                    Phone = $"123456{i:000}",
                    DateOfBirth = DateTime.Now.AddYears(-25),
                    EducationLevel = "Bachelor"
                });
            }
            context.Students.AddRange(students);

            await context.SaveChangesAsync();

            // Create enrollments with different counts per course
            var enrollments = new List<Enrollment>();
            int studentIndex = 0;

            // Course 1: 5 enrollments
            for (int i = 0; i < 5; i++)
            {
                enrollments.Add(new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[studentIndex++].Id,
                    CourseId = courses[0].Id,
                    EnrollmentDate = DateTime.Now.AddDays(-i),
                    Status = "Approved",
                    SchedulePreference = "Morning",
                    PaymentId = Guid.NewGuid()
                });
            }

            // Course 2: 4 enrollments
            for (int i = 0; i < 4; i++)
            {
                enrollments.Add(new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[studentIndex++].Id,
                    CourseId = courses[1].Id,
                    EnrollmentDate = DateTime.Now.AddDays(-i),
                    Status = "Approved",
                    SchedulePreference = "Afternoon",
                    PaymentId = Guid.NewGuid()
                });
            }

            // Course 3: 3 enrollments
            for (int i = 0; i < 3; i++)
            {
                enrollments.Add(new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[studentIndex++].Id,
                    CourseId = courses[2].Id,
                    EnrollmentDate = DateTime.Now.AddDays(-i),
                    Status = "Approved",
                    SchedulePreference = "Evening",
                    PaymentId = Guid.NewGuid()
                });
            }

            // Course 4: 2 enrollments
            for (int i = 0; i < 2; i++)
            {
                enrollments.Add(new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[studentIndex++].Id,
                    CourseId = courses[3].Id,
                    EnrollmentDate = DateTime.Now.AddDays(-i),
                    Status = "Approved",
                    SchedulePreference = "Morning",
                    PaymentId = Guid.NewGuid()
                });
            }

            // Course 5: 1 enrollment
            enrollments.Add(new Enrollment
            {
                Id = Guid.NewGuid(),
                StudentId = students[studentIndex].Id,
                CourseId = courses[4].Id,
                EnrollmentDate = DateTime.Now,
                Status = "Approved",
                SchedulePreference = "Afternoon",
                PaymentId = Guid.NewGuid()
            });

            context.Enrollments.AddRange(enrollments);
            await context.SaveChangesAsync();
        }
    }
}
