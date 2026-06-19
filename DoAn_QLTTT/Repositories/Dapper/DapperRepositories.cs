using System.Data;
using Dapper;
using DoAn_QLTTT.Data;
using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Dapper;

public class LoaiPhongDapperRepository(DapperContext context)
    : DapperCrudRepository<LoaiPhong>(context, "LoaiPhong", "MaLoaiPhong", "sp_LoaiPhong_Insert", "sp_LoaiPhong_Update", "sp_LoaiPhong_Delete"), ILoaiPhongRepository;

public class PhongTroDapperRepository(DapperContext context)
    : DapperCrudRepository<PhongTro>(context, "PhongTro", "MaPhong", "sp_PhongTro_Insert", "sp_PhongTro_Update", "sp_PhongTro_Delete"), IPhongTroRepository;

public class KhachThueDapperRepository(DapperContext context)
    : DapperCrudRepository<KhachThue>(context, "KhachThue", "MaKhach", "sp_KhachThue_Insert", "sp_KhachThue_Update", "sp_KhachThue_Delete"), IKhachThueRepository;

public class HopDongDapperRepository(DapperContext context)
    : DapperCrudRepository<HopDong>(context, "HopDong", "MaHopDong", "sp_HopDong_Insert", "sp_HopDong_Update", "sp_HopDong_Delete"), IHopDongRepository;

public class DichVuDapperRepository(DapperContext context)
    : DapperCrudRepository<DichVu>(context, "DichVu", "MaDichVu", "sp_DichVu_Insert", "sp_DichVu_Update", "sp_DichVu_Delete"), IDichVuRepository;

public class ChiSoDienNuocDapperRepository(DapperContext context)
    : DapperCrudRepository<ChiSoDienNuoc>(context, "ChiSoDienNuoc", "MaChiSo", "sp_ChiSoDienNuoc_Insert", "sp_ChiSoDienNuoc_Update", "sp_ChiSoDienNuoc_Delete"), IChiSoDienNuocRepository;

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
    private readonly DapperContext _context;

    public HoaDonDapperRepository(DapperContext context)
        : base(context, "HoaDon", "MaHoaDon", "sp_HoaDon_Insert", "sp_HoaDon_Update", "sp_HoaDon_Delete")
    {
        _context = context;
    }

    public async Task<IReadOnlyList<HoaDon>> GetRecentAsync(int take)
    {
        using var connection = _context.CreateConnection();
        // TODO: Thay bằng sp_HoaDon_GetRecent nếu cần join đầy đủ tên phòng/khách/người lập.
        var result = await connection.QueryAsync<HoaDon>(
            "SELECT TOP (@Take) * FROM HoaDon ORDER BY Nam DESC, Thang DESC, MaHoaDon DESC",
            new { Take = take });
        return result.ToList();
    }

    public async Task<IReadOnlyList<HoaDon>> GetOverdueAsync()
    {
        using var connection = _context.CreateConnection();
        // TODO: Có thể thay bằng stored procedure/view danh sách nhắc nợ chính thức.
        var result = await connection.QueryAsync<HoaDon>(
            "SELECT * FROM HoaDon WHERE ConLai > 0 AND HanThanhToan < CAST(GETDATE() AS date)");
        return result.ToList();
    }

    public async Task<IReadOnlyList<ChiTietHoaDon>> GetChiTietAsync(int maHoaDon)
    {
        using var connection = _context.CreateConnection();
        // TODO: Thay bằng sp_ChiTietHoaDon_GetByHoaDon để join tên dịch vụ khi chốt DB.
        var result = await connection.QueryAsync<ChiTietHoaDon>(
            "SELECT * FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon",
            new { MaHoaDon = maHoaDon });
        return result.ToList();
    }

    public async Task<int> AddChiTietAsync(ChiTietHoaDon chiTiet)
    {
        using var connection = _context.CreateConnection();
        // TODO: sp_ChiTietHoaDon_Insert sẽ thêm dòng chi tiết; trigger DB tự tính lại TongTien hóa đơn.
        return await connection.ExecuteAsync("sp_ChiTietHoaDon_Insert", chiTiet, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> ScanOverdueAsync()
    {
        using var connection = _context.CreateConnection();
        // TODO: sp_HoaDon_QuetQuaHan có thể dùng cursor/job để cập nhật trạng thái hóa đơn quá hạn.
        return await connection.ExecuteAsync("sp_HoaDon_QuetQuaHan", commandType: CommandType.StoredProcedure);
    }
}

public class ThanhToanDapperRepository : DapperCrudRepository<ThanhToan>, IThanhToanRepository
{
    private readonly DapperContext _context;

    public ThanhToanDapperRepository(DapperContext context)
        : base(context, "ThanhToan", "MaThanhToan", "sp_ThanhToan_Insert", "sp_ThanhToan_Update", "sp_ThanhToan_Delete")
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ThanhToan>> GetByHoaDonAsync(int maHoaDon)
    {
        using var connection = _context.CreateConnection();
        // TODO: Thay bằng sp_ThanhToan_GetByHoaDon nếu cần join tên người thu.
        var result = await connection.QueryAsync<ThanhToan>(
            "SELECT * FROM ThanhToan WHERE MaHoaDon = @MaHoaDon ORDER BY NgayThu DESC",
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
