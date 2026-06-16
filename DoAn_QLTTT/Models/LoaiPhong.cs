using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class LoaiPhong
{
    public int MaLoaiPhong { get; set; }

    [Display(Name = "Tên loại")]
    public string TenLoai { get; set; } = string.Empty;

    [Display(Name = "Diện tích")]
    public decimal DienTich { get; set; }

    [Display(Name = "Giá thuê cơ sở")]
    public decimal GiaThueCoSo { get; set; }

    [Display(Name = "Sức chứa tối đa")]
    public int SucChuaToiDa { get; set; }
}
