using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Helpers;

public static class BadgeHelper
{
    public static string PhongStatusClass(string? status) => status switch
    {
        AppStatuses.Phong.Trong => "text-bg-success",
        AppStatuses.Phong.DangThue => "text-bg-primary",
        AppStatuses.Phong.BaoTri => "text-bg-warning",
        _ => "text-bg-secondary"
    };

    public static string HoaDonStatusClass(string? status) => status?.Trim() switch
    {
        AppStatuses.HoaDon.ChuaThanhToan => "bg-warning text-dark",
        "Chưa thanh toán" => "bg-warning text-dark",
        "ChuaThanhToan" => "bg-warning text-dark",
        AppStatuses.HoaDon.MotPhan => "bg-info text-dark",
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
        AppStatuses.HoaDon.MotPhan or "Thanh toán một phần" or "MotPhan" => "Thanh toán một phần",
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

    public static string HopDongStatusClass(string? status) => status switch
    {
        AppStatuses.HopDong.HieuLuc => "text-bg-success",
        AppStatuses.HopDong.SapHetHan => "text-bg-warning",
        AppStatuses.HopDong.KetThuc => "text-bg-secondary",
        _ => "text-bg-light text-dark"
    };
}
