namespace LearnHub.Back.Application.DTOs
{
    /// <summary>
    /// Represents the most successful course with enrollment information
    /// </summary>
    public class MostSuccessfulCourseDto
    {
        /// <summary>
        /// Course details
        /// </summary>
        public CourseDto Course { get; set; }

        /// <summary>
        /// Number of people enrolled in this course
        /// </summary>
        public int EnrollmentCount { get; set; }
    }
}