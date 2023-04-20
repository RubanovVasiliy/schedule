using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;


public interface IScheduleLoadRepository : IRepository<ScheduleLoad>
{
}

public class ScheduleLoadRepository : Repository<ScheduleLoad>, IScheduleLoadRepository
{
    public ScheduleLoadRepository(DbContext context) : base(context)
    {
    }

    public override Task<bool> ExistsAsync(string value)
    {
        throw new NotImplementedException();
    }

    public override Task<ScheduleLoad> GetByFieldValueAsync(Expression<Func<ScheduleLoad, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}