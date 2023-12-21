using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StageProcessos.Domain.Entities.Base;
using StageProcessos.Domain.Interfaces;
using StageProcessos.Domain.Utils;
using StageProcessos.Infrastructure.Context;
using System.Linq.Expressions;

namespace StageProcessos.Infrastructure.Data.Repositories.Base;

public class BaseRepository<T>(StageProcessosContext context, ILogger<T> logger) : IDisposable, IBaseRepository<T> where T : BaseEntity
{
    private bool _disposed;
    protected readonly StageProcessosContext Context = context;
    public readonly ILogger<T> _logger = logger;
    private DbSet<T> Entities { get; set; } = context.Set<T>();

    private IQueryable<T> Query(params Expression<Func<T, object>>[] includes)
    {
        var query = Entities.AsQueryable();

        if (includes != null)
        {
            foreach (var include in includes)
                if (include != null)
                    query = query.Include(include);
        }
        return query;
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includes)
    {
        var query = Query(includes).AsNoTracking();

        if (filters != null)
            return query.Where(filters).SingleOrDefault()!;

        return await query.SingleOrDefaultAsync();
    }

    public async Task<T> GetByIdAsync(long Id, params Expression<Func<T, object>>[] includes)
    {
        return await Query(includes).SingleOrDefaultAsync(i => i.Id.Equals(Id));
    }

    public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filters = null,
        params Expression<Func<T, object>>[] includes)
    {
        var query = Query(includes).AsNoTracking();

        if (filters != null)
            query = query.Where(filters);

        return query;
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filters = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> sortedBy = null,
        params Expression<Func<T, object>>[] includes)
    {
        var query = Query(includes).AsNoTracking();

        if (filters != null)
            query = query.Where(filters);

        if (sortedBy != null)
            query = sortedBy(query);

        return await query.ToListAsync();
    }

    public Task<PageConsultation<T>> GetAllPagedAsync(int page, int itemsByPage,
        ICollection<Expression<Func<T, bool>>>? filters = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? sortedBy = null,
        params Expression<Func<T, object>>[] includes)
    {
        var query = Query(includes).AsNoTracking();

        PageConsultation<T> pageConsultation = new PageConsultation<T>();

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }

        int total = query.Count();

        if (sortedBy != null)
            query = sortedBy(query);
        else
            query = query.OrderBy(o => o.Id);

        if (page < 1)
            page = 1;

        pageConsultation.NumberPage = page;
        pageConsultation.SizePage = itemsByPage;
        pageConsultation.TotalRecords = total;

        if (pageConsultation.TotalRecords > 0 && pageConsultation.SizePage > 0)
        {
            pageConsultation.TotalPages = pageConsultation.TotalRecords / pageConsultation.SizePage;

            if (pageConsultation.TotalRecords % pageConsultation.SizePage > 0)
            {
                pageConsultation.TotalPages++;
            }
        }

        pageConsultation.List = query.Skip(itemsByPage * (page - 1)).Take(itemsByPage);

        return Task.FromResult(pageConsultation);
    }

    public async Task<T> AddAsync(T Entity)
    {
        if (Entity == null)
            throw new ArgumentNullException(typeof(T).FullName);

        var retorno = await Entities.AddAsync(Entity);

        return retorno.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> Entity)
    {
        if (Entity == null)
            throw new ArgumentNullException(typeof(T).FullName);

        await Entities.AddRangeAsync(Entity);
    }

    public async Task<T> UpdateAsync(T Entity)
    {
        if (Entity == null)
            throw new ArgumentNullException(typeof(T).FullName);

        T? exist = await Entities.SingleOrDefaultAsync(t => t.Id == Entity.Id);
        if (exist != null)
        {
            Context.Entry(exist).CurrentValues.SetValues(Entity);
            return Entity;
        }

        return null!;
    }

    public async Task<bool> DeleteAsync(long Id)
    {
        T exist = await Entities.SingleOrDefaultAsync(t => t.Id == Id);
        if (exist != null)
        {
            Entities.Remove(exist);
            return true;
        }

        return false;
    }

    public bool Delete(T entityToDelete)
    {
        if (entityToDelete == null)
            return false;

        Entities.Remove(entityToDelete);
        return true;

    }

    public void Attach(T entity) => Context.Attach(entity);

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }

    public void CreateTransaction()
    {
        Context.Database.BeginTransaction();
    }
    public void Commit()
    {
        Context.Database.CommitTransaction();
    }
    public void Rollback()
    {
        Context.Database.RollbackTransaction();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            Context.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}