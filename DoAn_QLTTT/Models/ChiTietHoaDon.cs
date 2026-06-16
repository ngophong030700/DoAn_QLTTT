using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class ChiTietHoaDon
{
    public int MaChiTiet { get; set; }

    [Display(Name = "Hóa đơn")]
    public int MaHoaDon { get; set; }

    [Display(Name = "Dịch vụ")]
    public int MaDichVu { get; set; }

    [Display(Name = "Mô tả")]
    public string MoTa { get; set; } = string.Empty;

    [Display(Name = "Số lượng")]
    public decimal SoLuong { get; set; }

    [Display(Name = "Đơn giá tại thời điểm")]
    public decimal DonGiaTaiThoiDiem { get; set; }

    [Display(Name = "Thành tiền")]
    public decimal ThanhTien { get; set; }

    public string? TenDichVu { get; set; }
}
