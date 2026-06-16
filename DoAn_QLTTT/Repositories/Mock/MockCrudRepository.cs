namespace DoAn_QLTTT.Repositories.Mock;

public abstract class MockCrudRepository<TEntity> : ICrudRepository<TEntity, int>
{
    protected readonly MockDataService Data;

    protected MockCrudRepository(MockDataService data)
    {
        Data = data;
    }

    protected abstract List<TEntity> Items { get; }
    protected abstract int GetId(TEntity entity);
    protected abstract void SetId(TEntity entity, int id);
    protected abstract bool Matches(TEntity entity, string keyword);

    public Task<IReadOnlyList<TEntity>> GetAllAsync(string? keyword = null)
    {
        IEnumerable<TEntity> query = Items;
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(item => Matches(item, keyword.Trim()));
        }

        return Task.FromResult<IReadOnlyList<TEntity>>(query.ToList());
    }

    public Task<TEntity?> GetByIdAsync(int id)
    {
        return Task.FromResult(Items.FirstOrDefault(item => GetId(item) == id));
    }

    public Task<int> AddAsync(TEntity entity)
    {
        if (GetId(entity) == 0)
        {
            SetId(entity, Items.Count == 0 ? 1 : Items.Max(GetId) + 1);
        }

        Items.Add(entity);
        Data.RebuildLookups();
        return Task.FromResult(GetId(entity));
    }

    public Task<int> UpdateAsync(TEntity entity)
    {
        var index = Items.FindIndex(item => GetId(item) == GetId(entity));
        if (index < 0)
        {
            return Task.FromResult(0);
        }

        Items[index] = entity;
        Data.RebuildLookups();
        return Task.FromResult(1);
    }

    public Task<int> DeleteAsync(int id)
    {
        var current = Items.FirstOrDefault(item => GetId(item) == id);
        if (current is null)
        {
            return Task.FromResult(0);
        }

        Items.Remove(current);
        Data.RebuildLookups();
        return Task.FromResult(1);
    }
}
