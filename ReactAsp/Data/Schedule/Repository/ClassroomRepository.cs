using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface IClassroomRepository : IRepository<Classroom>
{
}

public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
{
    public ClassroomRepository(DbContext context) : base(context)
    {
    }

    public override async Task<bool> ExistsAsync(string value)
    {
        return await _dbSet.AnyAsync(e => e.ClassroomNumber == value);
    }

    public override async Task<Classroom> GetByFieldValueAsync(Expression<Func<Classroom, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> CreateIfNotExistAsync(Classroom entity)
    {
        if (await _dbSet.AnyAsync(e => e.ClassroomNumber == entity.ClassroomNumber)) return false;

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}