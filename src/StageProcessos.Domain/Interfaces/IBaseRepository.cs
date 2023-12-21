using StageProcessos.Domain.Entities.Base;
using StageProcessos.Domain.Utils;
using System.Linq.Expressions;

namespace StageProcessos.Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(long Id, params Expression<Func<T, object>>[] includes);
    Task<T> GetAsync(Expression<Func<T, bool>> filters, params Expression<Func<T, object>>[] includes);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filters = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> sortedBy = null,
        params Expression<Func<T, object>>[] includes);
    Task<PageConsultation<T>> GetAllPagedAsync(int page, int itemsByPage,
        ICollection<Expression<Func<T, bool>>> filters = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> sortedBy = null,
        params Expression<Func<T, object>>[] includes);
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> filters = null,
        params Expression<Func<T, object>>[] includes);
    Task<T> AddAsync(T Entity);
    Task AddRangeAsync(IEnumerable<T> Entity);
    Task<T> UpdateAsync(T Entity);
    Task<bool> DeleteAsync(long Id);
    bool Delete(T entityToDelete);
    void Attach(T entity);
    Task SaveAsync();
    void CreateTransaction();
    void Commit();
    void Rollback();
}
