using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class NguoiDung
{
    public int MaNguoiDung { get; set; }

    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = string.Empty;

    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Display(Name = "Vai trò")]
    public string VaiTro { get; set; } = AppStatuses.VaiTro.Staff;

    [Display(Name = "Đang hoạt động")]
    public bool DangHoatDong { get; set; } = true;
}
