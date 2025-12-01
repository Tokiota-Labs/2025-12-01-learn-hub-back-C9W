namespace LearnHub.Back.Domain;

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string Status { get; set; } // Approved, Rejected, Pending
    public string SchedulePreference { get; set; }
    public Guid? PaymentId { get; set; }  // Making nullable since payment might not exist at enrollment time
    public Payment Payment { get; set; }
}