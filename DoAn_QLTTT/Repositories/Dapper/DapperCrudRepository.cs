using System.Data;
using Dapper;
using DoAn_QLTTT.Data;

namespace DoAn_QLTTT.Repositories.Dapper;

public abstract class DapperCrudRepository<TEntity> : ICrudRepository<TEntity, int>
{
    private readonly DapperContext _context;
    private readonly string _tableName;
    private readonly string _keyColumn;
    private readonly string _insertProcedure;
    private readonly string _updateProcedure;
    private readonly string _deleteProcedure;

    protected DapperCrudRepository(
        DapperContext context,
        string tableName,
        string keyColumn,
        string insertProcedure,
        string updateProcedure,
        string deleteProcedure)
    {
        _context = context;
        _tableName = tableName;
        _keyColumn = keyColumn;
        _insertProcedure = insertProcedure;
        _updateProcedure = updateProcedure;
        _deleteProcedure = deleteProcedure;
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
        // TODO: Đổi tên stored procedure nếu database dùng tên khác.
        return await connection.ExecuteAsync(_insertProcedure, entity, commandType: CommandType.StoredProcedure);
    }

    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        using var connection = _context.CreateConnection();
        // TODO: Đổi tên stored procedure nếu database dùng tên khác.
        return await connection.ExecuteAsync(_updateProcedure, entity, commandType: CommandType.StoredProcedure);
    }

    public virtual async Task<int> DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add(_keyColumn, id);
        // TODO: Đổi tên stored procedure nếu database dùng tên khác.
        return await connection.ExecuteAsync(_deleteProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
