using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class HopDong
{
    public int MaHopDong { get; set; }

    [Display(Name = "Phòng")]
    public int MaPhong { get; set; }

    [Display(Name = "Khách đại diện")]
    public int MaKhachDaiDien { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngày bắt đầu")]
    public DateOnly NgayBatDau { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngày kết thúc")]
    public DateOnly? NgayKetThuc { get; set; }

    [Display(Name = "Tiền thuê tháng")]
    public decimal TienThueThang { get; set; }

    [Display(Name = "Tiền cọc")]
    public decimal TienCoc { get; set; }

    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = AppStatuses.HopDong.HieuLuc;

    public string? SoPhong { get; set; }

    public string? KhachDaiDienHoTen { get; set; }
}
