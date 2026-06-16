using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class PhongTro
{
    public int MaPhong { get; set; }

    [Display(Name = "Loại phòng")]
    public int MaLoaiPhong { get; set; }

    [Display(Name = "Số phòng")]
    public string SoPhong { get; set; } = string.Empty;

    [Display(Name = "Giá thuê")]
    public decimal GiaThue { get; set; }

    [Display(Name = "Sức chứa tối đa")]
    public int SucChuaToiDa { get; set; }

    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = AppStatuses.Phong.Trong;

    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    public string? LoaiPhongTen { get; set; }
}
