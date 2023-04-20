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
}