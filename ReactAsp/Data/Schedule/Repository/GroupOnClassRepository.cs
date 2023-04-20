using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface IGroupOnClassRepository : IRepository<GroupOnClass>
{
}

public class GroupOnClassRepository : Repository<GroupOnClass>, IGroupOnClassRepository
{
    public GroupOnClassRepository(DbContext context) : base(context)
    {
    }
}