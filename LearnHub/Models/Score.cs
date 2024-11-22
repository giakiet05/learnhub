using LearnHub.Models;

public class Score
{
    // Foreign keys to link to SubjectResult
    public string YearId { get; set; }
    public string SubjectId { get; set; }
    public string StudentId { get; set; }
    public string Semester { get; set; } // Part of composite key in SubjectResult

    public string Type { get; set; } // "Regular", "Midterm", "FinalTerm", etc.
    public double Value { get; set; }

    // Navigation property
    public SubjectResult SubjectResult { get; set; }
}
