namespace LabTracker.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;

    public ICollection<LabWork> LabWorks { get; set; } = new List<LabWork>();
}
