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

    public static string HoaDonStatusClass(string? status) => status switch
    {
        AppStatuses.HoaDon.ChuaThanhToan => "text-bg-warning",
        AppStatuses.HoaDon.MotPhan => "text-bg-info",
        AppStatuses.HoaDon.DaThanhToan => "text-bg-success",
        AppStatuses.HoaDon.QuaHan => "text-bg-danger",
        _ => "text-bg-secondary"
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
