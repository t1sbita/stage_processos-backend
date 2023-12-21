using Microsoft.EntityFrameworkCore;

namespace StageProcessos.Infrastructure.Context;

public class BaseContext : DbContext
{
    public BaseContext() : base()
    {

    }
    public BaseContext(DbContextOptions options) : base(options)
    {
    }
}
