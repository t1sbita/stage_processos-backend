
using Microsoft.Extensions.Logging;
using StageProcessos.Domain.Entities;
using StageProcessos.Domain.Interfaces;
using StageProcessos.Infrastructure.Context;
using StageProcessos.Infrastructure.Data.Repositories.Base;

namespace StageProcessos.Infrastructure.Data.Repositories;

public class ProcessRepository(StageProcessosContext context, ILogger<Process> logger) : BaseRepository<Process>(context, logger), IProcessRepository
{
    
}
