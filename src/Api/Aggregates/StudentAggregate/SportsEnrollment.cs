namespace EFCoreEncapsulation.Api.Aggregates.StudentAggregate;

public class SportsEnrollment
{
    public long Id { get; set; }
    public Grade Grade { get; set; }
    // NOTE: We've replaced Sports with SportsId so that we don't load data outside of current aggregate root.
    public long SportsId { get; set; }
    public Student Student { get; set; }
}