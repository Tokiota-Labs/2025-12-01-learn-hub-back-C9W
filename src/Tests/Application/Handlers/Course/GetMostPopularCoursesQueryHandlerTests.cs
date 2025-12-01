using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Domain;
using LearnHub.Back.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearnHub.Back.Tests.Application.Handlers.Course
{
    public class GetMostPopularCoursesQueryHandlerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Handle_ShouldReturnCoursesOrderedByEnrollmentCount()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            await SeedTestDataAsync(context);

            var handler = new GetMostPopularCoursesQueryHandler(context, null);
            var query = new GetMostPopularCoursesQuery { Count = 3 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.True(result[0].TotalEnrollments >= result[1].TotalEnrollments);
            Assert.True(result[1].TotalEnrollments >= result[2].TotalEnrollments);
        }

        [Fact]
        public async Task Handle_ShouldIncludeCorrectEnrollmentStatistics()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            await SeedTestDataAsync(context);

            var handler = new GetMostPopularCoursesQueryHandler(context, null);
            var query = new GetMostPopularCoursesQuery { Count = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var mostPopularCourse = result.First();
            Assert.True(mostPopularCourse.TotalEnrollments > 0);
            Assert.True(mostPopularCourse.ActiveEnrollments <= mostPopularCourse.TotalEnrollments);
        }

        private async Task SeedTestDataAsync(ApplicationDbContext context)
        {
            // Create instructors
            var instructor1 = new Instructor 
            { 
                Id = Guid.NewGuid(), 
                Name = "Dr. Smith", 
                Biography = "Expert in technology" 
            };
            
            var instructor2 = new Instructor 
            { 
                Id = Guid.NewGuid(), 
                Name = "Prof. Johnson", 
                Biography = "Business expert" 
            };

            context.Instructors.AddRange(instructor1, instructor2);

            // Create courses
            var course1 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Advanced Programming",
                Description = "Learn advanced programming concepts",
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(90),
                Duration = 40,
                Price = 500m,
                Prerequisites = "Basic programming",
                InstructorId = instructor1.Id,
                Modality = "Online",
                IncludedMaterials = "Books, Videos",
                Certification = "Certificate",
                AvailableSeats = 20,
                Location = "Online",
                Category = "Technology"
            };

            var course2 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Business Strategy",
                Description = "Learn business strategy fundamentals",
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(90),
                Duration = 30,
                Price = 400m,
                Prerequisites = "None",
                InstructorId = instructor2.Id,
                Modality = "Presencial",
                IncludedMaterials = "Books",
                Certification = "Certificate",
                AvailableSeats = 15,
                Location = "Aula 101",
                Category = "Business"
            };

            var course3 = new Domain.Course
            {
                Id = Guid.NewGuid(),
                Title = "Data Science Basics",
                Description = "Introduction to data science",
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(90),
                Duration = 35,
                Price = 450m,
                Prerequisites = "Math basics",
                InstructorId = instructor1.Id,
                Modality = "Híbrido",
                IncludedMaterials = "Software, Books",
                Certification = "Certificate",
                AvailableSeats = 25,
                Location = "Lab 201",
                Category = "Technology"
            };

            context.Courses.AddRange(course1, course2, course3);

            // Create students
            var students = new List<Student>();
            for (int i = 1; i <= 10; i++)
            {
                students.Add(new Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = $"Student{i}",
                    LastName = $"LastName{i}",
                    Email = $"student{i}@test.com",
                    Phone = $"123456{i:000}",
                    DateOfBirth = DateTime.Now.AddYears(-25),
                    EducationLevel = "Bachelor"
                });
            }
            context.Students.AddRange(students);

            // Create enrollments (course1 = 5 enrollments, course2 = 3 enrollments, course3 = 2 enrollments)
            var enrollments = new List<Enrollment>();
            
            // Course 1 - 5 enrollments (4 approved, 1 pending)
            for (int i = 0; i < 5; i++)
            {
                enrollments.Add(new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[i].Id,
                    CourseId = course1.Id,
                    EnrollmentDate = DateTime.Now.AddDays(-i),
                    Status = i < 4 ? "Approved" : "Pending",
                    SchedulePreference = "Morning",
                    PaymentId = Guid.NewGuid()
                });
            }

            // Course 2 - 3 enrollments (all approved)
            for (int i = 0; i < 3; i++)
            {
                enrollments.Add(new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[i + 5].Id,
                    CourseId = course2.Id,
                    EnrollmentDate = DateTime.Now.AddDays(-i),
                    Status = "Approved",
                    SchedulePreference = "Afternoon",
                    PaymentId = Guid.NewGuid()
                });
            }

            // Course 3 - 2 enrollments (all approved)
            for (int i = 0; i < 2; i++)
            {
                enrollments.Add(new Enrollment
                {
                    Id = Guid.NewGuid(),
                    StudentId = students[i + 8].Id,
                    CourseId = course3.Id,
                    EnrollmentDate = DateTime.Now.AddDays(-i),
                    Status = "Approved",
                    SchedulePreference = "Evening",
                    PaymentId = Guid.NewGuid()
                });
            }

            context.Enrollments.AddRange(enrollments);
            await context.SaveChangesAsync();
        }
    }
}
