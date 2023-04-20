using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface ISubjectRepository : IRepository<Subject>
{
}

public class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public SubjectRepository(DbContext context) : base(context)
    {
    }
}