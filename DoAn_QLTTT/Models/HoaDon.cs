using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class HoaDon
{
    public int MaHoaDon { get; set; }

    [Display(Name = "Hợp đồng")]
    public int MaHopDong { get; set; }

    [Display(Name = "Người lập")]
    public int MaNguoiLap { get; set; }

    [Range(1, 12)]
    public int Thang { get; set; }

    public int Nam { get; set; }

    [Display(Name = "Tổng tiền")]
    public decimal TongTien { get; set; }

    [Display(Name = "Đã thanh toán")]
    public decimal DaThanhToan { get; set; }

    [Display(Name = "Còn lại")]
    public decimal ConLai { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Hạn thanh toán")]
    public DateOnly HanThanhToan { get; set; }

    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = AppStatuses.HoaDon.ChuaThanhToan;

    public string? HopDongMoTa { get; set; }

    public string? SoPhong { get; set; }

    public string? KhachDaiDienHoTen { get; set; }

    public string? NguoiLapHoTen { get; set; }
}
