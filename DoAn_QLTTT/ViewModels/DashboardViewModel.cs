using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.ViewModels;

public class DashboardViewModel
{
    public int TongSoPhong { get; set; }
    public int PhongTrong { get; set; }
    public int PhongDangThue { get; set; }
    public int PhongBaoTri { get; set; }
    public int HoaDonChuaThanhToan { get; set; }
    public int HoaDonQuaHan { get; set; }
    public decimal DoanhThuThang { get; set; }
    public decimal TongCongNo { get; set; }
    public IReadOnlyList<HoaDon> HoaDonGanDay { get; set; } = [];
    public IReadOnlyList<PhongTro> PhongDangThueList { get; set; } = [];
    public List<string> MonthlyRevenueLabels { get; set; } = [];
    public List<decimal> MonthlyRevenueValues { get; set; } = [];
}
