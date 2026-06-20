using System.Data;
using Dapper;
using DoAn_QLTTT.Data;
using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Dapper;

public class LoaiPhongDapperRepository(DapperContext context)
    : DapperCrudRepository<LoaiPhong>(context, "LoaiPhong", "MaLoaiPhong", "sp_LoaiPhong_Insert", "sp_LoaiPhong_Update", "sp_LoaiPhong_Delete"), ILoaiPhongRepository;

public class PhongTroDapperRepository : DapperCrudRepository<PhongTro>, IPhongTroRepository
{
    private const string SelectSql = """
        SELECT PT.*, LP.TenLoai AS LoaiPhongTen
        FROM PHONGTRO PT
        LEFT JOIN LOAIPHONG LP ON LP.MaLoaiPhong = PT.MaLoaiPhong
        """;

    private readonly DapperContext _context;

    public PhongTroDapperRepository(DapperContext context)
        : base(context, "PhongTro", "MaPhong", "sp_PhongTro_Insert", "sp_PhongTro_Update", "sp_PhongTro_Delete")
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
    : DapperCrudRepository<KhachThue>(context, "KhachThue", "MaKhach", "sp_KhachThue_Insert", "sp_KhachThue_Update", "sp_KhachThue_Delete"), IKhachThueRepository;

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
        : base(context, "HopDong", "MaHopDong", "sp_HopDong_Insert", "sp_HopDong_Update", "sp_HopDong_Delete")
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
    : DapperCrudRepository<DichVu>(context, "DichVu", "MaDichVu", "sp_DichVu_Insert", "sp_DichVu_Update", "sp_DichVu_Delete"), IDichVuRepository;

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
        : base(context, "ChiSoDienNuoc", "MaChiSo", "sp_ChiSoDienNuoc_Insert", "sp_ChiSoDienNuoc_Update", "sp_ChiSoDienNuoc_Delete")
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
        : base(context, "NguoiDung", "MaNguoiDung", "sp_NguoiDung_Insert", "sp_NguoiDung_Update", "sp_NguoiDung_Delete")
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
        : base(context, "HoaDon", "MaHoaDon", "sp_HoaDon_Insert", "sp_HoaDon_Update", "sp_HoaDon_Delete")
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

    public async Task<IReadOnlyList<HoaDon>> GetOverdueAsync()
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<HoaDon>(
            HoaDonSelect + """

            WHERE HD.ConLai > 0
              AND HD.HanThanhToan < CAST(GETDATE() AS date)
            ORDER BY HD.HanThanhToan, HD.MaHoaDon;
            """);
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
        // TODO: sp_ChiTietHoaDon_Insert sẽ thêm dòng chi tiết; trigger DB tự tính lại TongTien hóa đơn.
        return await connection.ExecuteAsync("sp_ChiTietHoaDon_Insert", chiTiet, commandType: CommandType.StoredProcedure);
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
        : base(context, "ThanhToan", "MaThanhToan", "sp_ThanhToan_Insert", "sp_ThanhToan_Update", "sp_ThanhToan_Delete")
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

    public override async Task<int> AddAsync(ThanhToan entity)
    {
        using var connection = _context.CreateConnection();
        // TODO: sp_ThanhToan_Insert ghi nhận thanh toán; trigger DB cập nhật DaThanhToan, ConLai, TrangThai của HoaDon.
        return await connection.ExecuteAsync("sp_ThanhToan_Insert", entity, commandType: CommandType.StoredProcedure);
    }
}
