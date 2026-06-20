using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Helpers;

public static class BadgeHelper
{
    public static string PhongStatusClass(string? status) => status?.Trim() switch
    {
        AppStatuses.Phong.Trong or "Trong" => "bg-success text-white",
        AppStatuses.Phong.DangThue or "Dang thue" or "DangThue" => "bg-primary text-white",
        AppStatuses.Phong.BaoTri or "Bao tri" or "BaoTri" => "bg-warning text-dark",
        _ => "bg-secondary text-white"
    };

    public static string PhongStatusText(string? status) => status?.Trim() switch
    {
        AppStatuses.Phong.Trong or "Trong" => "Trống",
        AppStatuses.Phong.DangThue or "Dang thue" or "DangThue" => "Đang thuê",
        AppStatuses.Phong.BaoTri or "Bao tri" or "BaoTri" => "Bảo trì",
        null or "" => "Chưa xác định",
        _ => status!
    };

    public static string HoaDonStatusClass(string? status) => status?.Trim() switch
    {
        AppStatuses.HoaDon.ChuaThanhToan => "bg-warning text-dark",
        "Chưa thanh toán" => "bg-warning text-dark",
        "ChuaThanhToan" => "bg-warning text-dark",
        AppStatuses.HoaDon.MotPhan => "bg-info text-dark",
        "Mot phan" => "bg-info text-dark",
        "Thanh toán một phần" => "bg-info text-dark",
        "MotPhan" => "bg-info text-dark",
        AppStatuses.HoaDon.DaThanhToan => "bg-success text-white",
        "Đã thanh toán" => "bg-success text-white",
        "DaThanhToan" => "bg-success text-white",
        AppStatuses.HoaDon.QuaHan => "bg-danger text-white",
        "QuaHan" => "bg-danger text-white",
        _ => "bg-secondary text-white"
    };

    public static string HoaDonStatusText(string? status) => status?.Trim() switch
    {
        AppStatuses.HoaDon.ChuaThanhToan or "Chưa thanh toán" or "ChuaThanhToan" => "Chưa thanh toán",
        AppStatuses.HoaDon.MotPhan or "Mot phan" or "Thanh toán một phần" or "MotPhan" => "Thanh toán một phần",
        AppStatuses.HoaDon.DaThanhToan or "Đã thanh toán" or "DaThanhToan" => "Đã thanh toán",
        AppStatuses.HoaDon.QuaHan or "QuaHan" => "Quá hạn",
        null or "" => "Chưa xác định",
        _ => status!
    };

    public static string VaiTroClass(string? role) => role switch
    {
        AppStatuses.VaiTro.Admin => "text-bg-dark",
        AppStatuses.VaiTro.Staff => "text-bg-secondary",
        _ => "text-bg-light text-dark"
    };

    public static string HopDongStatusClass(string? status) => status?.Trim() switch
    {
        AppStatuses.HopDong.HieuLuc or "Hieu luc" or "HieuLuc" => "bg-success text-white",
        AppStatuses.HopDong.SapHetHan or "Sap het han" or "SapHetHan" => "bg-warning text-dark",
        AppStatuses.HopDong.KetThuc or "Ket thuc" or "KetThuc" or "Đã kết thúc" => "bg-secondary text-white",
        _ => "bg-dark text-white"
    };

    public static string HopDongStatusText(string? status) => status?.Trim() switch
    {
        AppStatuses.HopDong.HieuLuc or "Hieu luc" or "HieuLuc" => "Hiệu lực",
        AppStatuses.HopDong.SapHetHan or "Sap het han" or "SapHetHan" => "Sắp hết hạn",
        AppStatuses.HopDong.KetThuc or "Ket thuc" or "KetThuc" or "Đã kết thúc" => "Kết thúc",
        null or "" => "Chưa xác định",
        _ => status!
    };

    public static string HinhThucThanhToanClass(string? method) => method?.Trim() switch
    {
        "TienMat" or "Tiền mặt" => "bg-success text-white",
        "ChuyenKhoan" or "Chuyển khoản" => "bg-info text-dark",
        _ => "bg-secondary text-white"
    };

    public static string HinhThucThanhToanText(string? method) => method?.Trim() switch
    {
        "TienMat" or "Tiền mặt" => "Tiền mặt",
        "ChuyenKhoan" or "Chuyển khoản" => "Chuyển khoản",
        null or "" => "Chưa xác định",
        _ => method!
    };
}
