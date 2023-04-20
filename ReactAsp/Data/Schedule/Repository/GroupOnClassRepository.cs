using System.Linq.Expressions;
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

    public override Task<bool> ExistsAsync(string value)
    {
        throw new NotImplementedException();
    }

    public override Task<GroupOnClass> GetByFieldValueAsync(Expression<Func<GroupOnClass, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}