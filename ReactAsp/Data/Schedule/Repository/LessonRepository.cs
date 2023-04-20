using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule.Repository;



public interface ILessonRepository : IRepository<Lesson>
{
}

public class LessonRepository : Repository<Lesson>, ILessonRepository
{
    public LessonRepository(DbContext context) : base(context)
    {
    }

    public override Task<bool> ExistsAsync(string value)
    {
        throw new NotImplementedException();
    }
}