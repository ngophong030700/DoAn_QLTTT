namespace DoAn_QLTTT.Repositories;

public interface ICrudRepository<TEntity, TKey>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(string? keyword = null);
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<int> AddAsync(TEntity entity);
    Task<int> UpdateAsync(TEntity entity);
    Task<int> DeleteAsync(TKey id);
}
