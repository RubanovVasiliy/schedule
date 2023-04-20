using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface IGroupRepository : IRepository<Group>
{
}

public class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(DbContext context) : base(context)
    {
    }
}