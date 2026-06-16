namespace DoAn_QLTTT.Models;

public static class AppStatuses
{
    public static class Phong
    {
        public const string Trong = "Trống";
        public const string DangThue = "Đang thuê";
        public const string BaoTri = "Bảo trì";

        public static readonly string[] All = [Trong, DangThue, BaoTri];
    }

    public static class HoaDon
    {
        public const string ChuaThanhToan = "Chưa TT";
        public const string MotPhan = "Một phần";
        public const string DaThanhToan = "Đã TT";
        public const string QuaHan = "Quá hạn";

        public static readonly string[] All = [ChuaThanhToan, MotPhan, DaThanhToan, QuaHan];
    }

    public static class HopDong
    {
        public const string HieuLuc = "Hiệu lực";
        public const string SapHetHan = "Sắp hết hạn";
        public const string KetThuc = "Kết thúc";

        public static readonly string[] All = [HieuLuc, SapHetHan, KetThuc];
    }

    public static class VaiTro
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";

        public static readonly string[] All = [Admin, Staff];
    }
}
