using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.ViewModels;

public class HoaDonDetailsViewModel
{
    public HoaDon HoaDon { get; set; } = new();
    public IReadOnlyList<ChiTietHoaDon> ChiTietHoaDons { get; set; } = [];
    public IReadOnlyList<ThanhToan> ThanhToans { get; set; } = [];
}
