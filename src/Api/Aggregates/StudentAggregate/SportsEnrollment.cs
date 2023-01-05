using EFCoreEncapsulation.Api.Aggregates.SportsAggregate;

namespace EFCoreEncapsulation.Api.Aggregates.StudentAggregate;

public class SportsEnrollment
{
    public long Id { get; set; }
    public Grade Grade { get; set; }
    public long SportsId { get; set; }
    public Student Student { get; set; }
    public Sports Sports { get; set; }
}