using Dapper;
using DoAn_QLTTT.Data;
using DoAn_QLTTT.ViewModels;

namespace DoAn_QLTTT.Services;

public class SqlDemoService : ISqlDemoService
{
    private readonly DapperContext _context;
    private readonly ISqlScriptReader _scriptReader;
    private static readonly HashSet<string> Whitelist = SqlDemoScenarioCatalog.Items
        .Select(x => x.ExecuteName)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public SqlDemoService(DapperContext context, ISqlScriptReader scriptReader)
    {
        _context = context;
        _scriptReader = scriptReader;
    }

    public string DataProvider => "SqlServer";

    public Task<IReadOnlyList<SqlDemoScenarioSummaryViewModel>> GetScenariosAsync()
    {
        IReadOnlyList<SqlDemoScenarioSummaryViewModel> items = SqlDemoScenarioCatalog.Items
            .Select(x => new SqlDemoScenarioSummaryViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Type = x.Type
            })
            .ToList();

        return Task.FromResult(items);
    }

    public async Task<SqlDemoScenarioViewModel> GetScenarioAsync(string? id, bool execute)
    {
        var definition = SqlDemoScenarioCatalog.Find(id);
        var model = new SqlDemoScenarioViewModel
        {
            Id = definition.Id,
            Title = definition.Title,
            Type = definition.Type,
            Problem = definition.Problem,
            Note = definition.Note,
            HasExecuted = execute,
            SqlScript = await _scriptReader.ReadAsync(definition.ScriptFileName),
            ObjectScript = await _scriptReader.ReadAsync(definition.ObjectScriptFileName)
        };

        try
        {
            model.BeforeTables = await LoadPreviewTablesAsync(definition.Id);

            if (execute)
            {
                if (!Whitelist.Contains(definition.ExecuteName))
                {
                    model.ErrorMessage = "Kịch bản không nằm trong whitelist nên không được thực thi.";
                    return model;
                }

                model.OutputMessage = await ExecuteWhitelistedScenarioAsync(definition);
                model.AfterTables = await LoadPreviewTablesAsync(definition.Id);
            }
        }
        catch (Exception ex)
        {
            model.ErrorMessage = $"Chưa thể kết nối hoặc thực thi SQL Server. Kiểm tra DataProvider, connection string và object SQL thật. Chi tiết: {ex.Message}";
        }

        return model;
    }

    private async Task<IReadOnlyList<SqlDemoTableViewModel>> LoadPreviewTablesAsync(string scenarioId)
    {
        using var connection = _context.CreateConnection();
        var sql = scenarioId switch
        {
            "lap-hop-dong" => "SELECT TOP 5 MaPhong, SoPhong, TrangThai FROM PhongTro ORDER BY MaPhong; SELECT TOP 5 MaKhachThue, HoTen, SoDienThoai FROM KhachThue ORDER BY MaKhachThue; SELECT TOP 5 MaHopDong, MaPhong, MaKhachThue, TrangThai FROM HopDong ORDER BY MaHopDong DESC;",
            "trigger-trang-thai-phong" => "SELECT TOP 5 MaPhong, SoPhong, TrangThai FROM PhongTro ORDER BY MaPhong; SELECT TOP 5 MaHopDong, MaPhong, TrangThai FROM HopDong ORDER BY MaHopDong DESC;",
            "trigger-tong-tien-hoa-don" => "SELECT TOP 5 MaHoaDon, TongTien, DaThanhToan, ConLai, TrangThai FROM HoaDon ORDER BY MaHoaDon DESC; SELECT TOP 10 MaChiTiet, MaHoaDon, LoaiPhi, ThanhTien FROM ChiTietHoaDon ORDER BY MaChiTiet DESC;",
            "ghi-nhan-thanh-toan" => "SELECT TOP 5 MaHoaDon, TongTien, DaThanhToan, ConLai, TrangThai FROM HoaDon ORDER BY MaHoaDon DESC; SELECT TOP 5 MaThanhToan, MaHoaDon, SoTien, PhuongThuc FROM ThanhToan ORDER BY MaThanhToan DESC;",
            "function-tinh-cong-no" => "SELECT TOP 5 MaHoaDon, TongTien, DaThanhToan, ConLai FROM HoaDon ORDER BY MaHoaDon DESC;",
            "cursor-quet-qua-han" => "SELECT TOP 10 MaHoaDon, HanThanhToan, ConLai, TrangThai FROM HoaDon ORDER BY HanThanhToan;",
            _ => "SELECT 1 AS KetQua;"
        };

        using var grid = await connection.QueryMultipleAsync(sql);
        return scenarioId switch
        {
            "lap-hop-dong" => [await ReadTableAsync(grid, "PhongTro"), await ReadTableAsync(grid, "KhachThue"), await ReadTableAsync(grid, "HopDong")],
            "trigger-trang-thai-phong" => [await ReadTableAsync(grid, "PhongTro"), await ReadTableAsync(grid, "HopDong")],
            "trigger-tong-tien-hoa-don" => [await ReadTableAsync(grid, "HoaDon"), await ReadTableAsync(grid, "ChiTietHoaDon")],
            "ghi-nhan-thanh-toan" => [await ReadTableAsync(grid, "HoaDon"), await ReadTableAsync(grid, "ThanhToan")],
            "function-tinh-cong-no" => [await ReadTableAsync(grid, "HoaDon")],
            "cursor-quet-qua-han" => [await ReadTableAsync(grid, "HoaDon")],
            _ => [await ReadTableAsync(grid, "KetQua")]
        };
    }

    private static async Task<SqlDemoTableViewModel> ReadTableAsync(SqlMapper.GridReader grid, string title)
    {
        var rows = (await grid.ReadAsync()).Cast<IDictionary<string, object?>>().ToList();
        var columns = rows.FirstOrDefault()?.Keys.ToList() ?? [];

        return new SqlDemoTableViewModel
        {
            Title = title,
            Columns = columns,
            Rows = rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(row)).ToList()
        };
    }

    private async Task<string> ExecuteWhitelistedScenarioAsync(SqlDemoScenarioDefinition definition)
    {
        using var connection = _context.CreateConnection();

        if (definition.Type.Equals("Function", StringComparison.OrdinalIgnoreCase))
        {
            var result = await connection.ExecuteScalarAsync<decimal?>("SELECT dbo.fn_TinhCongNo(@MaHoaDon)", new { MaHoaDon = "HD003" });
            return $"fn_TinhCongNo trả về {result:N0}.";
        }

        await connection.ExecuteAsync(
            definition.ExecuteName,
            BuildDemoParameters(definition.Id),
            commandType: System.Data.CommandType.StoredProcedure);

        return $"Đã gọi {definition.ExecuteName} bằng Dapper.";
    }

    private static object BuildDemoParameters(string scenarioId)
    {
        return scenarioId switch
        {
            "lap-hop-dong" => new { MaPhong = "P101", MaKhachThue = "KT001" },
            "trigger-trang-thai-phong" => new { MaPhong = "P102" },
            "trigger-tong-tien-hoa-don" => new { MaHoaDon = "HD001" },
            "ghi-nhan-thanh-toan" => new { MaHoaDon = "HD002", SoTien = 1500000 },
            "cursor-quet-qua-han" => new { NgayQuet = DateTime.Today },
            _ => new { }
        };
    }
}
