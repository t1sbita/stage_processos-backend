using Microsoft.AspNetCore.Mvc;
using StageProcessos.Domain.Dtos;
using StageProcessos.Domain.Interfaces;

namespace StageProcessos.Presentation.Controllers;

[ApiController]
[Route("api/process")]
public class ProcessController(IProcessService processService) : ControllerBase
{
    private readonly IProcessService _processService = processService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _processService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ProcessDto process)
    {
        var result = await _processService.AddAsync(process);
        return Ok(result);
    }

    [HttpPost("id")]
    public async Task<IActionResult> AddChildrens(int idFather, [FromBody] List<ProcessDto> process)
    {
        var result = await _processService.AddChildrensAsync(idFather, process);
        return Ok(result);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Remove(int id)
    {
       var result = await _processService.Remove(id);
        return Ok(result);
    }

}
