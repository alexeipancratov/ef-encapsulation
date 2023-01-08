using EFCoreEncapsulation.Api.Aggregates.CourseAggregate;
using EFCoreEncapsulation.Api.Aggregates.StudentAggregate;
using EFCoreEncapsulation.Api.Aggregates.StudentAggregate.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreEncapsulation.Api;

[ApiController]
[Route("students")]
public class StudentController : ControllerBase
{
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;
    private readonly SchoolContext _schoolContext;

    public StudentController(StudentRepository studentRepository, CourseRepository courseRepository,
        SchoolContext schoolContext)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _schoolContext = schoolContext;
    }

    [HttpGet]
    public IReadOnlyList<StudentDto> Get()
    {
        return _studentRepository.GetAll(".com")
            .Select(MapToDto)
            .ToList();
    }

    private StudentDto MapToDto(Student student)
    {
        return new StudentDto
        {
            StudentId = student.Id,
            Name = student.Name,
            Email = student.Email
        };
    }

    [HttpGet("{id}")]
    public StudentDto Get(long id)
    {
        // NOTE: In the initial approach we were retrieving Courses together with Enrollments
        // even though they were outside of aggregate scope (using .AutoLoad()).
        // But now we decided to optimize this, and we're retrieving data directly from repository in an optimized way.

        //Student student = _studentRepository.GetById(id);

        //if (student == null)
        //{
        //    return null;
        //}

        //return new StudentDto
        //{
        //    StudentId = student.Id,
        //    Name = student.Name,
        //    Email = student.Email,
        //    Enrollments = student.Enrollments.Select(e => new EnrollmentDto
        //    {
        //        Course = e.Course.Name,
        //        Grade = e.Grade.ToString()
        //    }).ToList()
        //};

        return _studentRepository.GetDto(id);
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

    [HttpPost]
    public string EditPersonalInfo(long studentId, string name)
    {
        var student = _studentRepository.GetById(studentId);
        student.Name = name;

        _schoolContext.SaveChanges();

        return "OK";
    }
}