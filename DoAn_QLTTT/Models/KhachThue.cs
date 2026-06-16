using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class KhachThue
{
    public int MaKhach { get; set; }

    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Display(Name = "CCCD")]
    public string CCCD { get; set; } = string.Empty;

    [Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    [Display(Name = "Địa chỉ")]
    public string DiaChi { get; set; } = string.Empty;
}
