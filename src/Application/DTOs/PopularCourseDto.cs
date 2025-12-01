using System.ComponentModel.DataAnnotations;

namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Represents a popular course with enrollment statistics
    /// </summary>
    public class PopularCourseDto
    {
        /// <summary>
        /// Unique identifier of the course
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the course
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Detailed description of the course
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Course price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Course category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Duration in hours
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Course modality (Online, Presencial, Híbrido)
        /// </summary>
        public string Modality { get; set; }

        /// <summary>
        /// Total number of enrollments for this course
        /// </summary>
        public int TotalEnrollments { get; set; }

        /// <summary>
        /// Number of active enrollments (Approved status)
        /// </summary>
        public int ActiveEnrollments { get; set; }

        /// <summary>
        /// Available seats remaining
        /// </summary>
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Instructor name
        /// </summary>
        public string InstructorName { get; set; }

        /// <summary>
        /// Course start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Course end date
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
