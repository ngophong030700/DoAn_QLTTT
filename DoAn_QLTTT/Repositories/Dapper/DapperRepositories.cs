using Dapper;
using DoAn_QLTTT.Data;
using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Dapper;

public class LoaiPhongDapperRepository(DapperContext context)
    : DapperCrudRepository<LoaiPhong>(
        context, "LoaiPhong", "MaLoaiPhong",
        ["TenLoai", "DienTich", "GiaThueCoSo", "SucChuaToiDa"],
        ["MaLoaiPhong", "TenLoai", "DienTich", "GiaThueCoSo", "SucChuaToiDa"]), ILoaiPhongRepository;

public class PhongTroDapperRepository : DapperCrudRepository<PhongTro>, IPhongTroRepository
{
    private const string SelectSql = """
        SELECT PT.*, LP.TenLoai AS LoaiPhongTen
        FROM PHONGTRO PT
        LEFT JOIN LOAIPHONG LP ON LP.MaLoaiPhong = PT.MaLoaiPhong
        """;

    private readonly DapperContext _context;

    public PhongTroDapperRepository(DapperContext context)
        : base(
            context, "PhongTro", "MaPhong",
            ["MaLoaiPhong", "SoPhong", "GiaThue", "SucChuaToiDa", "TrangThai", "GhiChu"],
            ["MaPhong", "MaLoaiPhong", "SoPhong", "GiaThue", "SucChuaToiDa", "TrangThai", "GhiChu"])
    {
        _context = context;
    }

    public override async Task<IReadOnlyList<PhongTro>> GetAllAsync(string? keyword = null)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<PhongTro>(
            SelectSql + """

            WHERE @Keyword IS NULL
               OR PT.SoPhong LIKE N'%' + @Keyword + N'%'
               OR LP.TenLoai LIKE N'%' + @Keyword + N'%'
               OR PT.TrangThai LIKE N'%' + @Keyword + N'%'
            ORDER BY PT.SoPhong;
            """,
            new { Keyword = NormalizeKeyword(keyword) });
        return result.ToList();
    }

    public override async Task<PhongTro?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<PhongTro>(
            SelectSql + "\nWHERE PT.MaPhong = @Id;",
            new { Id = id });
    }

    private static string? NormalizeKeyword(string? keyword) =>
        string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim();
}

public class KhachThueDapperRepository(DapperContext context)
    : DapperCrudRepository<KhachThue>(
        context, "KhachThue", "MaKhach",
        ["HoTen", "CCCD", "SoDienThoai", "DiaChi"],
        ["MaKhach", "HoTen", "CCCD", "SoDienThoai", "DiaChi"]), IKhachThueRepository;

public class HopDongDapperRepository : DapperCrudRepository<HopDong>, IHopDongRepository
{
    private const string SelectSql = """
        SELECT
            HD.*,
            P.SoPhong,
            KT.HoTen AS KhachDaiDienHoTen
        FROM HOPDONG HD
        LEFT JOIN PHONGTRO P ON P.MaPhong = HD.MaPhong
        LEFT JOIN KHACHTHUE KT ON KT.MaKhach = HD.MaKhachDaiDien
        """;

    private readonly DapperContext _context;

    public HopDongDapperRepository(DapperContext context)
        : base(
            context, "HopDong", "MaHopDong",
            ["MaPhong", "MaKhachDaiDien", "NgayBatDau", "NgayKetThuc", "TienThueThang", "TienCoc", "TrangThai"],
            ["MaHopDong", "MaPhong", "MaKhachDaiDien", "NgayBatDau", "NgayKetThuc", "TienThueThang", "TienCoc", "TrangThai"])
    {
        _context = context;
    }

    public override async Task<IReadOnlyList<HopDong>> GetAllAsync(string? keyword = null)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<HopDong>(
            SelectSql + """

            WHERE @Keyword IS NULL
               OR CAST(HD.MaHopDong AS varchar(20)) LIKE '%' + @Keyword + '%'
               OR P.SoPhong LIKE N'%' + @Keyword + N'%'
               OR KT.HoTen LIKE N'%' + @Keyword + N'%'
               OR HD.TrangThai LIKE N'%' + @Keyword + N'%'
            ORDER BY HD.MaHopDong DESC;
            """,
            new { Keyword = NormalizeKeyword(keyword) });
        return result.ToList();
    }

    public override async Task<HopDong?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<HopDong>(
            SelectSql + "\nWHERE HD.MaHopDong = @Id;",
            new { Id = id });
    }

    private static string? NormalizeKeyword(string? keyword) =>
        string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim();
}

public class DichVuDapperRepository(DapperContext context)
    : DapperCrudRepository<DichVu>(
        context, "DichVu", "MaDichVu",
        ["TenDichVu", "DonVi", "DonGia", "LoaiTinhPhi"],
        ["MaDichVu", "TenDichVu", "DonVi", "DonGia", "LoaiTinhPhi"]), IDichVuRepository;

public class ChiSoDienNuocDapperRepository : DapperCrudRepository<ChiSoDienNuoc>, IChiSoDienNuocRepository
{
    private const string SelectSql = """
        SELECT
            CS.*,
            P.SoPhong,
            DV.TenDichVu AS DichVuTen,
            ND.HoTen AS NguoiNhapHoTen
        FROM CHISODIENNUOC CS
        LEFT JOIN PHONGTRO P ON P.MaPhong = CS.MaPhong
        LEFT JOIN DICHVU DV ON DV.MaDichVu = CS.MaDichVu
        LEFT JOIN NGUOIDUNG ND ON ND.MaNguoiDung = CS.MaNguoiNhap
        """;

    private readonly DapperContext _context;

    public ChiSoDienNuocDapperRepository(DapperContext context)
        : base(
            context, "ChiSoDienNuoc", "MaChiSo",
            ["MaPhong", "MaDichVu", "MaNguoiNhap", "Thang", "Nam", "ChiSoCu", "ChiSoMoi", "TieuThu"],
            ["MaChiSo", "MaPhong", "MaDichVu", "MaNguoiNhap", "Thang", "Nam", "ChiSoCu", "ChiSoMoi", "TieuThu"])
    {
        _context = context;
    }

    public override async Task<IReadOnlyList<ChiSoDienNuoc>> GetAllAsync(string? keyword = null)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<ChiSoDienNuoc>(
            SelectSql + """

            WHERE @Keyword IS NULL
               OR P.SoPhong LIKE N'%' + @Keyword + N'%'
               OR DV.TenDichVu LIKE N'%' + @Keyword + N'%'
               OR ND.HoTen LIKE N'%' + @Keyword + N'%'
               OR CONCAT(CS.Thang, '/', CS.Nam) LIKE '%' + @Keyword + '%'
            ORDER BY CS.Nam DESC, CS.Thang DESC, CS.MaChiSo DESC;
            """,
            new { Keyword = NormalizeKeyword(keyword) });
        return result.ToList();
    }

    public override async Task<ChiSoDienNuoc?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ChiSoDienNuoc>(
            SelectSql + "\nWHERE CS.MaChiSo = @Id;",
            new { Id = id });
    }

    private static string? NormalizeKeyword(string? keyword) =>
        string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim();
}

public class NguoiDungDapperRepository : DapperCrudRepository<NguoiDung>, INguoiDungRepository
{
    private readonly DapperContext _context;

    public NguoiDungDapperRepository(DapperContext context)
        : base(
            context, "NguoiDung", "MaNguoiDung",
            ["TenDangNhap", "MatKhau", "HoTen", "VaiTro", "DangHoatDong"],
            ["MaNguoiDung", "TenDangNhap", "MatKhau", "HoTen", "VaiTro", "DangHoatDong"])
    {
        _context = context;
    }

    public async Task<NguoiDung?> AuthenticateAsync(string tenDangNhap, string matKhau)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<NguoiDung>(
            """
            SELECT TOP 1 MaNguoiDung, TenDangNhap, MatKhau, HoTen, VaiTro, DangHoatDong
            FROM NGUOIDUNG
            WHERE TenDangNhap = @TenDangNhap
              AND MatKhau = @MatKhau
              AND DangHoatDong = 1;
            """,
            new { TenDangNhap = tenDangNhap, MatKhau = matKhau });
    }
}

public class HoaDonDapperRepository : DapperCrudRepository<HoaDon>, IHoaDonRepository
{
    private const string HoaDonSelect = """
        SELECT
            HD.*,
            P.SoPhong,
            KT.HoTen AS KhachDaiDienHoTen,
            ND.HoTen AS NguoiLapHoTen,
            CONCAT(N'HĐ #', HD.MaHopDong, N' - Phòng ', COALESCE(P.SoPhong, N'—')) AS HopDongMoTa
        FROM HOADON HD
        LEFT JOIN HOPDONG HDG ON HDG.MaHopDong = HD.MaHopDong
        LEFT JOIN PHONGTRO P ON P.MaPhong = HDG.MaPhong
        LEFT JOIN KHACHTHUE KT ON KT.MaKhach = HDG.MaKhachDaiDien
        LEFT JOIN NGUOIDUNG ND ON ND.MaNguoiDung = HD.MaNguoiLap
        """;

    private readonly DapperContext _context;

    public HoaDonDapperRepository(DapperContext context)
        : base(
            context, "HoaDon", "MaHoaDon",
            ["MaHopDong", "MaNguoiLap", "Thang", "Nam", "TongTien", "DaThanhToan", "ConLai", "HanThanhToan", "TrangThai"],
            ["MaHoaDon", "MaHopDong", "MaNguoiLap", "Thang", "Nam", "TongTien", "DaThanhToan", "ConLai", "HanThanhToan", "TrangThai"])
    {
        _context = context;
    }

    public override async Task<IReadOnlyList<HoaDon>> GetAllAsync(string? keyword = null)
    {
        using var connection = _context.CreateConnection();
        var sql = HoaDonSelect + """

            WHERE @Keyword IS NULL
               OR CAST(HD.MaHoaDon AS varchar(20)) LIKE '%' + @Keyword + '%'
               OR CAST(HD.MaHopDong AS varchar(20)) LIKE '%' + @Keyword + '%'
               OR P.SoPhong LIKE '%' + @Keyword + '%'
               OR KT.HoTen LIKE N'%' + @Keyword + N'%'
               OR HD.TrangThai LIKE N'%' + @Keyword + N'%'
            ORDER BY HD.Nam DESC, HD.Thang DESC, HD.MaHoaDon DESC;
            """;

        var result = await connection.QueryAsync<HoaDon>(
            sql,
            new { Keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim() });
        return result.ToList();
    }

    public override async Task<HoaDon?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<HoaDon>(
            HoaDonSelect + "\nWHERE HD.MaHoaDon = @Id;",
            new { Id = id });
    }

    public async Task<IReadOnlyList<HoaDon>> GetRecentAsync(int take)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<HoaDon>(
            HoaDonSelect.Replace("SELECT", "SELECT TOP (@Take)", StringComparison.Ordinal)
                + "\nORDER BY HD.Nam DESC, HD.Thang DESC, HD.MaHoaDon DESC;",
            new { Take = take });
        return result.ToList();
    }

    public async Task<IReadOnlyList<HoaDon>> GetOverdueAsync(string? keyword = null)
    {
        using var connection = _context.CreateConnection();
        var sql = HoaDonSelect + """

            WHERE HD.ConLai > 0
              AND HD.HanThanhToan < CAST(GETDATE() AS date)
              AND (@Keyword IS NULL
                   OR CAST(HD.MaHoaDon AS varchar(20)) LIKE '%' + @Keyword + '%'
                   OR CAST(HD.MaHopDong AS varchar(20)) LIKE '%' + @Keyword + '%'
                   OR P.SoPhong LIKE '%' + @Keyword + '%'
                   OR KT.HoTen LIKE N'%' + @Keyword + N'%'
                   OR HD.TrangThai LIKE N'%' + @Keyword + N'%')
            ORDER BY HD.HanThanhToan, HD.MaHoaDon;
            """;
        var result = await connection.QueryAsync<HoaDon>(
            sql,
            new { Keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim() });
        return result.ToList();
    }

    public async Task<IReadOnlyList<ChiTietHoaDon>> GetChiTietAsync(int maHoaDon)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<ChiTietHoaDon>(
            """
            SELECT CT.*, DV.TenDichVu
            FROM CHITIETHOADON CT
            LEFT JOIN DICHVU DV ON DV.MaDichVu = CT.MaDichVu
            WHERE CT.MaHoaDon = @MaHoaDon
            ORDER BY CT.MaChiTiet;
            """,
            new { MaHoaDon = maHoaDon });
        return result.ToList();
    }

    public async Task<int> AddChiTietAsync(ChiTietHoaDon chiTiet)
    {
        using var connection = _context.CreateConnection();
        return await connection.ExecuteAsync(
            """
            INSERT INTO CHITIETHOADON
                (MaHoaDon, MaDichVu, MoTa, SoLuong, DonGiaTaiThoiDiem, ThanhTien)
            VALUES
                (@MaHoaDon, @MaDichVu, @MoTa, @SoLuong, @DonGiaTaiThoiDiem, @ThanhTien);
            """,
            chiTiet);
    }

}

public class ThanhToanDapperRepository : DapperCrudRepository<ThanhToan>, IThanhToanRepository
{
    private const string SelectSql = """
        SELECT
            TT.*,
            ND.HoTen AS NguoiThuHoTen,
            CONCAT(N'Hóa đơn #', TT.MaHoaDon, N' - HĐ #', HD.MaHopDong, N' - Phòng ', COALESCE(P.SoPhong, N'—')) AS HoaDonMoTa
        FROM THANHTOAN TT
        LEFT JOIN HOADON HD ON HD.MaHoaDon = TT.MaHoaDon
        LEFT JOIN HOPDONG HDG ON HDG.MaHopDong = HD.MaHopDong
        LEFT JOIN PHONGTRO P ON P.MaPhong = HDG.MaPhong
        LEFT JOIN NGUOIDUNG ND ON ND.MaNguoiDung = TT.MaNguoiThu
        """;

    private readonly DapperContext _context;

    public ThanhToanDapperRepository(DapperContext context)
        : base(
            context, "ThanhToan", "MaThanhToan",
            ["MaHoaDon", "MaNguoiThu", "SoTien", "NgayThu", "HinhThuc"],
            ["MaThanhToan", "MaHoaDon", "MaNguoiThu", "SoTien", "NgayThu", "HinhThuc"])
    {
        _context = context;
    }

    public override async Task<IReadOnlyList<ThanhToan>> GetAllAsync(string? keyword = null)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<ThanhToan>(
            SelectSql + """

            WHERE @Keyword IS NULL
               OR CAST(TT.MaThanhToan AS varchar(20)) LIKE '%' + @Keyword + '%'
               OR CAST(TT.MaHoaDon AS varchar(20)) LIKE '%' + @Keyword + '%'
               OR P.SoPhong LIKE N'%' + @Keyword + N'%'
               OR ND.HoTen LIKE N'%' + @Keyword + N'%'
               OR TT.HinhThuc LIKE N'%' + @Keyword + N'%'
            ORDER BY TT.NgayThu DESC, TT.MaThanhToan DESC;
            """,
            new { Keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim() });
        return result.ToList();
    }

    public override async Task<ThanhToan?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ThanhToan>(
            SelectSql + "\nWHERE TT.MaThanhToan = @Id;",
            new { Id = id });
    }

    public async Task<IReadOnlyList<ThanhToan>> GetByHoaDonAsync(int maHoaDon)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<ThanhToan>(
            SelectSql + "\nWHERE TT.MaHoaDon = @MaHoaDon ORDER BY TT.NgayThu DESC, TT.MaThanhToan DESC;",
            new { MaHoaDon = maHoaDon });
        return result.ToList();
    }

}
