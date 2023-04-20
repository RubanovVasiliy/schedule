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
}