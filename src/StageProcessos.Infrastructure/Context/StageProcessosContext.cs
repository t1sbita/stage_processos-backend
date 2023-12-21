using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace StageProcessos.Infrastructure.Context;

public class StageProcessosContext : BaseContext
{
    public StageProcessosContext(DbContextOptions<StageProcessosContext> options) : base(options) { }
    public StageProcessosContext() : base() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var typesToRegister = from t in Assembly.GetExecutingAssembly().GetTypes()
                              where !string.IsNullOrWhiteSpace(t.Namespace) &&
                              t.GetInterfaces().Any(i => i.Name == typeof(IEntityTypeConfiguration<>).Name && i.Namespace == typeof(IEntityTypeConfiguration<>).Namespace)
                              select t;

        foreach (var type in typesToRegister)
        {
            dynamic? configurationInstance = Activator.CreateInstance(type);
            modelBuilder.ApplyConfiguration(configurationInstance);
        }

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql();
        }
        base.OnConfiguring(optionsBuilder);

    }

}
