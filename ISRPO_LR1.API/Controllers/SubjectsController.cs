using ISRPO_LR1.API.Models;
using ISRPO_LR1.API.Utils;
using ISRPO_LR1.Domain;
using ISRPO_LR1.Domain.Context;
using System.Linq.Dynamic.Core;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISRPO_LR1.API.Controllers;

[ApiController]
[Route("api/subjects")]
public class SubjectsController : Controller
{
    private readonly ILogger<SubjectsController> _logger;
    private readonly DatabaseContext _context;

    public SubjectsController(DatabaseContext context, ILogger<SubjectsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get subjects list
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// GET api/subjects
    /// 
    /// </remarks>
    /// <response code="200">Return subjects list</response>
    /// <response code="404">Subjects not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Subject>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<IEnumerable<Subject>>> Get([FromQuery] QueryParameters<Subject> queryParameters)
    {
        try
        {
            _logger.LogDebug("Get list of subjects");

            IQueryable<Subject> allSubjects =
                _context.Subjects
                    .Include(s => s.Grades);

            if (queryParameters.HasQuery())
            {
                if (queryParameters.Object != null)
                {
                    var parametersObject = (Subject) queryParameters.Object;
                    allSubjects = allSubjects.Where(u =>
                        u.sj_name == parametersObject.sj_name
                    );
                }
            }

            return await allSubjects.CountAsync() switch
            {
                0 => NotFound(new ErrorModel("Subjects not found")),
                _ => Ok(await allSubjects.ToListAsync())
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
    /// GET api/subjects/11
    /// 
    /// </remarks>
    /// <param name="id"></param>
    /// <response code="200">Return subjects with specific id</response>
    /// <response code="404">subject not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Subject), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Subject>> Get(int id)
    {
        try
        {
            _logger.LogDebug("Get subject with id = {Id}", id);

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(u => u.sj_id == id);

            if (subject is null)
                return NotFound(new ErrorModel("Subject not found"));

            return Ok(subject);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }

    /// <summary>
    /// Create new subjects
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// POST api/subjects&#xA;&#xD;
    ///     {
    ///         "login": "ttlc198",
    ///         "password": "M$4d3ikx+L",
    ///         "email": "ttlc198@gmail.com",
    ///         "roleId": 3
    ///     }
    /// 
    /// </remarks>
    /// <response code="201">Return created subject</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Subject), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Subject>> Post([FromBody] Subject? subject)
    {
        try
        {
            if (subject is null)
                return BadRequest(new ErrorModel("Input data is empty"));

            _logger.LogDebug("Create new subject with id = {id}", subject.sj_id);

            var createdSubject = await _context.Subjects.AddAsync(subject);

            return await _context.SaveChangesAsync() switch
            {
                0 => StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred")),
                _ => StatusCode(StatusCodes.Status201Created, createdSubject.Entity)
            };
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }

    /// <summary>
    /// Update single subjects
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// PUT api/subjects&#xA;&#xD;
    ///     {
    ///         "id": 3,
    ///         "login": "ttlc198",
    ///         "password": "M$4d3ikx+L1",
    ///         "email": "ttlc198@gmail.com",
    ///         "roleId": 3
    ///     }
    /// 
    /// </remarks>
    /// <param name="editSubject"></param>
    /// <response code="200">Return changed subject</response>
    /// <response code="400">Input data is empty</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpPut]
    [ProducesResponseType(typeof(Student), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Subject>> Put([FromBody] Subject? editSubject)
    {
        try
        {
            if (editSubject is null)
                return BadRequest(new ErrorModel("Input data is empty"));

            if (!_context.Subjects.Any(u => u.sj_id == editSubject.sj_id))
                return NotFound(new ErrorModel("Subject not found"));

            _logger.LogDebug("Update existing subject with id = {Id}", editSubject.sj_id);

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(u => u.sj_id == editSubject.sj_id);

            if (subject is null)
                return BadRequest(new ErrorModel("Subject does not exist"));

            _context.Entry(subject).State = EntityState.Modified;

            switch (await _context.SaveChangesAsync())
            {
                case 0:
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new ErrorModel("Some error has occurred"));
                default:
                    var editedSubject =
                        await _context.Subjects.FirstOrDefaultAsync(u => u.sj_id == subject.sj_id);
                    return editedSubject is null
                        ? StatusCode(StatusCodes.Status500InternalServerError,
                            new ErrorModel("Some error has occurred"))
                        : Ok(editedSubject);
            }
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
        }
    }

    /// <summary>
    /// Delete single subject
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// DELETE api/students/11
    /// 
    /// </remarks>
    /// <param name="id"></param>
    /// <response code="204">Delete subject</response>
    /// <response code="400">The input data is empty</response>
    /// <response code="404">subject not found</response>
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

            _logger.LogDebug("Delete existing subject with id = {Id}", id);

            var toDelete = await _context.Subjects.FirstOrDefaultAsync(u => u.sj_id == id);

            if (toDelete is null)
                return NotFound(new ErrorModel("Subject not found"));

            _context.Subjects.Remove(toDelete);

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