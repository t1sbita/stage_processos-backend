using StageProcessos.Domain.Entities;

namespace StageProcessos.Domain.Dtos;

public class ProcessDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsSubProcess { get; set; }
    public int FatherProcessId { get; set; }
    public IEnumerable<ProcessDto> SubProcesss { get; set; } = [];

    public ProcessDto() { }
    public ProcessDto(Process process)
    {
        Id = process.Id;
        Name = process.Name;
        Description = process.Description;
        CreatedAt = process.CreatedAt;
        UpdatedAt = process.UpdatedAt;
        IsSubProcess = process.IsSubProcess;
        FatherProcessId = process.FatherProcessId;
        SubProcesss = process.SubProcesss.Select(x => new ProcessDto(x));
    }


    public Process ToProcess()
    {
        return new Process()
        {
            Name = Name,
            Description = Description,
            IsSubProcess = IsSubProcess,
            FatherProcessId = FatherProcessId,
            SubProcesss = SubProcesss.Select(x =>  x.ToProcess()).ToList()
        };
    }
}
