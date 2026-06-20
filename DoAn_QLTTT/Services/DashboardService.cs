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

        // Calculate revenue for the last 6 months
        var last6Months = Enumerable.Range(0, 6)
            .Select(i => today.AddMonths(-i))
            .OrderBy(d => d.Year).ThenBy(d => d.Month)
            .ToList();

        var monthlyRevenueLabels = last6Months.Select(d => $"{d.Month}/{d.Year}").ToList();
        var monthlyRevenueValues = last6Months.Select(d => 
            payments.Where(p => p.NgayThu.Month == d.Month && p.NgayThu.Year == d.Year).Sum(p => p.SoTien)
        ).ToList();

        return new DashboardViewModel
        {
            TongSoPhong = rooms.Count,
            PhongTrong = rooms.Count(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.Trong, "Trong")),
            PhongDangThue = rooms.Count(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.DangThue, "Dang thue", "DangThue")),
            PhongBaoTri = rooms.Count(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.BaoTri, "Bao tri", "BaoTri")),
            HoaDonChuaThanhToan = invoices.Count(x => x.ConLai > 0),
            HoaDonQuaHan = invoices.Count(x => x.ConLai > 0 && x.HanThanhToan < today),
            DoanhThuThang = payments.Where(x => x.NgayThu.Month == DateTime.Today.Month && x.NgayThu.Year == DateTime.Today.Year).Sum(x => x.SoTien),
            TongCongNo = invoices.Sum(x => x.ConLai),
            HoaDonGanDay = await _hoaDonRepository.GetRecentAsync(6),
            PhongDangThueList = rooms
                .Where(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.DangThue, "Dang thue", "DangThue"))
                .Take(6)
                .ToList(),
            MonthlyRevenueLabels = monthlyRevenueLabels,
            MonthlyRevenueValues = monthlyRevenueValues
        };
    }

    private static bool IsRoomStatus(string? current, params string[] accepted) =>
        accepted.Any(x => string.Equals(current?.Trim(), x, StringComparison.OrdinalIgnoreCase));
}
