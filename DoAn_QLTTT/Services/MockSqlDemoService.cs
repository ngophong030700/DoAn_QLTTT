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

    public async Task<SqlDemoScenarioViewModel> GetScenarioAsync(string? id, bool execute, string? sqlScript = null)
    {
        var definition = SqlDemoScenarioCatalog.Find(id);
        var model = BuildMockScenario(definition, execute);
        var defaultSqlScript = await _scriptReader.ReadAsync(definition.ScriptFileName);
        model.SqlScript = string.IsNullOrWhiteSpace(sqlScript) ? defaultSqlScript : sqlScript;
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
            case "them-khach-thue":
                model.BeforeTables = [
                    Table("Bảng KHACHTHUE", ["MaKhach", "HoTen", "CCCD", "SoDienThoai", "DiaChi"],
                        Row(30, "Lê Văn Tám", "079188888888", "0908888888", "Hà Nội"),
                        Row(29, "Trần Thị Chín", "079177777777", "0907777777", "Đà Nẵng"))
                ];
                model.AfterTables = execute
                    ? [
                        Table("Bảng KHACHTHUE", ["MaKhach", "HoTen", "CCCD", "SoDienThoai", "DiaChi"],
                            Row(31, "Nguyễn Văn Demo", "079199999999", "0909999999", "TP.HCM"),
                            Row(30, "Lê Văn Tám", "079188888888", "0908888888", "Hà Nội"),
                            Row(29, "Trần Thị Chín", "079177777777", "0907777777", "Đà Nẵng"))
                      ]
                    : [];
                model.OutputTitle = execute ? "MaKhachMoi = 31" : null;
                model.OutputMessage = execute ? "SP_THEM_KHACHTHUE đã thêm khách mới và trả về mã khách." : null;
                break;
 
            case "lap-hop-dong":
                model.BeforeTables =
                [
                    Table("Bảng PHONGTRO", ["MaPhong", "SoPhong", "GiaThue", "SucChuaToiDa", "TrangThai"],
                        Row(3, "P103", 1800000, 2, "Trống"),
                        Row(2, "P102", 1500000, 2, "Đang thuê"),
                        Row(1, "P101", 1500000, 2, "Đang thuê")),
                    Table("Bảng KHACHTHUE", ["MaKhach", "HoTen", "CCCD", "SoDienThoai"],
                        Row(31, "Nguyễn Văn Demo", "079199999999", "0909999999"),
                        Row(30, "Lê Văn Tám", "079188888888", "0908888888")),
                    Table("Bảng HOPDONG", ["MaHopDong", "MaPhong", "MaKhachDaiDien", "NgayBatDau", "NgayKetThuc", "TienThueThang", "TrangThai"],
                        Row(20, 2, 30, new DateTime(2026, 6, 1), new DateTime(2027, 6, 1), 1500000, "Hiệu lực"))
                ];
                model.AfterTables = execute
                    ? [
                        Table("Bảng HOPDONG", ["MaHopDong", "MaPhong", "MaKhachDaiDien", "NgayBatDau", "NgayKetThuc", "TienThueThang", "TrangThai"],
                            Row(21, 3, 31, new DateTime(2026, 6, 18), new DateTime(2027, 6, 18), 1800000, "Hiệu lực"),
                            Row(20, 2, 30, new DateTime(2026, 6, 1), new DateTime(2027, 6, 1), 1500000, "Hiệu lực")),
                        Table("Bảng PHONGTRO", ["MaPhong", "SoPhong", "TrangThai"],
                            Row(3, "P103", "Đang thuê"),
                            Row(2, "P102", "Đang thuê"),
                            Row(1, "P101", "Đang thuê")),
                        Table("Bảng CHITIETHOPDONG", ["MaHopDong", "MaKhach", "LaNguoiDaiDien"],
                            Row(21, 31, true),
                            Row(20, 30, true)),
                        Table("Kết quả SQL", ["MaHopDongMoi", "SoNguoiTrongHopDong"], Row(21, 1))
                      ]
                    : [];
                model.OutputMessage = execute ? "SP_LAP_HOPDONG tạo hợp đồng, trigger đổi trạng thái phòng, function đếm 1 người trong hợp đồng." : null;
                break;
 
            case "ghi-chi-so":
                model.BeforeTables =
                [
                    Table("Bảng DICHVU", ["MaDichVu", "TenDichVu", "DonVi", "DonGia", "LoaiTinhPhi"],
                        Row(2, "Nước", "m3", 18000, "TheoChiSo"),
                        Row(1, "Điện", "kWh", 3500, "TheoChiSo")),
                    Table("Bảng CHISODIENNUOC", ["MaChiSo", "MaPhong", "TenDichVu", "Thang", "Nam", "ChiSoCu", "ChiSoMoi", "TieuThu"],
                        Row(120, 3, "Điện", 6, 2026, 210, 280, 70),
                        Row(119, 2, "Điện", 6, 2026, 150, 220, 70))
                ];
                model.AfterTables = execute
                    ? [
                        Table("Bảng CHISODIENNUOC", ["MaChiSo", "MaPhong", "TenDichVu", "Thang", "Nam", "ChiSoCu", "ChiSoMoi", "TieuThu"],
                            Row(121, 3, "Điện", 7, 2026, 280, 360, 80),
                            Row(120, 3, "Điện", 6, 2026, 210, 280, 70),
                            Row(119, 2, "Điện", 6, 2026, 150, 220, 70)),
                        Table("Kết quả SQL", ["Function", "TienDienThang"], Row("FN_TINH_TIEN_CHISO", 280000))
                      ]
                    : [];
                model.OutputMessage = execute ? "SP_GHI_CHISO_DICHVU ghi chỉ số mới, trigger giữ TieuThu = 80, function trả về 280.000." : null;
                break;
 
            case "lap-hoa-don-thang":
                model.BeforeTables =
                [
                    Table("Bảng HOPDONG", ["MaHopDong", "MaPhong", "MaKhachDaiDien", "TienThueThang", "TrangThai"],
                        Row(21, 3, 31, 1800000, "Hiệu lực"),
                        Row(20, 2, 30, 1500000, "Hiệu lực")),
                    Table("Bảng HOADON", ["MaHoaDon", "MaHopDong", "Thang", "Nam", "TongTien", "DaThanhToan", "ConLai", "TrangThai"],
                        Row(53, 20, 7, 2026, 1950000, 1950000, 0, "Đã TT"))
                ];
                model.AfterTables = execute
                    ? [
                        Table("Bảng HOADON", ["MaHoaDon", "MaHopDong", "Thang", "Nam", "TongTien", "DaThanhToan", "ConLai", "TrangThai"],
                            Row(54, 21, 7, 2026, 2310000, 0, 2310000, "Chưa TT"),
                            Row(53, 20, 7, 2026, 1950000, 1950000, 0, "Đã TT")),
                        Table("Bảng CHITIETHOADON", ["MaChiTiet", "MaHoaDon", "TenDichVu", "MoTa", "SoLuong", "DonGiaTaiThoiDiem", "ThanhTien"],
                            Row(205, 54, "Giữ Xe", "Tiền Giữ Xe tháng 07/2026", 1, 100000, 100000),
                            Row(204, 54, "Rác", "Tiền Rác tháng 07/2026", 1, 30000, 30000),
                            Row(203, 54, "Wifi", "Tiền Wifi tháng 07/2026", 1, 100000, 100000),
                            Row(202, 54, "Điện", "Tiền Điện tháng 07/2026", 80, 3500, 280000),
                            Row(201, 54, "Tiền Thuê Phòng", "Tiền phòng tháng 07/2026", 1, 1800000, 1800000))
                      ]
                    : [];
                model.OutputMessage = execute ? "SP_LAP_HOADON_THANG tạo hóa đơn, trigger cộng chi tiết thành TongTien = 2.310.000." : null;
                break;
 
            case "ghi-nhan-thanh-toan":
                model.BeforeTables =
                [
                    Table("Bảng HOADON", ["MaHoaDon", "MaHopDong", "TongTien", "DaThanhToan", "ConLai", "TrangThai"],
                        Row(54, 21, 2310000, 0, 2310000, "Chưa TT"),
                        Row(53, 20, 1950000, 1950000, 0, "Đã TT")),
                    Table("Bảng THANHTOAN", ["MaThanhToan", "MaHoaDon", "MaNguoiThu", "SoTien", "NgayThu", "HinhThuc"],
                        Row(69, 53, 1, 1950000, new DateTime(2026, 7, 5), "ChuyenKhoan"))
                ];
                model.AfterTables = execute
                    ? [
                        Table("Bảng THANHTOAN", ["MaThanhToan", "MaHoaDon", "MaNguoiThu", "SoTien", "NgayThu", "HinhThuc"],
                            Row(70, 54, 1, 1000000, new DateTime(2026, 6, 18), "ChuyenKhoan"),
                            Row(69, 53, 1, 1950000, new DateTime(2026, 7, 5), "ChuyenKhoan")),
                        Table("Bảng HOADON", ["MaHoaDon", "TongTien", "DaThanhToan", "ConLai", "TrangThai"],
                            Row(54, 2310000, 1000000, 1310000, "Một phần"),
                            Row(53, 1950000, 1950000, 0, "Đã TT"))
                      ]
                    : [];
                model.OutputMessage = execute ? "SP_GHI_NHAN_THANHTOAN thêm phiếu thu, trigger cập nhật công nợ hóa đơn." : null;
                break;
 
            case "tinh-cong-no-hop-dong":
                model.BeforeTables =
                [
                    Table("Bảng HOPDONG", ["MaHopDong", "MaPhong", "TrangThai", "TongCongNo"],
                        Row(21, 3, "Hiệu lực", 1310000),
                        Row(20, 2, "Hiệu lực", 0)),
                    Table("Bảng HOADON", ["MaHoaDon", "MaHopDong", "TongTien", "DaThanhToan", "ConLai", "TrangThai"],
                        Row(54, 21, 2310000, 1000000, 1310000, "Một phần"),
                        Row(53, 20, 1950000, 1950000, 0, "Đã TT"))
                ];
                model.AfterTables = execute
                    ? [Table("Kết quả SQL", ["Function", "MaHopDong", "TongCongNo"],
                        Row("FN_TONG_CONGNO_HOPDONG", 21, 1310000),
                        Row("FN_TONG_CONGNO_HOPDONG", 20, 0))]
                    : [];
                model.OutputMessage = execute ? "Function cộng toàn bộ ConLai của hóa đơn thuộc hợp đồng 21." : null;
                break;
 
            case "quet-hoa-don-qua-han":
                model.BeforeTables =
                [
                    Table("Bảng HOADON", ["MaHoaDon", "MaHopDong", "TongTien", "DaThanhToan", "ConLai", "HanThanhToan", "TrangThai"],
                        Row(40, 12, 3200000, 500000, 2700000, new DateTime(2026, 5, 10), "Chưa TT"),
                        Row(39, 11, 2100000, 2100000, 0, new DateTime(2026, 5, 10), "Đã TT"))
                ];
                model.AfterTables = execute
                    ? [Table("Bảng HOADON", ["MaHoaDon", "MaHopDong", "ConLai", "HanThanhToan", "TrangThai"],
                        Row(40, 12, 2700000, new DateTime(2026, 5, 10), "Quá hạn"),
                        Row(39, 11, 0, new DateTime(2026, 5, 10), "Đã TT"))]
                    : [];
                model.OutputMessage = execute ? "SP_QUET_HOADON_QUAHAN dùng cursor cur_HoaDonQuaHan để chuyển hóa đơn quá hạn sang Quá hạn." : null;
                break;
 
            case "quet-hop-dong-het-han":
                model.BeforeTables =
                [
                    Table("Bảng HOPDONG và PHONGTRO", ["MaHopDong", "MaPhong", "SoPhong", "NgayKetThuc", "TrangThai", "TongCongNo", "TrangThaiPhong"],
                        Row(18, 10, "P401", new DateTime(2026, 5, 31), "Hiệu lực", 0, "Đang thuê"),
                        Row(17, 9, "P303", new DateTime(2026, 6, 15), "Hiệu lực", 500000, "Đang thuê"))
                ];
                model.AfterTables = execute
                    ? [Table("Bảng HOPDONG và PHONGTRO", ["MaHopDong", "MaPhong", "SoPhong", "NgayKetThuc", "TrangThai", "TongCongNo", "TrangThaiPhong"],
                        Row(18, 10, "P401", new DateTime(2026, 5, 31), "Đã kết thúc", 0, "Trống"),
                        Row(17, 9, "P303", new DateTime(2026, 6, 15), "Hiệu lực", 500000, "Đang thuê"))]
                    : [];
                model.OutputMessage = execute ? "SP_QUET_HOPDONG_HETHAN kết thúc hợp đồng hết hạn không còn nợ, trigger đưa phòng về Trống." : null;
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
