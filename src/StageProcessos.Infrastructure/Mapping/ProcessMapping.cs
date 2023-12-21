using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StageProcessos.Domain.Entities;

namespace StageProcessos.Infrastructure.Mapping;

internal class ProcessMapping : IEntityTypeConfiguration<Process>
{
    public void Configure(EntityTypeBuilder<Process> builder)
    {
        builder.ToTable("process");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.FatherProcessId).HasColumnName("fatherid");
        builder.Property(x => x.IsSubProcess).HasColumnName("issubprocess");
        builder.Property(x => x.CreatedAt).HasColumnName("createdat");
        builder.Property(x => x.UpdatedAt).HasColumnName("updatedat");

    }
}
