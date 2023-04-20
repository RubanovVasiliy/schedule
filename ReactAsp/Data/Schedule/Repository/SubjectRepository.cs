using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;

public interface ISubjectRepository : IRepository<Subject>
{
}

public class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public SubjectRepository(DbContext context) : base(context)
    {
    }

    public override async Task<bool> ExistsAsync(string value)
    {
        return await _dbSet.AnyAsync(e => e.SubjectName == value);
    }

    public override Task<Subject> GetByFieldValueAsync(Expression<Func<Subject, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateIfNotExistAsync(Subject entity)
    {
        if (await _dbSet.AnyAsync(e => e.SubjectName == entity.SubjectName)) return false;

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}