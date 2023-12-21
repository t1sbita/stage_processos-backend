using StageProcessos.Domain.Entities.Base;

namespace StageProcessos.Domain.Entities;

public class Process : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get;}
    public bool IsSubProcess { get; set; }
    public int FatherProcessId { get; set; }
    public virtual Process FatherProcess {  get; set; }
    public List<Process> SubProcesss { get; set; } = [];
}
