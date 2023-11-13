using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

public class SubjectsController : BaseController<Subject>
{
    public SubjectsController(BaseRepository<SubjectsController> baseRepository, Logger<BaseController<SubjectsController>> logger) : base(baseRepository, logger)
    {
    }
    
    public async Task<IActionResult> Index()
    {
        var response = await BaseRepository.Get();
        if (response.Success)
            ViewBag.Grades = response.Item ?? new List<Subject>();
        return View();
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await BaseRepository.Get();
        if (response.Success)
            ViewBag.Grades = response.Item ?? new List<Subject>();
        return View("Grade");
    }
    
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(Subject model)
    {
        var response = await BaseRepository.Post(model);
        if (response.Success)
            return RedirectToAction("Index");
        return View();
    }
    
    [HttpPut("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, Subject model)
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