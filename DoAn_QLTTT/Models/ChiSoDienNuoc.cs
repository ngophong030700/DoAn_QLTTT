using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class ChiSoDienNuoc
{
    public int MaChiSo { get; set; }

    [Display(Name = "Phòng")]
    public int MaPhong { get; set; }

    [Display(Name = "Dịch vụ")]
    public int MaDichVu { get; set; }

    [Display(Name = "Người nhập")]
    public int MaNguoiNhap { get; set; }

    [Range(1, 12)]
    public int Thang { get; set; }

    public int Nam { get; set; }

    [Display(Name = "Chỉ số cũ")]
    public decimal ChiSoCu { get; set; }

    [Display(Name = "Chỉ số mới")]
    public decimal ChiSoMoi { get; set; }

    [Display(Name = "Tiêu thụ")]
    public decimal TieuThu { get; set; }

    public string? SoPhong { get; set; }

    public string? DichVuTen { get; set; }

    public string? NguoiNhapHoTen { get; set; }
}
