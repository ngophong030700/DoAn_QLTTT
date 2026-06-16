using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;

namespace DoAn_QLTTT.Services;

public class DashboardService : IDashboardService
{
    private readonly IPhongTroRepository _phongTroRepository;
    private readonly IHoaDonRepository _hoaDonRepository;
    private readonly IThanhToanRepository _thanhToanRepository;

    public DashboardService(
        IPhongTroRepository phongTroRepository,
        IHoaDonRepository hoaDonRepository,
        IThanhToanRepository thanhToanRepository)
    {
        _phongTroRepository = phongTroRepository;
        _hoaDonRepository = hoaDonRepository;
        _thanhToanRepository = thanhToanRepository;
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var rooms = await _phongTroRepository.GetAllAsync();
        var invoices = await _hoaDonRepository.GetAllAsync();
        var payments = await _thanhToanRepository.GetAllAsync();
        var today = DateOnly.FromDateTime(DateTime.Today);

        return new DashboardViewModel
        {
            TongSoPhong = rooms.Count,
            PhongTrong = rooms.Count(x => x.TrangThai == AppStatuses.Phong.Trong),
            PhongDangThue = rooms.Count(x => x.TrangThai == AppStatuses.Phong.DangThue),
            HoaDonChuaThanhToan = invoices.Count(x => x.ConLai > 0),
            HoaDonQuaHan = invoices.Count(x => x.ConLai > 0 && x.HanThanhToan < today),
            DoanhThuThang = payments.Where(x => x.NgayThu.Month == DateTime.Today.Month && x.NgayThu.Year == DateTime.Today.Year).Sum(x => x.SoTien),
            TongCongNo = invoices.Sum(x => x.ConLai),
            HoaDonGanDay = await _hoaDonRepository.GetRecentAsync(6),
            PhongDangThueList = rooms.Where(x => x.TrangThai == AppStatuses.Phong.DangThue).Take(6).ToList()
        };
    }
}
