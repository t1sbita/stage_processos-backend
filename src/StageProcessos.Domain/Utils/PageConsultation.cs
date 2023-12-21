namespace StageProcessos.Domain.Utils;

public class PageConsultation<TEntity>
{
    public PageConsultation() { }

    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int NumberPage { get; set; }
    public int SizePage { get; set; }
    public IEnumerable<TEntity> List { get; set; } = Enumerable.Empty<TEntity>();
}