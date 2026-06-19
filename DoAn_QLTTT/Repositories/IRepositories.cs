using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories;

public interface ILoaiPhongRepository : ICrudRepository<LoaiPhong, int>
{
}

public interface IPhongTroRepository : ICrudRepository<PhongTro, int>
{
}

public interface IKhachThueRepository : ICrudRepository<KhachThue, int>
{
}

public interface IHopDongRepository : ICrudRepository<HopDong, int>
{
}

public interface IDichVuRepository : ICrudRepository<DichVu, int>
{
}

public interface IChiSoDienNuocRepository : ICrudRepository<ChiSoDienNuoc, int>
{
}

public interface IHoaDonRepository : ICrudRepository<HoaDon, int>
{
    Task<IReadOnlyList<HoaDon>> GetRecentAsync(int take);
    Task<IReadOnlyList<HoaDon>> GetOverdueAsync();
    Task<IReadOnlyList<ChiTietHoaDon>> GetChiTietAsync(int maHoaDon);
    Task<int> AddChiTietAsync(ChiTietHoaDon chiTiet);
    Task<int> ScanOverdueAsync();
}

public interface IThanhToanRepository : ICrudRepository<ThanhToan, int>
{
    Task<IReadOnlyList<ThanhToan>> GetByHoaDonAsync(int maHoaDon);
}

public interface INguoiDungRepository : ICrudRepository<NguoiDung, int>
{
    Task<NguoiDung?> AuthenticateAsync(string tenDangNhap, string matKhau);
}
