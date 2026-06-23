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

public class DashboardChartsData
{
    public DebtRatioData DebtRatio { get; set; } = new();
    public List<UtilityRoomData> TopUtilities { get; set; } = [];
    public List<ExpiringContractData> ExpiringContracts { get; set; } = [];
}

public class DebtRatioData
{
    public decimal TyLeNoDong_PhanTram { get; set; }
    public decimal TyLeNoQuaHan_PhanTram { get; set; }
}

public class UtilityRoomData
{
    public string SoPhong { get; set; } = "";
    public string TenDichVu { get; set; } = "";
    public decimal TongTien { get; set; }
}

public class ExpiringContractData
{
    public string SoPhong { get; set; } = "";
    public string KhachDaiDien { get; set; } = "";
    public string SoDienThoai { get; set; } = "";
    public DateTime NgayKetThuc { get; set; }
    public int SoNgayConLai { get; set; }
    public string NhomCanhBao { get; set; } = ""; // "Nguy cấp" / "Cảnh báo"
}

