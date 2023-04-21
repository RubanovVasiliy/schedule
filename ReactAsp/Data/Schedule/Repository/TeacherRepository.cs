using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface ITeacherRepository : IRepository<Teacher>
{
}

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(DbContext context) : base(context)
    {
    }

    public override Task<bool> ExistsAsync(string value)
    {
        throw new NotImplementedException();
    }

    public override async Task<Teacher> GetByFieldValueAsync(Expression<Func<Teacher, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> CreateIfNotExistAsync(Teacher entity)
    {
        if (await _dbSet.AnyAsync(e => e.FullName == entity.FullName)) return false;

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}