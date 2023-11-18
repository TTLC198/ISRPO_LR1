using System.ComponentModel.DataAnnotations;
using ISRPO_LR1.Domain;

namespace ISRPO_LR1.Web.Models.ViewModels;

public class GradeCreateViewModel : Grade
{
    [Display(Name = "Предмет")] 
    public Subject g_sj { get; set; } = new Subject();
    [Display(Name = "Студент")] 
    public Student g_s { get; set; } = new Student();
    public List<Subject> Subjects { get; set; } = new List<Subject>();
    public List<Student> Students { get; set; } = new List<Student>();
}