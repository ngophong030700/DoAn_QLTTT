using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_QLTTT.ViewModels;

public class LoaiPhongFormViewModel
{
    public int MaLoaiPhong { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên loại phòng")]
    [StringLength(100)]
    [Display(Name = "Tên loại")]
    public string TenLoai { get; set; } = string.Empty;

    [Range(1, 500, ErrorMessage = "Diện tích phải lớn hơn 0")]
    [Display(Name = "Diện tích")]
    public decimal DienTich { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá thuê không hợp lệ")]
    [Display(Name = "Giá thuê cơ sở")]
    public decimal GiaThueCoSo { get; set; }

    [Range(1, 20, ErrorMessage = "Sức chứa tối thiểu là 1")]
    [Display(Name = "Sức chứa tối đa")]
    public int SucChuaToiDa { get; set; }
}

public class PhongTroFormViewModel
{
    public int MaPhong { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
    [Display(Name = "Loại phòng")]
    public int MaLoaiPhong { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số phòng")]
    [StringLength(30)]
    [Display(Name = "Số phòng")]
    public string SoPhong { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Giá thuê không hợp lệ")]
    [Display(Name = "Giá thuê")]
    public decimal GiaThue { get; set; }

    [Range(1, 20, ErrorMessage = "Sức chứa tối thiểu là 1")]
    [Display(Name = "Sức chứa tối đa")]
    public int SucChuaToiDa { get; set; }

    [Required]
    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    public IEnumerable<SelectListItem> LoaiPhongOptions { get; set; } = [];
    public IEnumerable<SelectListItem> TrangThaiOptions { get; set; } = [];
}

public class KhachThueFormViewModel
{
    public int MaKhach { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(150)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập CCCD")]
    [StringLength(20)]
    [Display(Name = "CCCD")]
    public string CCCD { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [StringLength(20)]
    [Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Địa chỉ")]
    public string DiaChi { get; set; } = string.Empty;
}

public class HopDongFormViewModel
{
    public int MaHopDong { get; set; }

    [Required]
    [Display(Name = "Phòng")]
    public int MaPhong { get; set; }

    [Required]
    [Display(Name = "Khách đại diện")]
    public int MaKhachDaiDien { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày bắt đầu")]
    public DateOnly NgayBatDau { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [Display(Name = "Ngày kết thúc")]
    public DateOnly? NgayKetThuc { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Tiền thuê tháng")]
    public decimal TienThueThang { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Tiền cọc")]
    public decimal TienCoc { get; set; }

    [Required]
    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = string.Empty;

    public IEnumerable<SelectListItem> PhongOptions { get; set; } = [];
    public IEnumerable<SelectListItem> KhachOptions { get; set; } = [];
    public IEnumerable<SelectListItem> TrangThaiOptions { get; set; } = [];
}

public class DichVuFormViewModel
{
    public int MaDichVu { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên dịch vụ")]
    [StringLength(100)]
    [Display(Name = "Tên dịch vụ")]
    public string TenDichVu { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    [Display(Name = "Đơn vị")]
    public string DonVi { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    [Display(Name = "Đơn giá")]
    public decimal DonGia { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Loại tính phí")]
    public string LoaiTinhPhi { get; set; } = string.Empty;
}

public class ChiSoDienNuocFormViewModel
{
    public int MaChiSo { get; set; }

    [Required]
    [Display(Name = "Phòng")]
    public int MaPhong { get; set; }

    [Required]
    [Display(Name = "Dịch vụ")]
    public int MaDichVu { get; set; }

    [Required]
    [Display(Name = "Người nhập")]
    public int MaNguoiNhap { get; set; }

    [Range(1, 12)]
    public int Thang { get; set; } = DateTime.Today.Month;

    [Range(2000, 2100)]
    public int Nam { get; set; } = DateTime.Today.Year;

    [Range(0, double.MaxValue)]
    [Display(Name = "Chỉ số cũ")]
    public decimal ChiSoCu { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Chỉ số mới")]
    public decimal ChiSoMoi { get; set; }

    public IEnumerable<SelectListItem> PhongOptions { get; set; } = [];
    public IEnumerable<SelectListItem> DichVuOptions { get; set; } = [];
    public IEnumerable<SelectListItem> NguoiDungOptions { get; set; } = [];
}

public class HoaDonFormViewModel
{
    public int MaHoaDon { get; set; }

    [Required]
    [Display(Name = "Hợp đồng")]
    public int MaHopDong { get; set; }

    [Required]
    [Display(Name = "Người lập")]
    public int MaNguoiLap { get; set; }

    [Range(1, 12)]
    public int Thang { get; set; } = DateTime.Today.Month;

    [Range(2000, 2100)]
    public int Nam { get; set; } = DateTime.Today.Year;

    [DataType(DataType.Date)]
    [Display(Name = "Hạn thanh toán")]
    public DateOnly HanThanhToan { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(7));

    [Required]
    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = string.Empty;

    public IEnumerable<SelectListItem> HopDongOptions { get; set; } = [];
    public IEnumerable<SelectListItem> NguoiDungOptions { get; set; } = [];
    public IEnumerable<SelectListItem> TrangThaiOptions { get; set; } = [];
}

public class ThanhToanFormViewModel
{
    public int MaThanhToan { get; set; }

    [Required]
    [Display(Name = "Hóa đơn")]
    public int MaHoaDon { get; set; }

    [Required]
    [Display(Name = "Người thu")]
    public int MaNguoiThu { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
    [Display(Name = "Số tiền")]
    public decimal SoTien { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngày thu")]
    public DateOnly NgayThu { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [StringLength(50)]
    [Display(Name = "Hình thức")]
    public string HinhThuc { get; set; } = "TienMat";

    public IEnumerable<SelectListItem> HoaDonOptions { get; set; } = [];
    public IEnumerable<SelectListItem> NguoiDungOptions { get; set; } = [];
}

public class NguoiDungFormViewModel
{
    public int MaNguoiDung { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Vai trò")]
    public string VaiTro { get; set; } = string.Empty;

    [Display(Name = "Đang hoạt động")]
    public bool DangHoatDong { get; set; } = true;

    public IEnumerable<SelectListItem> VaiTroOptions { get; set; } = [];
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = string.Empty;

    public bool GhiNhoDangNhap { get; set; }
    public string? ReturnUrl { get; set; }
}
