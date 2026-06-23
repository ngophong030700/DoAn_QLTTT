using DoAn_QLTTT.Models;
using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using DoAn_QLTTT.Data;
using Dapper;
using System.Data;

namespace DoAn_QLTTT.Services;

public class DashboardService : IDashboardService
{
    private readonly IPhongTroRepository _phongTroRepository;
    private readonly IHoaDonRepository _hoaDonRepository;
    private readonly IThanhToanRepository _thanhToanRepository;
    private readonly DapperContext _context;

    public DashboardService(
        IPhongTroRepository phongTroRepository,
        IHoaDonRepository hoaDonRepository,
        IThanhToanRepository thanhToanRepository,
        DapperContext context)
    {
        _phongTroRepository = phongTroRepository;
        _hoaDonRepository = hoaDonRepository;
        _thanhToanRepository = thanhToanRepository;
        _context = context;
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var rooms = await _phongTroRepository.GetAllAsync();
        var invoices = await _hoaDonRepository.GetAllAsync();
        var payments = await _thanhToanRepository.GetAllAsync();
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Calculate revenue for the last 6 months
        var last6Months = Enumerable.Range(0, 6)
            .Select(i => today.AddMonths(-i))
            .OrderBy(d => d.Year).ThenBy(d => d.Month)
            .ToList();

        var monthlyRevenueLabels = last6Months.Select(d => $"{d.Month}/{d.Year}").ToList();
        var monthlyRevenueValues = last6Months.Select(d => 
            payments.Where(p => p.NgayThu.Month == d.Month && p.NgayThu.Year == d.Year).Sum(p => p.SoTien)
        ).ToList();

        return new DashboardViewModel
        {
            TongSoPhong = rooms.Count,
            PhongTrong = rooms.Count(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.Trong, "Trong")),
            PhongDangThue = rooms.Count(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.DangThue, "Dang thue", "DangThue")),
            PhongBaoTri = rooms.Count(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.BaoTri, "Bao tri", "BaoTri")),
            HoaDonChuaThanhToan = invoices.Count(x => x.ConLai > 0),
            HoaDonQuaHan = invoices.Count(x => x.ConLai > 0 && x.HanThanhToan < today),
            DoanhThuThang = payments.Where(x => x.NgayThu.Month == DateTime.Today.Month && x.NgayThu.Year == DateTime.Today.Year).Sum(x => x.SoTien),
            TongCongNo = invoices.Sum(x => x.ConLai),
            HoaDonGanDay = await _hoaDonRepository.GetRecentAsync(6),
            PhongDangThueList = rooms
                .Where(x => IsRoomStatus(x.TrangThai, AppStatuses.Phong.DangThue, "Dang thue", "DangThue"))
                .Take(6)
                .ToList(),
            MonthlyRevenueLabels = monthlyRevenueLabels,
            MonthlyRevenueValues = monthlyRevenueValues
        };
    }

    public async Task<DashboardChartsData> GetDashboardChartsDataAsync()
    {
        var chartsData = new DashboardChartsData();

        using var connection = _context.CreateConnection();

        // 1. Tỷ lệ nợ động / quá hạn (Chart 3) — tính trực tiếp từ bảng HOADON
        try
        {
            var debtRatio = await connection.QueryFirstOrDefaultAsync<DebtRatioData>(
                @"SELECT 
                    CAST(SUM(CASE WHEN ConLai > 0 THEN 1.0 ELSE 0 END) * 100.0 / NULLIF(COUNT(*), 0) AS DECIMAL(10,2)) AS TyLeNoDong_PhanTram,
                    CAST(SUM(CASE WHEN ConLai > 0 AND HanThanhToan < GETDATE() THEN 1.0 ELSE 0 END) * 100.0 / NULLIF(COUNT(*), 0) AS DECIMAL(10,2)) AS TyLeNoQuaHan_PhanTram
                FROM HOADON"
            );
            if (debtRatio != null)
            {
                chartsData.DebtRatio = debtRatio;
            }
        }
        catch
        {
            chartsData.DebtRatio = new DebtRatioData();
        }

        // 2. Top phòng xài nhiều điện nước (Chart 4) — JOIN trực tiếp các bảng gốc
        try
        {
            chartsData.TopUtilities = (await connection.QueryAsync<UtilityRoomData>(
                @"SELECT TOP 10 
                    p.SoPhong, 
                    dv.TenDichVu, 
                    SUM(cs.TieuThu * dv.DonGia) AS TongTien
                FROM CHISODIENNUOC cs
                INNER JOIN PHONGTRO p ON cs.MaPhong = p.MaPhong
                INNER JOIN DICHVU dv ON cs.MaDichVu = dv.MaDichVu
                GROUP BY p.SoPhong, dv.TenDichVu
                ORDER BY TongTien DESC"
            )).ToList();
        }
        catch
        {
            chartsData.TopUtilities = [];
        }

        // 3. Hợp đồng sắp hết hạn (Chart 5) — JOIN trực tiếp các bảng gốc
        try
        {
            var expiringContracts = (await connection.QueryAsync<ExpiringContractData>(
                @"SELECT 
                    p.SoPhong, 
                    kt.HoTen AS KhachDaiDien, 
                    kt.SoDienThoai, 
                    hd.NgayKetThuc, 
                    DATEDIFF(day, GETDATE(), hd.NgayKetThuc) AS SoNgayConLai
                FROM HOPDONG hd
                INNER JOIN PHONGTRO p ON hd.MaPhong = p.MaPhong
                INNER JOIN KHACHTHUE kt ON hd.MaKhachDaiDien = kt.MaKhach
                WHERE LTRIM(RTRIM(hd.TrangThai)) IN (N'Hiệu lực', N'Hieu luc')
                  AND DATEDIFF(day, GETDATE(), hd.NgayKetThuc) <= 60
                  AND DATEDIFF(day, GETDATE(), hd.NgayKetThuc) >= 0
                ORDER BY SoNgayConLai ASC"
            )).ToList();

            foreach (var contract in expiringContracts)
            {
                contract.NhomCanhBao = contract.SoNgayConLai <= 30 ? "Nguy cấp" : "Cảnh báo";
            }

            chartsData.ExpiringContracts = expiringContracts;
        }
        catch
        {
            chartsData.ExpiringContracts = [];
        }

        return chartsData;
    }

    private static bool IsRoomStatus(string? current, params string[] accepted) =>
        accepted.Any(x => string.Equals(current?.Trim(), x, StringComparison.OrdinalIgnoreCase));
}
