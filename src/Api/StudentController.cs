using EFCoreEncapsulation.Api.Aggregates.CourseAggregate;
using EFCoreEncapsulation.Api.Aggregates.StudentAggregate;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreEncapsulation.Api;

[ApiController]
[Route("students")]
public class StudentController : ControllerBase
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;

    public StudentController(StudentRepository studentRepository, CourseRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    [HttpGet("{id}")]
    public StudentDto Get(long id)
    {
        Student student = _studentRepository.GetById(id);

        if (student == null)
        {
            return null;
        }

        return new StudentDto
        {
            StudentId = student.Id,
            Name = student.Name,
            Email = student.Email,
            Enrollments = student.Enrollments.Select(e => new EnrollmentDto
            {
                Course = e.Course.Name,
                Grade = e.Grade.ToString()
            }).ToList()
        };
    }

    [HttpPost]
    public void Register()
    {
        var student = new Student();
        
        _studentRepository.Save(student);
    }

    [HttpPost]
    public string Enroll(long studentId, long courseId, Grade grade)
    {
        var student = _studentRepository.GetById(studentId);
        var course = _courseRepository.GetById(courseId);

        string result = student.EnrollIn(course, grade);
        
        _studentRepository.Save(student);

        return result;
    }
}
