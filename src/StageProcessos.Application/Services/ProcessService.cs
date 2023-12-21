using Microsoft.Extensions.Logging;
using StageProcessos.Domain.Dtos;
using StageProcessos.Domain.Entities;
using StageProcessos.Domain.Interfaces;

namespace StageProcessos.Application.Services;

public class ProcessService(IProcessRepository processRepository, ILogger<ProcessService> logger) : IProcessService
{
    private readonly ILogger<ProcessService> _logger = logger;
    private readonly IProcessRepository _processRepository = processRepository;


    public async Task<IEnumerable<ProcessDto>> GetAllAsync()
    {
        var list = await _processRepository.GetAllAsync();

        var result = list.Select(x => new ProcessDto(x));

        return result;
    }
    public async Task<ProcessDto> AddAsync(ProcessDto dto)
    {
        try
        {
            Process process = dto.ToProcess();

            var result = await _processRepository.AddAsync(process);
            _logger.LogInformation("Adding process {nameProcess}, with {count} subprocess", process.Name, process.SubProcesss.Count);
            await _processRepository.SaveAsync();

            return new ProcessDto(result);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while adding process with error: {error}", e.Message);
            throw;
        }
        
    }

    public async Task<ProcessDto> AddChildrensAsync(int idFather, List<ProcessDto> dto)
    {
        try
        {
            var process = await _processRepository.GetAsync(x => x.Id == idFather, x => x.SubProcesss);

            if (process == null) throw new Exception("Process not found");

            dto.ForEach(x => { x.FatherProcessId = idFather; x.IsSubProcess = true; } );

            var list = dto.Select(x => x.ToProcess());

            _logger.LogInformation("Add {countItens} to processId {idFather}", list.Count(), idFather);

            await _processRepository.AddRangeAsync(list);
            await _processRepository.SaveAsync();

            var result = await _processRepository.GetAsync(x => x.Id == idFather, x => x.SubProcesss);

            return new ProcessDto(result);
        }

        catch (Exception e)
        {
            _logger.LogError("Error while adding subprocess to processId {idFather} with error: {error}",idFather, e.Message);
            throw;
        }
    }

    public async Task<ProcessDto> UpdateAsync(ProcessDto dto)
    {
        Process process = dto.ToProcess();

        var result = await _processRepository.UpdateAsync(process);
        await _processRepository.SaveAsync();

        return new ProcessDto(result);
    }

    public async Task<bool> Remove(long id)
    {
        var process = _processRepository.GetQueryable(x => x.Id == id, x => x.SubProcesss).FirstOrDefault();

        var result = _processRepository.Delete(process);
        await _processRepository.SaveAsync();

        return result;
    }
}
