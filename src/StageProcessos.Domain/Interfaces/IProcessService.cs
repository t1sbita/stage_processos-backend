using StageProcessos.Domain.Dtos;
using StageProcessos.Domain.Entities;
using StageProcessos.Domain.Utils;
using System.Reflection.Metadata;

namespace StageProcessos.Domain.Interfaces;

public interface IProcessService
{
    Task<ProcessDto> AddAsync(ProcessDto dto);
    Task<ProcessDto> AddChildrensAsync(int idFather, List<ProcessDto> dto);
    Task<ProcessDto> UpdateAsync(ProcessDto dto);
    Task<bool> Remove(long id);
    //ProcessDto GetProcessById(long id);
    //PageConsultation<Process> GetAllPaginated(FilterProcessViewModel filter, int page = 1, int sizePage = 10);
    Task<IEnumerable<ProcessDto>> GetAllAsync();
}
