using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface IGroupRepository : IRepository<Group>
{
    Task<bool> CreateIfNotExistAsync(Group entity);

}

public class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(DbContext context) : base(context)
    {
    }

    public override async Task<bool> ExistsAsync(string value)
    {
        return await _dbSet.AnyAsync(e => e.GroupNumber == value);
    }

    public override Task<Group> GetByFieldValueAsync(Expression<Func<Group, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateIfNotExistAsync(Group entity)
    {
        if (await _dbSet.AnyAsync(e => e.GroupNumber == entity.GroupNumber)) return false;

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}