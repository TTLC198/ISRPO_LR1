using System.Linq.Dynamic.Core;
using System.Net;
using ISRPO_LR1.API.Models;
using ISRPO_LR1.API.Utils;
using ISRPO_LR1.Domain;
using ISRPO_LR1.Domain.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISRPO_LR1.API.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : Controller
{
    private readonly ILogger<StudentsController> _logger;
    private readonly DatabaseContext _context;

    public StudentsController(DatabaseContext context, ILogger<StudentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get students list
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// GET api/students
    /// 
    /// </remarks>
    /// <response code="200">Return students list</response>
    /// <response code="404">Students not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Student>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<IEnumerable<Student>>> Get([FromQuery] QueryParameters<Student> queryParameters)
    {
        try
        {
            _logger.LogDebug("Get list of students");

            IQueryable<Student> allStudents =
                _context.Students
                    .Include(s => s.Grades);

            if (queryParameters.HasQuery())
            {
                if (queryParameters.Object != null)
                {
                    var parametersObject = (Student) queryParameters.Object;
                    allStudents = allStudents.Where(u =>
                        u.s_full_name == parametersObject.s_full_name |
                        u.s_birth_date == parametersObject.s_birth_date |
                        u.s_email == parametersObject.s_email
                    );
                }
            }

            return await allStudents.CountAsync() switch
            {
                0 => NotFound(new ErrorModel("Students not found")),
                _ => Ok(await allStudents.ToListAsync())
            };
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }

    /// <summary>
    /// Get single editGrade
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// GET api/grades/11
    /// 
    /// </remarks>
    /// <param name="id"></param>
    /// <response code="200">Return student with specific id</response>
    /// <response code="404">student not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Student), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Student>> Get(int id)
    {
        try
        {
            _logger.LogDebug("Get student with id = {Id}", id);

            var student = await _context.Students
                .FirstOrDefaultAsync(u => u.s_id == id);

            if (student is null)
                return NotFound(new ErrorModel("Student not found"));

            return Ok(student);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }

    /// <summary>
    /// Create new students
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// POST api/students&#xA;&#xD;
    ///     {
    ///         "login": "ttlc198",
    ///         "password": "M$4d3ikx+L",
    ///         "email": "ttlc198@gmail.com",
    ///         "roleId": 3
    ///     }
    /// 
    /// </remarks>
    /// <param name="student"></param>
    /// <response code="201">Return created student</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Student), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Student>> Post([FromBody] Student? student)
    {
        try
        {
            if (student is null)
                return BadRequest(new ErrorModel("Input data is empty"));

            _logger.LogDebug("Create new student with id = {id}", student.s_id);

            var createdStudent = await _context.Students.AddAsync(student);

            return await _context.SaveChangesAsync() switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred")),
                _ => StatusCode(StatusCodes.Status201Created, createdStudent.Entity)
            };
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }

    /// <summary>
    /// Update single student
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// PUT api/students&#xA;&#xD;
    ///     {
    ///         "id": 3,
    ///         "login": "ttlc198",
    ///         "password": "M$4d3ikx+L1",
    ///         "email": "ttlc198@gmail.com",
    ///         "roleId": 3
    ///     }
    /// 
    /// </remarks>
    /// <param name="editStudent"></param>
    /// <response code="200">Return changed student</response>
    /// <response code="400">Input data is empty</response>
    /// <response code="404">Student not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpPut]
    [ProducesResponseType(typeof(Student), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Student>> Put([FromBody] Student? editStudent)
    {
        try
        {
            if (editStudent is null)
                return BadRequest(new ErrorModel("Input data is empty"));

            if (!_context.Students.Any(u => u.s_id == editStudent.s_id))
                return NotFound(new ErrorModel("Student not found"));

            _logger.LogDebug("Update existing student with id = {Id}", editStudent.s_id);

            var student = await _context.Students
                .FirstOrDefaultAsync(u => u.s_id == editStudent.s_id);

            if (student is null)
                return BadRequest(new ErrorModel("Student does not exist"));

            student.s_birth_date = editStudent.s_birth_date;
            student.s_email = editStudent.s_email;
            student.s_full_name = editStudent.s_full_name;

            _context.Entry(student).State = EntityState.Modified;

            switch (await _context.SaveChangesAsync())
            {
                case 0:
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new ErrorModel("Some error has occurred"));
                default:
                    var editedStudent =
                        await _context.Students.FirstOrDefaultAsync(u => u.s_id == student.s_id);
                    return editedStudent is null
                        ? StatusCode(StatusCodes.Status500InternalServerError,
                            new ErrorModel("Some error has occurred"))
                        : Ok(editedStudent);
            }
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }

    /// <summary>
    /// Delete single student
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// DELETE api/students/11
    /// 
    /// </remarks>
    /// <param name="id"></param>
    /// <response code="204">Delete student</response>
    /// <response code="400">The input data is empty</response>
    /// <response code="404">Student not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Delete(long id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new ErrorModel("The input data is empty"));

            _logger.LogDebug("Delete existing student with id = {Id}", id);

            var toDelete = await _context.Students.FirstOrDefaultAsync(u => u.s_id == id);

            if (toDelete is null)
                return NotFound(new ErrorModel("Student not found"));

            _context.Students.Remove(toDelete);

            return await _context.SaveChangesAsync() switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred")),
                _ => NoContent()
            };
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }
}