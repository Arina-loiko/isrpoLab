namespace LabTracker.Models;

public class LabWork
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int LabNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Не сдана";
    public int? Grade { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public string Notes { get; set; } = string.Empty;

    public Student Student { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}
