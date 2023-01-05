namespace EFCoreEncapsulation.Api;

public class Enrollment
{
    public long Id { get; set; }
    public Grade Grade { get; set; }
    public virtual Course Course { get; set; }
    public virtual Student Student { get; set; }
}

public enum Grade
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    F = 5
}

public class SportsEnrollment
{
    public long Id { get; set; }
    public Grade Grade { get; set; }
    public long SportsId { get; set; }
    public Student Student { get; set; }
    public ICollection<Sports> Sports { get; set; }
}

public class Sports
{
    public long Id { get; set; }
    public string Name { get; set; }
}