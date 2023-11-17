using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Repositories.Base;

namespace ISRPO_LR1.Web.Repositories;

public class SubjectsRepository : BaseRepository<Subject>
{
    public SubjectsRepository(HttpClient httpClient, ILogger<SubjectsRepository> logger) : base(httpClient, logger)
    {
    }
}