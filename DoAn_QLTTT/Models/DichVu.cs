using System.ComponentModel.DataAnnotations;

namespace DoAn_QLTTT.Models;

public class DichVu
{
    public int MaDichVu { get; set; }

    [Display(Name = "Tên dịch vụ")]
    public string TenDichVu { get; set; } = string.Empty;

    [Display(Name = "Đơn vị")]
    public string DonVi { get; set; } = string.Empty;

    [Display(Name = "Đơn giá")]
    public decimal DonGia { get; set; }

    [Display(Name = "Loại tính phí")]
    public string LoaiTinhPhi { get; set; } = string.Empty;
}
