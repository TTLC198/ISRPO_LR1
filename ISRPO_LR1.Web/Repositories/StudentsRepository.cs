using System.Text.Json;
using ISRPO_LR1.Domain;
using ISRPO_LR1.Domain.Service;
using ISRPO_LR1.Web.Repositories.Base;

namespace ISRPO_LR1.Web.Repositories;

public class StudentsRepository : BaseRepository<Student>
{
    public StudentsRepository(HttpClient httpClient, ILogger<StudentsRepository> logger) : base(httpClient, logger)
    {
    }
}