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
[Route("api/grades")]
public class GradesController : Controller
{
    private readonly ILogger<GradesController> _logger;
    private readonly DatabaseContext _context;

    public GradesController(DatabaseContext context, ILogger<GradesController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    /// <summary>
    /// Get grades list
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// GET api/grades
    /// 
    /// </remarks>
    /// <response code="200">Return grades list</response>
    /// <response code="404">Grades not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Grade>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<IEnumerable<Grade>>> Get([FromQuery] QueryParameters<Grade> queryParameters)
    {
        _logger.LogDebug("Get list of grades");

        IQueryable<Grade> allGrades =
            _context.Grades
                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());

        if (queryParameters.HasQuery())
        {
            try
            {
                var parametersObject = (Grade) queryParameters.Object;
                allGrades = allGrades.Where(u =>
                    u.g_value == parametersObject.g_value
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
            }
        }
        
        return await allGrades.CountAsync() switch
        {
            0 => NotFound(new ErrorModel("Grades not found")),
            _ => Ok(allGrades)
        };
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
    /// <response code="200">Return editGrade with specific id</response>
    /// <response code="404">Grade not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Grade), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Grade>> Get(int id)
    {
        _logger.LogDebug("Get editGrade with id = {Id}", id);

        var grade = await _context.Grades
            .FirstOrDefaultAsync(u => u.g_id == id);
        
        if (grade is null)
            return NotFound(new ErrorModel("Grade not found"));

        return Ok(grade);
    }

    /// <summary>
    /// Create new editGrade
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// POST api/editGrade&#xA;&#xD;
    ///     {
    ///         "login": "ttlc198",
    ///         "password": "M$4d3ikx+L",
    ///         "email": "ttlc198@gmail.com",
    ///         "roleId": 3
    ///     }
    /// 
    /// </remarks>
    /// <param name="grade"></param>
    /// <response code="201">Return created editGrade</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Grade), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Grade>> Post([FromBody] Grade? grade)
    {
        if (grade is null)
            return BadRequest(new ErrorModel("Input data is empty"));

        _logger.LogDebug("Create new editGrade with value = {value}", grade.g_value);

        var createdGrade = await _context.Grades.AddAsync(grade);

        return await _context.SaveChangesAsync() switch
        {
            0 => StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred")),
            _ => StatusCode(StatusCodes.Status201Created, createdGrade)
        };
    }

    /// <summary>
    /// Update single grade
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// PUT api/grades&#xA;&#xD;
    ///     {
    ///         "id": 3,
    ///         "login": "ttlc198",
    ///         "password": "M$4d3ikx+L1",
    ///         "email": "ttlc198@gmail.com",
    ///         "roleId": 3
    ///     }
    /// 
    /// </remarks>
    /// <param name="editGrade"></param>
    /// <response code="200">Return changed user</response>
    /// <response code="400">Input data is empty</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpPut]
    [ProducesResponseType(typeof(Grade), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<Grade>> Put([FromBody] Grade editGrade)
    {
        if (editGrade is null)
            return BadRequest(new ErrorModel("Input data is empty"));

        if (!_context.Grades.Any(u => u.g_id == editGrade.g_id))
            return NotFound(new ErrorModel("User not found"));
        _logger.LogDebug("Update existing editGrade with id = {Id}", editGrade.g_id);

        var grade = await _context.Grades
            .FirstOrDefaultAsync(u => u.g_id == editGrade.g_id);
        
        if (grade is null)
            return BadRequest(new ErrorModel("Grade does not exist"));
        
        _context.Entry(grade).State = EntityState.Modified;

        switch (await _context.SaveChangesAsync())
        {
            case 0:
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"));
            default:
                var editedGrade =
                    await _context.Grades.FirstOrDefaultAsync(u => u.g_id == grade.g_id);
                return editedGrade is null
                    ? StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred"))
                    : Ok(editedGrade);
        }
    }
    
    /// <summary>
    /// Delete single grade
    /// </summary>
    /// <remarks>
    /// Example request
    /// 
    /// DELETE api/grades/11
    /// 
    /// </remarks>
    /// <param name="id"></param>
    /// <response code="204">Delete grade</response>
    /// <response code="400">The input data is empty</response>
    /// <response code="404">Grade not found</response>
    /// <response code="500">Oops! Server internal error</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), (int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Delete(long id)
    {
        if (id <= 0)
            return BadRequest(new ErrorModel("The input data is empty"));

        _logger.LogDebug("Delete existing grade with id = {Id}", id);

        var toDelete = await _context.Grades.FirstOrDefaultAsync(u => u.g_id == id);

        if (toDelete is null)
            return NotFound(new ErrorModel("Grade not found"));

        _context.Grades.Remove(toDelete);

        return await _context.SaveChangesAsync() switch
        {
            0 => StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel("Some error has occurred")),
            _ => NoContent()
        };
    }
}