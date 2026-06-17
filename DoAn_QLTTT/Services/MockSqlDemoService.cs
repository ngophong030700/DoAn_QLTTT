using DoAn_QLTTT.ViewModels;

namespace DoAn_QLTTT.Services;

public class MockSqlDemoService : ISqlDemoService
{
    private readonly ISqlScriptReader _scriptReader;

    public MockSqlDemoService(ISqlScriptReader scriptReader)
    {
        _scriptReader = scriptReader;
    }

    public string DataProvider => "Mock";

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
        var model = BuildMockScenario(definition, execute);
        model.SqlScript = await _scriptReader.ReadAsync(definition.ScriptFileName);
        model.ObjectScript = await _scriptReader.ReadAsync(definition.ObjectScriptFileName);
        return model;
    }

    private static SqlDemoScenarioViewModel BuildMockScenario(SqlDemoScenarioDefinition definition, bool execute)
    {
        var model = new SqlDemoScenarioViewModel
        {
            Id = definition.Id,
            Title = definition.Title,
            Type = definition.Type,
            Problem = definition.Problem,
            Note = definition.Note,
            HasExecuted = execute
        };

        switch (definition.Id)
        {
            case "lap-hop-dong":
                model.BeforeTables =
                [
                    Table("PhongTro", ["MaPhong", "SoPhong", "TrangThai"], Row("P101", "P101", "Trống")),
                    Table("KhachThue", ["MaKhachThue", "HoTen", "SoDienThoai"], Row("KT001", "Nguyễn Văn A", "0901000001")),
                    Table("HopDong", ["MaHopDong", "MaPhong", "MaKhachThue", "TrangThai"])
                ];
                model.AfterTables = execute
                    ? [
                        Table("PhongTro", ["MaPhong", "SoPhong", "TrangThai"], Row("P101", "P101", "Đang thuê")),
                        Table("HopDong", ["MaHopDong", "MaPhong", "MaKhachThue", "NgayLap", "TrangThai"], Row("HDG001", "P101", "KT001", "16/06/2026", "Hiệu lực"))
                    ]
                    : [];
                model.OutputMessage = execute ? "Đã giả lập thêm hợp đồng mới và cập nhật trạng thái phòng P101." : null;
                break;

            case "trigger-trang-thai-phong":
                model.BeforeTables =
                [
                    Table("PhongTro", ["MaPhong", "SoPhong", "TrangThai"], Row("P102", "P102", "Trống"))
                ];
                model.AfterTables = execute
                    ? [
                        Table("HopDong", ["MaHopDong", "MaPhong", "TrangThai"], Row("HDG002", "P102", "Hiệu lực")),
                        Table("PhongTro", ["MaPhong", "SoPhong", "TrangThai"], Row("P102", "P102", "Đang thuê"))
                    ]
                    : [];
                model.OutputMessage = execute ? "Trigger được giả lập: INSERT HopDong làm PhongTro chuyển sang Đang thuê." : null;
                break;

            case "trigger-tong-tien-hoa-don":
                model.BeforeTables =
                [
                    Table("HoaDon", ["MaHoaDon", "TongTien", "DaThanhToan", "ConLai", "TrangThai"], Row("HD001", 0, 0, 0, "Chưa TT")),
                    Table("ChiTietHoaDon", ["MaChiTiet", "MaHoaDon", "LoaiPhi", "ThanhTien"])
                ];
                model.AfterTables = execute
                    ? [
                        Table("ChiTietHoaDon", ["MaChiTiet", "MaHoaDon", "LoaiPhi", "ThanhTien"],
                            Row("CT001", "HD001", "Tiền phòng", 2500000),
                            Row("CT002", "HD001", "Tiền điện", 180000),
                            Row("CT003", "HD001", "Tiền nước", 90000),
                            Row("CT004", "HD001", "Wifi", 100000)),
                        Table("HoaDon", ["MaHoaDon", "TongTien", "DaThanhToan", "ConLai", "TrangThai"], Row("HD001", 2870000, 0, 2870000, "Chưa TT"))
                    ]
                    : [];
                model.OutputMessage = execute ? "Đã giả lập roll-up TongTien và ConLai từ các dòng chi tiết hóa đơn." : null;
                break;

            case "ghi-nhan-thanh-toan":
                model.BeforeTables =
                [
                    Table("HoaDon", ["MaHoaDon", "TongTien", "DaThanhToan", "ConLai", "TrangThai"], Row("HD002", 3000000, 500000, 2500000, "Chưa TT"))
                ];
                model.AfterTables = execute
                    ? [
                        Table("ThanhToan", ["MaThanhToan", "MaHoaDon", "SoTien", "PhuongThuc"], Row("TT001", "HD002", 1500000, "Chuyển khoản")),
                        Table("HoaDon", ["MaHoaDon", "TongTien", "DaThanhToan", "ConLai", "TrangThai"], Row("HD002", 3000000, 2000000, 1000000, "Một phần"))
                    ]
                    : [];
                model.OutputMessage = execute ? "Đã giả lập ghi nhận thanh toán 1.500.000 cho hóa đơn HD002." : null;
                break;

            case "function-tinh-cong-no":
                model.BeforeTables =
                [
                    Table("HoaDon", ["MaHoaDon", "TongTien", "DaThanhToan", "ConLai"], Row("HD003", 3200000, 1200000, 2000000))
                ];
                model.AfterTables = execute
                    ? [
                        Table("Output", ["TenFunction", "MaHoaDon", "KetQuaFunction"], Row("fn_TinhCongNo", "HD003", 2000000))
                    ]
                    : [];
                model.OutputTitle = execute ? "KetQuaFunction = TongTien - DaThanhToan" : null;
                model.OutputMessage = execute ? "fn_TinhCongNo(HD003) trả về 2.000.000." : null;
                break;

            case "cursor-quet-qua-han":
                model.BeforeTables =
                [
                    Table("HoaDon", ["MaHoaDon", "HanThanhToan", "ConLai", "TrangThai"], Row("HD004", "10/06/2026", 1800000, "Chưa TT"))
                ];
                model.AfterTables = execute
                    ? [
                        Table("HoaDon", ["MaHoaDon", "HanThanhToan", "ConLai", "TrangThai"], Row("HD004", "10/06/2026", 1800000, "Quá hạn"))
                    ]
                    : [];
                model.OutputMessage = execute ? "Đã giả lập cursor quét hóa đơn quá hạn và cập nhật trạng thái HD004." : null;
                break;
        }

        return model;
    }

    private static SqlDemoTableViewModel Table(string title, IReadOnlyList<string> columns, params object?[][] rows)
    {
        return new SqlDemoTableViewModel
        {
            Title = title,
            Columns = columns,
            Rows = rows.Select(values => columns
                .Select((column, index) => new { column, value = index < values.Length ? values[index] : null })
                .ToDictionary(x => x.column, x => x.value))
                .ToList()
        };
    }

    private static object?[] Row(params object?[] values) => values;
}
