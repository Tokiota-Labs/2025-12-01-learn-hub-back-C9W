using System.ComponentModel.DataAnnotations;

namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Represents a course in the system
    /// </summary>
    public class CourseDto
    {
        /// <summary>
        /// Unique identifier of the course
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the course
        /// </summary>
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Detailed description of the course
        /// </summary>
        [Required(ErrorMessage = "Course description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        /// <summary>
        /// Start date of the course
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the course
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
    }
}