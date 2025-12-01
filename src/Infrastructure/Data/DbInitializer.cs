using LearnHub.Back.Domain;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.Migrate();

        if (!context.Instructors.Any())
        {
            var instructors = new[]
            {
                new Instructor
                {
                    Name = "John Doe",
                    Biography = "Experienced web development instructor with 10+ years in the field"
                },
                new Instructor
                {
                    Name = "Alice Smith",
                    Biography = "Machine Learning specialist with PhD in Computer Science"
                },
                new Instructor
                {
                    Name = "Bob Johnson",
                    Biography = "Mobile development expert with experience in iOS and Android"
                },
                new Instructor
                {
                    Name = "Maria Garcia",
                    Biography = "Cloud architecture specialist with AWS and Azure certifications"
                }
            };

            context.Instructors.AddRange(instructors);
            context.SaveChanges();
        }

        if (!context.Courses.Any())
        {
            var courses = new[]
            {
                new Course
                {
                    Title = "Introduction to .NET",
                    Description = "Learn the basics of .NET development",
                    StartDate = DateTime.UtcNow.AddDays(30),
                    EndDate = DateTime.UtcNow.AddDays(90),
                    Duration = 60,
                    Price = 99.99m,
                    Prerequisites = "Basic programming knowledge",
                    InstructorId = context.Instructors.First(i => i.Name == "John Doe").Id,
                    Modality = "Online",
                    IncludedMaterials = "Course materials, Sample code",
                    Certification = "Course completion certificate",
                    AvailableSeats = 20,
                    Location = "Online",
                    Category = "Web Development",
                    Schedule = new List<string> { "Monday 18:00-20:00", "Wednesday 18:00-20:00" }
                },
                new Course
                {
                    Title = "Machine Learning Fundamentals",
                    Description = "Introduction to ML concepts and practical applications",
                    StartDate = DateTime.UtcNow.AddDays(15),
                    EndDate = DateTime.UtcNow.AddDays(75),
                    Duration = 80,
                    Price = 149.99m,
                    Prerequisites = "Python basics, Statistics fundamentals",
                    InstructorId = context.Instructors.First(i => i.Name == "Alice Smith").Id,
                    Modality = "Hybrid",
                    IncludedMaterials = "Jupyter notebooks, Datasets, ML tools access",
                    Certification = "ML Foundation Certificate",
                    AvailableSeats = 15,
                    Location = "Online + Monthly Workshop",
                    Category = "Data Science",
                    Schedule = new List<string> { "Tuesday 17:00-20:00", "Friday 17:00-19:00" }
                },
                new Course
                {
                    Title = "Mobile App Development",
                    Description = "Build cross-platform mobile applications",
                    StartDate = DateTime.UtcNow.AddDays(45),
                    EndDate = DateTime.UtcNow.AddDays(105),
                    Duration = 70,
                    Price = 129.99m,
                    Prerequisites = "Basic JavaScript knowledge",
                    InstructorId = context.Instructors.First(i => i.Name == "Bob Johnson").Id,
                    Modality = "Online",
                    IncludedMaterials = "Development tools, App templates",
                    Certification = "Mobile Developer Certificate",
                    AvailableSeats = 25,
                    Location = "Online",
                    Category = "Mobile Development",
                    Schedule = new List<string> { "Thursday 18:00-21:00", "Saturday 10:00-12:00" }
                },
                new Course
                {
                    Title = "Cloud Architecture",
                    Description = "Master cloud services and architecture patterns",
                    StartDate = DateTime.UtcNow.AddDays(60),
                    EndDate = DateTime.UtcNow.AddDays(120),
                    Duration = 90,
                    Price = 199.99m,
                    Prerequisites = "Basic networking, Linux fundamentals",
                    InstructorId = context.Instructors.First(i => i.Name == "Maria Garcia").Id,
                    Modality = "Online",
                    IncludedMaterials = "Cloud credits, Architecture templates",
                    Certification = "Cloud Architecture Associate",
                    AvailableSeats = 18,
                    Location = "Online",
                    Category = "Cloud Computing",
                    Schedule = new List<string> { "Monday 19:00-22:00", "Wednesday 19:00-22:00" }
                }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();
        }

        if (!context.Students.Any())
        {
            var students = new[]
            {
                new Student
                {
                    FullName = "Emma Wilson",
                    Email = "emma.wilson@example.com",
                    PhoneNumber = "123-456-7890",
                    PostalAddress = "123 Main St, City",
                    EducationLevel = "Bachelor's Degree",
                    CurrentOccupation = "Software Developer",
                    PreviousExperience = "2 years in web development"
                },
                new Student
                {
                    FullName = "James Lee",
                    Email = "james.lee@example.com",
                    PhoneNumber = "234-567-8901",
                    PostalAddress = "456 Oak Ave, Town",
                    EducationLevel = "Master's Degree",
                    CurrentOccupation = "Data Analyst",
                    PreviousExperience = "3 years in data analysis"
                },
                new Student
                {
                    FullName = "Sofia Rodriguez",
                    Email = "sofia.rodriguez@example.com",
                    PhoneNumber = "345-678-9012",
                    PostalAddress = "789 Pine St, Village",
                    EducationLevel = "Bachelor's Degree",
                    CurrentOccupation = "Mobile Developer",
                    PreviousExperience = "1 year in mobile development"
                },
                new Student
                {
                    FullName = "Michael Chen",
                    Email = "michael.chen@example.com",
                    PhoneNumber = "456-789-0123",
                    PostalAddress = "321 Elm St, County",
                    EducationLevel = "PhD",
                    CurrentOccupation = "Research Scientist",
                    PreviousExperience = "5 years in machine learning research"
                },
                new Student
                {
                    FullName = "Laura Martinez",
                    Email = "laura.martinez@example.com",
                    PhoneNumber = "567-890-1234",
                    PostalAddress = "741 Cedar Rd, Borough",
                    EducationLevel = "Associate's Degree",
                    CurrentOccupation = "IT Support",
                    PreviousExperience = "2 years in technical support"
                },
                new Student
                {
                    FullName = "David Kim",
                    Email = "david.kim@example.com",
                    PhoneNumber = "678-901-2345",
                    PostalAddress = "852 Maple Dr, District",
                    EducationLevel = "Bachelor's Degree",
                    CurrentOccupation = "System Administrator",
                    PreviousExperience = "4 years in system administration"
                }
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            // Add enrollments with varied statuses and dates
            var enrollments = new[]
            {
                new Enrollment
                {
                    StudentId = students[0].Id,
                    CourseId = context.Courses.First(c => c.Title == "Introduction to .NET").Id,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-5),
                    Status = "Active",
                    SchedulePreference = "Monday 18:00-20:00"
                },
                new Enrollment
                {
                    StudentId = students[1].Id,
                    CourseId = context.Courses.First(c => c.Title == "Machine Learning Fundamentals").Id,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-3),
                    Status = "Active",
                    SchedulePreference = "Tuesday 17:00-20:00"
                },
                new Enrollment
                {
                    StudentId = students[2].Id,
                    CourseId = context.Courses.First(c => c.Title == "Mobile App Development").Id,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-2),
                    Status = "Active",
                    SchedulePreference = "Thursday 18:00-21:00"
                },
                new Enrollment
                {
                    StudentId = students[3].Id,
                    CourseId = context.Courses.First(c => c.Title == "Cloud Architecture").Id,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-10),
                    Status = "Completed",
                    SchedulePreference = "Monday 19:00-22:00"
                },
                new Enrollment
                {
                    StudentId = students[4].Id,
                    CourseId = context.Courses.First(c => c.Title == "Introduction to .NET").Id,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-1),
                    Status = "Pending",
                    SchedulePreference = "Monday 18:00-20:00"
                },
                new Enrollment
                {
                    StudentId = students[5].Id,
                    CourseId = context.Courses.First(c => c.Title == "Machine Learning Fundamentals").Id,
                    EnrollmentDate = DateTime.UtcNow.AddDays(-15),
                    Status = "Dropped",
                    SchedulePreference = "Tuesday 17:00-20:00"
                }
            };

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();

            // Add payments with different payment methods and dates
            var payments = enrollments.Select(e => new Payment
            {
                EnrollmentId = e.Id,
                PaymentAmount = context.Courses.First(c => c.Id == e.CourseId).Price,
                PaymentDate = e.EnrollmentDate,
                PaymentMethod = e.Status == "Dropped" ? "Refunded" : 
                               new[] { "Credit Card", "PayPal", "Bank Transfer" }[new Random().Next(3)],
                CardNumber = "************1234",
                CardExpirationDate = DateTime.UtcNow.AddYears(2),
                CVV = "***"
            }).ToArray();

            context.Payments.AddRange(payments);
            context.SaveChanges();
        }
    }
}