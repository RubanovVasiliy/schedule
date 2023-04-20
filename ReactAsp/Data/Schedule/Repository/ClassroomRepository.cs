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
}