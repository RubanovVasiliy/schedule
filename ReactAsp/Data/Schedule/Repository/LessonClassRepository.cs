using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface ILessonClassRepository : IRepository<LessonClass>
{
}

public class LessonClassRepository : Repository<LessonClass>, ILessonClassRepository
{
    public LessonClassRepository(DbContext context) : base(context)
    {
    }

    public override Task<bool> ExistsAsync(string value)
    {
        throw new NotImplementedException();
    }

    public override Task<LessonClass> GetByFieldValueAsync(Expression<Func<LessonClass, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}