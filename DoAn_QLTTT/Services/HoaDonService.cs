using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;

namespace DoAn_QLTTT.Services;

public class HoaDonService : IHoaDonService
{
    private readonly IHopDongRepository _hopDongRepository;
    private readonly IHoaDonRepository _hoaDonRepository;
    private readonly IDichVuRepository _dichVuRepository;

    public HoaDonService(
        IHopDongRepository hopDongRepository,
        IHoaDonRepository hoaDonRepository,
        IDichVuRepository dichVuRepository)
    {
        _hopDongRepository = hopDongRepository;
        _hoaDonRepository = hoaDonRepository;
        _dichVuRepository = dichVuRepository;
    }

    public async Task<int> LapHoaDonDemoAsync()
    {
        var hopDong = (await _hopDongRepository.GetAllAsync())
            .Where(x => x.TrangThai == AppStatuses.HopDong.HieuLuc)
            .OrderBy(x => x.MaHopDong)
            .FirstOrDefault();

        if (hopDong is null)
        {
            return 0;
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var hoaDon = new HoaDon
        {
            MaHopDong = hopDong.MaHopDong,
            MaNguoiLap = 2,
            Thang = today.Month,
            Nam = today.Year,
            HanThanhToan = today.AddDays(7),
            TrangThai = AppStatuses.HoaDon.ChuaThanhToan
        };

        var maHoaDon = await _hoaDonRepository.AddAsync(hoaDon);
        if (maHoaDon == 0)
        {
            return 0;
        }

        await _hoaDonRepository.AddChiTietAsync(new ChiTietHoaDon
        {
            MaHoaDon = maHoaDon,
            MaDichVu = 0,
            MoTa = $"Tiền thuê phòng {hopDong.SoPhong} tháng {today.Month}/{today.Year}",
            SoLuong = 1,
            DonGiaTaiThoiDiem = hopDong.TienThueThang
        });

        var fixedServices = (await _dichVuRepository.GetAllAsync())
            .Where(x => x.LoaiTinhPhi.Contains("Cố định", StringComparison.OrdinalIgnoreCase));

        foreach (var service in fixedServices)
        {
            await _hoaDonRepository.AddChiTietAsync(new ChiTietHoaDon
            {
                MaHoaDon = maHoaDon,
                MaDichVu = service.MaDichVu,
                MoTa = service.TenDichVu,
                SoLuong = 1,
                DonGiaTaiThoiDiem = service.DonGia
            });
        }

        // TODO: Khi dùng DB thật, sp_HoaDon_Insert/sp_ChiTietHoaDon_Insert nên trả MaHoaDon qua OUTPUT.
        // TODO: Trigger SQL Server sẽ tự tính TongTien, DaThanhToan, ConLai và TrangThai sau khi thêm chi tiết.
        return maHoaDon;
    }
}
