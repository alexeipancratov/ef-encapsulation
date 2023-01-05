﻿using EFCoreEncapsulation.Api.Aggregates.CourseAggregate;

namespace EFCoreEncapsulation.Api.Aggregates.StudentAggregate;

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