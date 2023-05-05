using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface ILessonClassRepository : IRepository<LessonGroup>
{
}

public class LessonClassRepository : Repository<LessonGroup>, ILessonClassRepository
{
    public LessonClassRepository(DbContext context) : base(context)
    {
    }

    public override Task<bool> ExistsAsync(string value)
    {
        throw new NotImplementedException();
    }

    public override Task<LessonGroup> GetByFieldValueAsync(Expression<Func<LessonGroup, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}