using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

public class StudentsController : BaseController<Student>
{
    public StudentsController(BaseRepository<Student> baseRepository, Logger<BaseController<Student>> logger) : base(baseRepository, logger)
    {
    }
    
    public async Task<IActionResult> Index()
    {
        var response = await BaseRepository.Get();
        if (response.Success)
            ViewBag.Grades = response.Item ?? new List<Student>();
        return View();
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await BaseRepository.Get();
        if (response.Success)
            ViewBag.Grades = response.Item ?? new List<Student>();
        return View("Grade");
    }
    
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(Student model)
    {
        var response = await BaseRepository.Post(model);
        if (response.Success)
            return RedirectToAction("Index");
        return View();
    }
    
    [HttpPut("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, Student model)
    {
        var response = await BaseRepository.Put(id, model);
        if (response.Success)
            return RedirectToAction("Index");
        return View();
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await BaseRepository.Delete(id);
        if (response)
            return RedirectToAction("Index");
        return View();
    }
}