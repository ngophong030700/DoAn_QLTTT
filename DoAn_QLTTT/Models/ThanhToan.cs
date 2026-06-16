using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class ThanhToan
{
    public int MaThanhToan { get; set; }

    [Display(Name = "Hóa đơn")]
    public int MaHoaDon { get; set; }

    [Display(Name = "Người thu")]
    public int MaNguoiThu { get; set; }

    [Display(Name = "Số tiền")]
    public decimal SoTien { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngày thu")]
    public DateOnly NgayThu { get; set; }

    [Display(Name = "Hình thức")]
    public string HinhThuc { get; set; } = string.Empty;

    public string? HoaDonMoTa { get; set; }

    public string? NguoiThuHoTen { get; set; }
}
