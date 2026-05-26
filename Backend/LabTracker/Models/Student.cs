namespace LabTracker.Models;

public class Student
{
    public int Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;

    public ICollection<LabWork> LabWorks { get; set; } = new List<LabWork>();
}
