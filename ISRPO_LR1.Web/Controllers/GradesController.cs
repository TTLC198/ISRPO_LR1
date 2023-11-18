using ISRPO_LR1.Domain;
using ISRPO_LR1.Domain.Service;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Models.ViewModels;
using ISRPO_LR1.Web.Repositories;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

[Route("grades")]
public class GradesController : BaseController<Grade>
{
    private readonly StudentsRepository _studentsRepository;
    private readonly SubjectsRepository _subjectsRepository;
    public GradesController(BaseRepository<Grade> baseRepository, ILogger<GradesController> logger, BaseRepository<Student> studentsRepository, BaseRepository<Subject> subjectsRepository) : base(baseRepository, logger)
    {
        _studentsRepository = (studentsRepository as StudentsRepository)!;
        _subjectsRepository = (subjectsRepository as SubjectsRepository)!;
    }
    
    public override async Task<IActionResult> Index()
    {
        var response = await _studentsRepository.Get();
        if (!response.Success)
            return View(new ServiceResponseModel<BaseIndexViewModel<Student>>()
            {
                Error = response.Error
            });
        return View(new ServiceResponseModel<BaseIndexViewModel<Student>>()
        {
            Success = true,
            Item = new BaseIndexViewModel<Student>()
            {
                Items = response.Item ?? new List<Student>()
            }
        });
    }
    
    [HttpGet("create")]
    public override async Task<IActionResult> Create()
    {
        var studentsResponse = await _studentsRepository.Get();
        var subjectsResponse = await _subjectsRepository.Get();
        
        return View(new GradeCreateViewModel()
        {
            Subjects = subjectsResponse.Item ?? new List<Subject>(),
            Students = studentsResponse.Item ?? new List<Student>()
        });
    }
    
    [HttpGet("get/{id:int}")]
    public override async Task<IActionResult> Get(int id)
    {
        var response = await BaseRepository.Get(id);
        var studentResponse = await _studentsRepository.Get(response.Item?.g_s_id ?? 0);
        if (studentResponse.Success)
            ViewBag.StudentName = studentResponse.Item?.s_full_name ?? "Нет имени";
        var subjectResponse = await _subjectsRepository.Get(response.Item?.g_sj_id ?? 0);
        if (subjectResponse.Success)
            ViewBag.SubjectName = subjectResponse.Item?.sj_name ?? "Нет названия";
        return View(response);
    }
}