using Dapper;
using DoAn_QLTTT.Data;

namespace DoAn_QLTTT.Repositories.Dapper;

public abstract class DapperCrudRepository<TEntity> : ICrudRepository<TEntity, int>
{
    private readonly DapperContext _context;
    private readonly string _tableName;
    private readonly string _keyColumn;
    private readonly string[] _insertParameters;
    private readonly string[] _updateParameters;

    protected DapperCrudRepository(
        DapperContext context,
        string tableName,
        string keyColumn,
        string[] insertParameters,
        string[] updateParameters)
    {
        _context = context;
        _tableName = tableName;
        _keyColumn = keyColumn;
        _insertParameters = insertParameters;
        _updateParameters = updateParameters;
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(string? keyword = null)
    {
        using var connection = _context.CreateConnection();
        // TODO: Thay query này bằng view/stored procedure chính thức nếu cần lọc, phân trang, join tên hiển thị.
        var result = await connection.QueryAsync<TEntity>($"SELECT * FROM {_tableName}");
        return result.ToList();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        // TODO: Thay query đọc chi tiết bằng stored procedure khi chốt thiết kế CSDL.
        return await connection.QueryFirstOrDefaultAsync<TEntity>(
            $"SELECT * FROM {_tableName} WHERE {_keyColumn} = @Id",
            new { Id = id });
    }

    public virtual async Task<int> AddAsync(TEntity entity)
    {
        using var connection = _context.CreateConnection();
        var columns = string.Join(", ", _insertParameters.Select(EscapeIdentifier));
        var values = string.Join(", ", _insertParameters.Select(name => $"@{name}"));
        var sql = $"INSERT INTO {EscapeIdentifier(_tableName)} ({columns}) VALUES ({values});";

        return await connection.ExecuteAsync(
            sql,
            BuildParameters(entity, _insertParameters));
    }

    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        using var connection = _context.CreateConnection();
        var updateColumns = _updateParameters
            .Where(name => !name.Equals(_keyColumn, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        var assignments = string.Join(
            ", ",
            updateColumns.Select(name => $"{EscapeIdentifier(name)} = @{name}"));
        var sql =
            $"UPDATE {EscapeIdentifier(_tableName)} SET {assignments} " +
            $"WHERE {EscapeIdentifier(_keyColumn)} = @{_keyColumn};";

        return await connection.ExecuteAsync(
            sql,
            BuildParameters(entity, _updateParameters));
    }

    public virtual async Task<int> DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        var sql =
            $"DELETE FROM {EscapeIdentifier(_tableName)} " +
            $"WHERE {EscapeIdentifier(_keyColumn)} = @Id;";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }

    private static DynamicParameters BuildParameters(TEntity entity, IEnumerable<string> parameterNames)
    {
        var parameters = new DynamicParameters();
        var entityType = typeof(TEntity);

        foreach (var name in parameterNames)
        {
            var property = entityType.GetProperty(name)
                ?? throw new InvalidOperationException(
                    $"Không tìm thấy thuộc tính {name} trên {entityType.Name}.");
            parameters.Add(name, property.GetValue(entity));
        }

        return parameters;
    }

    private static string EscapeIdentifier(string identifier) =>
        $"[{identifier.Replace("]", "]]", StringComparison.Ordinal)}]";
}
