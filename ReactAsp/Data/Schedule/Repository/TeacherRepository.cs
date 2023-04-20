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
    
    public async Task<bool> CreateIfNotExistAsync(Teacher entity)
    {
        if (await _dbSet.AnyAsync(e => e.FullName == entity.FullName)) return false;

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}