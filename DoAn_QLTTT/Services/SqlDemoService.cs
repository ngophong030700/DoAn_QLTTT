using Dapper;
using DoAn_QLTTT.Data;
using DoAn_QLTTT.ViewModels;
using System.Data;

namespace DoAn_QLTTT.Services;

public class SqlDemoService : ISqlDemoService
{
    private const string DemoCccd = "079199999999";
    private const int DemoMonth = 7;
    private const int DemoYear = 2026;

    private readonly DapperContext _context;
    private readonly ISqlScriptReader _scriptReader;
    public SqlDemoService(DapperContext context, ISqlScriptReader scriptReader)
    {
        _context = context;
        _scriptReader = scriptReader;
    }

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

    public async Task<SqlDemoScenarioViewModel> GetScenarioAsync(
        string? id,
        bool execute,
        string? sqlScript = null,
        int? preferredCustomerId = null,
        int? currentUserId = null)
    {
        var definition = SqlDemoScenarioCatalog.Find(id);
        var defaultSqlScript = await _scriptReader.ReadAsync(definition.ScriptFileName);
        var effectiveSqlScript = string.IsNullOrWhiteSpace(sqlScript) ? defaultSqlScript : sqlScript;
        var model = new SqlDemoScenarioViewModel
        {
            Id = definition.Id,
            Title = definition.Title,
            Type = definition.Type,
            Problem = definition.Problem,
            Note = definition.Note,
            HasExecuted = execute,
            SqlScript = effectiveSqlScript,
            PreferredCustomerId = preferredCustomerId,
            CurrentUserId = currentUserId ?? 1
        };

        try
        {
            await LoadFormOptionsAsync(model);
            model.BeforeTables = await LoadPreviewTablesAsync(definition.Id);

            if (execute)
            {
                var outputTables = await ExecuteSubmittedScriptAsync(effectiveSqlScript);
                model.CreatedCustomerId = TryGetInt(outputTables, "MaKhachMoi");
                model.DebtAmount = TryGetDecimal(outputTables, "TongCongNo");
                model.OutputTitle = "Đã chạy câu SQL trên màn hình";
                model.OutputMessage = outputTables.Count == 0
                    ? "Script đã thực thi thành công. Không có result set SELECT trả về."
                    : $"Script đã thực thi thành công và trả về {outputTables.Count} result set.";
                model.AfterTables = outputTables.Count > 0
                    ? outputTables
                    : await LoadPreviewTablesAsync(definition.Id);
            }
        }
        catch (Exception ex)
        {
            model.ErrorMessage = $"Lỗi thực thi script: {ex.Message}";
        }

        return model;
    }

    public async Task<int> GetPreviousReadingAsync(int roomId, int serviceId, int month, int year)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<int?>(
            """
            SELECT TOP 1 ChiSoMoi
            FROM CHISODIENNUOC
            WHERE MaPhong = @RoomId
              AND MaDichVu = @ServiceId
              AND DATEFROMPARTS(Nam, Thang, 1) < DATEFROMPARTS(@Year, @Month, 1)
            ORDER BY Nam DESC, Thang DESC, MaChiSo DESC;
            """,
            new { RoomId = roomId, ServiceId = serviceId, Month = month, Year = year }) ?? 0;
    }

    private async Task LoadFormOptionsAsync(SqlDemoScenarioViewModel model)
    {
        using var connection = _context.CreateConnection();

        switch (model.Id)
        {
            case "lap-hop-dong":
                model.RoomOptions = (await connection.QueryAsync<SqlDemoOptionViewModel>(
                    """
                    SELECT
                        MaPhong AS Value,
                        CONCAT(N'Phòng ', SoPhong, N' - ', FORMAT(GiaThue, 'N0'), N' đ/tháng') AS Label
                    FROM PHONGTRO
                    WHERE LTRIM(RTRIM(TrangThai)) IN (N'Trống', N'Trong')
                    ORDER BY SoPhong;
                    """)).ToList();
                model.CustomerOptions = (await connection.QueryAsync<SqlDemoOptionViewModel>(
                    """
                    SELECT
                        MaKhach AS Value,
                        CONCAT(HoTen, N' - CCCD: ', CCCD) AS Label
                    FROM KHACHTHUE
                    ORDER BY MaKhach DESC;
                    """)).ToList();
                break;

            case "ghi-chi-so":
                model.RoomOptions = (await connection.QueryAsync<SqlDemoOptionViewModel>(
                    """
                    SELECT
                        MaPhong AS Value,
                        CONCAT(N'Phòng ', SoPhong) AS Label
                    FROM PHONGTRO
                    WHERE LTRIM(RTRIM(TrangThai)) IN (N'Đang thuê', N'Dang thue')
                    ORDER BY SoPhong;
                    """)).ToList();
                model.ServiceOptions = (await connection.QueryAsync<SqlDemoOptionViewModel>(
                    """
                    SELECT
                        MaDichVu AS Value,
                        CONCAT(TenDichVu, N' (', DonVi, N')') AS Label
                    FROM DICHVU
                    WHERE LoaiTinhPhi IN (N'TheoChiSo', N'Theo chỉ số', N'Theo chi so')
                       OR TenDichVu IN (N'Điện', N'Nước', N'Dien', N'Nuoc')
                    ORDER BY TenDichVu;
                    """)).ToList();
                break;

            case "lap-hoa-don-thang":
                model.ContractOptions = (await connection.QueryAsync<SqlDemoOptionViewModel>(
                    """
                    SELECT
                        HD.MaHopDong AS Value,
                        CONCAT(N'HĐ #', HD.MaHopDong, N' - Phòng ', P.SoPhong, N' - ', KT.HoTen) AS Label
                    FROM HOPDONG HD
                    INNER JOIN PHONGTRO P ON P.MaPhong = HD.MaPhong
                    INNER JOIN KHACHTHUE KT ON KT.MaKhach = HD.MaKhachDaiDien
                    WHERE LTRIM(RTRIM(HD.TrangThai)) IN (N'Hiệu lực', N'Hieu luc')
                    ORDER BY HD.MaHopDong DESC;
                    """)).ToList();
                break;

            case "ghi-nhan-thanh-toan":
                model.InvoiceOptions = (await connection.QueryAsync<SqlDemoOptionViewModel>(
                    """
                    SELECT
                        HD.MaHoaDon AS Value,
                        CONCAT(N'Hóa đơn #', HD.MaHoaDon, N' - Phòng ', P.SoPhong, N' - Còn nợ ', FORMAT(HD.ConLai, 'N0'), N' đ') AS Label,
                        HD.ConLai AS Amount
                    FROM HOADON HD
                    INNER JOIN HOPDONG HDG ON HDG.MaHopDong = HD.MaHopDong
                    INNER JOIN PHONGTRO P ON P.MaPhong = HDG.MaPhong
                    WHERE HD.ConLai > 0
                    ORDER BY HD.HanThanhToan, HD.MaHoaDon DESC;
                    """)).ToList();
                break;

            case "tinh-cong-no-hop-dong":
                model.ContractOptions = (await connection.QueryAsync<SqlDemoOptionViewModel>(
                    """
                    SELECT
                        HD.MaHopDong AS Value,
                        CONCAT(N'HĐ #', HD.MaHopDong, N' - Phòng ', P.SoPhong, N' - ', KT.HoTen) AS Label
                    FROM HOPDONG HD
                    INNER JOIN PHONGTRO P ON P.MaPhong = HD.MaPhong
                    INNER JOIN KHACHTHUE KT ON KT.MaKhach = HD.MaKhachDaiDien
                    ORDER BY HD.MaHopDong DESC;
                    """)).ToList();
                break;
        }
    }

    private static int? TryGetInt(IReadOnlyList<SqlDemoTableViewModel> tables, string column)
    {
        var value = TryGetValue(tables, column);
        return value is null ? null : Convert.ToInt32(value);
    }

    private static decimal? TryGetDecimal(IReadOnlyList<SqlDemoTableViewModel> tables, string column)
    {
        var value = TryGetValue(tables, column);
        return value is null ? null : Convert.ToDecimal(value);
    }

    private static object? TryGetValue(IReadOnlyList<SqlDemoTableViewModel> tables, string column)
    {
        foreach (var row in tables.SelectMany(x => x.Rows))
        {
            var match = row.FirstOrDefault(x => x.Key.Equals(column, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(match.Key) && match.Value is not DBNull)
            {
                return match.Value;
            }
        }

        return null;
    }

    private static readonly System.Collections.Generic.HashSet<string> KnownTables = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "KHACHTHUE", "PHONGTRO", "HOPDONG", "CHITIETHOPDONG", "DICHVU", 
        "CHISODIENNUOC", "HOADON", "CHITIETHOADON", "THANHTOAN", "NGUOIDUNG", "LOAIPHONG"
    };

    private static readonly System.Text.RegularExpressions.Regex TableExtractionRegex = new(
        @"\b(?:FROM|JOIN)\s+(?:\[?(?:dbo|\w+)\]?\.+)?\[?([a-zA-Z_][a-zA-Z0-9_]*)\]?", 
        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled
    );

    private static string DetermineTitleFromSql(string sqlScript, string defaultTitle)
    {
        if (string.IsNullOrWhiteSpace(sqlScript)) return defaultTitle;

        var matches = TableExtractionRegex.Matches(sqlScript);
        var detectedTables = new System.Collections.Generic.List<string>();

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            if (match.Groups.Count > 1)
            {
                string tableName = match.Groups[1].Value.ToUpper();
                if (KnownTables.Contains(tableName) && !detectedTables.Contains(tableName))
                {
                    detectedTables.Add(tableName);
                }
            }
        }

        if (detectedTables.Count > 0)
        {
            return $"Bảng: {string.Join(", ", detectedTables)}";
        }

        return defaultTitle;
    }

    private async Task<IReadOnlyList<SqlDemoTableViewModel>> ExecuteSubmittedScriptAsync(string sqlScript)
    {
        var batches = SplitSqlBatches(sqlScript).ToList();
        if (batches.Count == 0)
        {
            return [];
        }

        var tables = new List<SqlDemoTableViewModel>();
        using var connection = _context.CreateConnection();

        for (var batchIndex = 0; batchIndex < batches.Count; batchIndex++)
        {
            using var grid = await connection.QueryMultipleAsync(batches[batchIndex]);
            var resultIndex = 1;

            while (!grid.IsConsumed)
            {
                var rows = (await grid.ReadAsync()).Cast<IDictionary<string, object?>>().ToList();
                var columns = rows.FirstOrDefault()?.Keys.ToList() ?? [];

                var defaultTitle = batches.Count == 1
                    ? $"Kết quả SQL #{resultIndex}"
                    : $"Kết quả SQL batch {batchIndex + 1}.{resultIndex}";

                var dynamicTitle = DetermineTitleFromSql(batches[batchIndex], defaultTitle);

                tables.Add(new SqlDemoTableViewModel
                {
                    Title = dynamicTitle,
                    Columns = columns,
                    Rows = rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(row)).ToList()
                });

                resultIndex++;
            }
        }

        return tables;
    }

    private static IEnumerable<string> SplitSqlBatches(string sqlScript)
    {
        var lines = sqlScript.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var current = new List<string>();

        foreach (var line in lines)
        {
            if (line.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
            {
                var batch = string.Join(Environment.NewLine, current).Trim();
                if (!string.IsNullOrWhiteSpace(batch))
                {
                    yield return batch;
                }

                current.Clear();
                continue;
            }

            current.Add(line);
        }

        var lastBatch = string.Join(Environment.NewLine, current).Trim();
        if (!string.IsNullOrWhiteSpace(lastBatch))
        {
            yield return lastBatch;
        }
    }

    private async Task<IReadOnlyList<SqlDemoTableViewModel>> LoadPreviewTablesAsync(string scenarioId)
    {
        using var connection = _context.CreateConnection();
        var sql = scenarioId switch
        {
            "them-khach-thue" => "SELECT TOP 10 MaKhach, HoTen, CCCD, SoDienThoai, DiaChi FROM KHACHTHUE ORDER BY MaKhach DESC;",
            "lap-hop-dong" => """
                SELECT TOP 10 MaPhong, SoPhong, GiaThue, SucChuaToiDa, TrangThai FROM PHONGTRO ORDER BY MaPhong DESC;
                SELECT TOP 10 MaKhach, HoTen, CCCD, SoDienThoai, DiaChi FROM KHACHTHUE ORDER BY MaKhach DESC;
                SELECT TOP 10 MaHopDong, MaPhong, MaKhachDaiDien, NgayBatDau, NgayKetThuc, TienThueThang, TrangThai FROM HOPDONG ORDER BY MaHopDong DESC;
                SELECT TOP 10 MaHopDong, MaKhach, LaNguoiDaiDien FROM CHITIETHOPDONG ORDER BY MaHopDong DESC;
                """,
            "ghi-chi-so" => """
                SELECT TOP 10 MaDichVu, TenDichVu, DonVi, DonGia, LoaiTinhPhi FROM DICHVU ORDER BY MaDichVu DESC;
                SELECT TOP 10 CS.MaChiSo, CS.MaPhong, DV.TenDichVu, CS.Thang, CS.Nam, CS.ChiSoCu, CS.ChiSoMoi, CS.TieuThu
                FROM CHISODIENNUOC CS
                INNER JOIN DICHVU DV ON CS.MaDichVu = DV.MaDichVu
                ORDER BY CS.MaChiSo DESC;
                """,
            "lap-hoa-don-thang" => """
                SELECT TOP 10 MaHopDong, MaPhong, MaKhachDaiDien, TienThueThang, TrangThai FROM HOPDONG ORDER BY MaHopDong DESC;
                SELECT TOP 10 MaHoaDon, MaHopDong, Thang, Nam, TongTien, DaThanhToan, ConLai, HanThanhToan, TrangThai FROM HOADON ORDER BY MaHoaDon DESC;
                SELECT TOP 15 CT.MaChiTiet, CT.MaHoaDon, DV.TenDichVu, CT.MoTa, CT.SoLuong, CT.DonGiaTaiThoiDiem, CT.ThanhTien
                FROM CHITIETHOADON CT
                INNER JOIN DICHVU DV ON CT.MaDichVu = DV.MaDichVu
                ORDER BY CT.MaChiTiet DESC;
                """,
            "ghi-nhan-thanh-toan" => """
                SELECT TOP 10 MaHoaDon, MaHopDong, TongTien, DaThanhToan, ConLai, HanThanhToan, TrangThai FROM HOADON ORDER BY MaHoaDon DESC;
                SELECT TOP 10 MaThanhToan, MaHoaDon, MaNguoiThu, SoTien, NgayThu, HinhThuc FROM THANHTOAN ORDER BY MaThanhToan DESC;
                """,
            "tinh-cong-no-hop-dong" => """
                SELECT TOP 10 HD.MaHopDong, HD.MaPhong, HD.TrangThai, dbo.FN_TONG_CONGNO_HOPDONG(HD.MaHopDong) AS TongCongNo FROM HOPDONG HD ORDER BY HD.MaHopDong DESC;
                SELECT TOP 10 MaHoaDon, MaHopDong, TongTien, DaThanhToan, ConLai, TrangThai FROM HOADON ORDER BY MaHoaDon DESC;
                """,
            "quet-hoa-don-qua-han" => "SELECT TOP 10 MaHoaDon, MaHopDong, TongTien, DaThanhToan, ConLai, HanThanhToan, TrangThai FROM HOADON ORDER BY MaHoaDon DESC;",
            "quet-hop-dong-het-han" => """
                SELECT TOP 10 HD.MaHopDong, HD.MaPhong, P.SoPhong, HD.NgayKetThuc, HD.TrangThai, dbo.FN_TONG_CONGNO_HOPDONG(HD.MaHopDong) AS TongCongNo, P.TrangThai AS TrangThaiPhong
                FROM HOPDONG HD
                INNER JOIN PHONGTRO P ON HD.MaPhong = P.MaPhong
                ORDER BY HD.MaHopDong DESC;
                """,
            _ => "SELECT 1 AS KetQua;"
        };

        using var grid = await connection.QueryMultipleAsync(sql);
        return scenarioId switch
        {
            "them-khach-thue" => [await ReadTableAsync(grid, "Bảng KHACHTHUE")],
            "lap-hop-dong" => [await ReadTableAsync(grid, "Bảng PHONGTRO"), await ReadTableAsync(grid, "Bảng KHACHTHUE"), await ReadTableAsync(grid, "Bảng HOPDONG"), await ReadTableAsync(grid, "Bảng CHITIETHOPDONG")],
            "ghi-chi-so" => [await ReadTableAsync(grid, "Bảng DICHVU"), await ReadTableAsync(grid, "Bảng CHISODIENNUOC")],
            "lap-hoa-don-thang" => [await ReadTableAsync(grid, "Bảng HOPDONG"), await ReadTableAsync(grid, "Bảng HOADON"), await ReadTableAsync(grid, "Bảng CHITIETHOADON")],
            "ghi-nhan-thanh-toan" => [await ReadTableAsync(grid, "Bảng HOADON"), await ReadTableAsync(grid, "Bảng THANHTOAN")],
            "tinh-cong-no-hop-dong" => [await ReadTableAsync(grid, "Bảng HOPDONG"), await ReadTableAsync(grid, "Bảng HOADON")],
            "quet-hoa-don-qua-han" => [await ReadTableAsync(grid, "Bảng HOADON")],
            "quet-hop-dong-het-han" => [await ReadTableAsync(grid, "Bảng HOPDONG và PHONGTRO")],
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

}
