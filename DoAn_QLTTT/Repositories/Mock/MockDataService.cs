using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class MockDataService
{
    public List<LoaiPhong> LoaiPhongs { get; } =
    [
        new() { MaLoaiPhong = 1, TenLoai = "Phòng đơn", DienTich = 18, GiaThueCoSo = 1800000, SucChuaToiDa = 1 },
        new() { MaLoaiPhong = 2, TenLoai = "Phòng đôi", DienTich = 25, GiaThueCoSo = 2600000, SucChuaToiDa = 2 },
        new() { MaLoaiPhong = 3, TenLoai = "Phòng studio", DienTich = 32, GiaThueCoSo = 3600000, SucChuaToiDa = 3 }
    ];

    public List<PhongTro> PhongTros { get; } =
    [
        new() { MaPhong = 1, MaLoaiPhong = 1, SoPhong = "A101", GiaThue = 1800000, SucChuaToiDa = 1, TrangThai = AppStatuses.Phong.DangThue, GhiChu = "Gần cổng" },
        new() { MaPhong = 2, MaLoaiPhong = 2, SoPhong = "A102", GiaThue = 2600000, SucChuaToiDa = 2, TrangThai = AppStatuses.Phong.DangThue, GhiChu = "Có ban công" },
        new() { MaPhong = 3, MaLoaiPhong = 2, SoPhong = "B201", GiaThue = 2500000, SucChuaToiDa = 2, TrangThai = AppStatuses.Phong.Trong, GhiChu = "Mới sơn" },
        new() { MaPhong = 4, MaLoaiPhong = 3, SoPhong = "B202", GiaThue = 3600000, SucChuaToiDa = 3, TrangThai = AppStatuses.Phong.BaoTri, GhiChu = "Sửa điều hòa" }
    ];

    public List<NguoiDung> NguoiDungs { get; } =
    [
        new() { MaNguoiDung = 1, TenDangNhap = "admin", MatKhau = "demo", HoTen = "Quản trị viên", VaiTro = AppStatuses.VaiTro.Admin, DangHoatDong = true },
        new() { MaNguoiDung = 2, TenDangNhap = "staff01", MatKhau = "demo", HoTen = "Nhân viên thu ngân", VaiTro = AppStatuses.VaiTro.Staff, DangHoatDong = true }
    ];

    public List<KhachThue> KhachThues { get; } =
    [
        new() { MaKhach = 1, HoTen = "Nguyễn Minh An", CCCD = "079201000001", SoDienThoai = "0901000001", DiaChi = "Quận 7, TP.HCM" },
        new() { MaKhach = 2, HoTen = "Trần Gia Bảo", CCCD = "079201000002", SoDienThoai = "0901000002", DiaChi = "Thủ Đức, TP.HCM" },
        new() { MaKhach = 3, HoTen = "Lê Hoài Nam", CCCD = "079201000003", SoDienThoai = "0901000003", DiaChi = "Biên Hòa, Đồng Nai" }
    ];

    public List<HopDong> HopDongs { get; } =
    [
        new() { MaHopDong = 1, MaPhong = 1, MaKhachDaiDien = 1, NgayBatDau = new DateOnly(2026, 1, 1), NgayKetThuc = new DateOnly(2026, 12, 31), TienThueThang = 1800000, TienCoc = 1800000, TrangThai = AppStatuses.HopDong.HieuLuc },
        new() { MaHopDong = 2, MaPhong = 2, MaKhachDaiDien = 2, NgayBatDau = new DateOnly(2026, 2, 15), NgayKetThuc = new DateOnly(2027, 2, 14), TienThueThang = 2600000, TienCoc = 2600000, TrangThai = AppStatuses.HopDong.HieuLuc }
    ];

    public List<ChiTietHopDong> ChiTietHopDongs { get; } =
    [
        new() { MaHopDong = 1, MaKhach = 1, LaNguoiDaiDien = true },
        new() { MaHopDong = 2, MaKhach = 2, LaNguoiDaiDien = true },
        new() { MaHopDong = 2, MaKhach = 3, LaNguoiDaiDien = false }
    ];

    public List<DichVu> DichVus { get; } =
    [
        new() { MaDichVu = 1, TenDichVu = "Điện", DonVi = "kWh", DonGia = 4000, LoaiTinhPhi = "Theo chỉ số" },
        new() { MaDichVu = 2, TenDichVu = "Nước", DonVi = "m3", DonGia = 18000, LoaiTinhPhi = "Theo chỉ số" },
        new() { MaDichVu = 3, TenDichVu = "Internet", DonVi = "phòng", DonGia = 100000, LoaiTinhPhi = "Cố định" },
        new() { MaDichVu = 4, TenDichVu = "Rác", DonVi = "phòng", DonGia = 30000, LoaiTinhPhi = "Cố định" }
    ];

    public List<ChiSoDienNuoc> ChiSoDienNuocs { get; } =
    [
        new() { MaChiSo = 1, MaPhong = 1, MaDichVu = 1, MaNguoiNhap = 2, Thang = 6, Nam = 2026, ChiSoCu = 1200, ChiSoMoi = 1265, TieuThu = 65 },
        new() { MaChiSo = 2, MaPhong = 1, MaDichVu = 2, MaNguoiNhap = 2, Thang = 6, Nam = 2026, ChiSoCu = 310, ChiSoMoi = 321, TieuThu = 11 },
        new() { MaChiSo = 3, MaPhong = 2, MaDichVu = 1, MaNguoiNhap = 2, Thang = 6, Nam = 2026, ChiSoCu = 940, ChiSoMoi = 1010, TieuThu = 70 }
    ];

    public List<HoaDon> HoaDons { get; } =
    [
        new() { MaHoaDon = 1, MaHopDong = 1, MaNguoiLap = 2, Thang = 6, Nam = 2026, HanThanhToan = new DateOnly(2026, 6, 10), TrangThai = AppStatuses.HoaDon.MotPhan },
        new() { MaHoaDon = 2, MaHopDong = 2, MaNguoiLap = 2, Thang = 6, Nam = 2026, HanThanhToan = new DateOnly(2026, 6, 20), TrangThai = AppStatuses.HoaDon.ChuaThanhToan },
        new() { MaHoaDon = 3, MaHopDong = 1, MaNguoiLap = 2, Thang = 5, Nam = 2026, HanThanhToan = new DateOnly(2026, 5, 10), TrangThai = AppStatuses.HoaDon.DaThanhToan }
    ];

    public List<ChiTietHoaDon> ChiTietHoaDons { get; } =
    [
        new() { MaChiTiet = 1, MaHoaDon = 1, MaDichVu = 0, MoTa = "Tiền thuê phòng A101 tháng 6/2026", SoLuong = 1, DonGiaTaiThoiDiem = 1800000, ThanhTien = 1800000 },
        new() { MaChiTiet = 2, MaHoaDon = 1, MaDichVu = 1, MoTa = "Điện 65 kWh", SoLuong = 65, DonGiaTaiThoiDiem = 4000, ThanhTien = 260000 },
        new() { MaChiTiet = 3, MaHoaDon = 1, MaDichVu = 2, MoTa = "Nước 11 m3", SoLuong = 11, DonGiaTaiThoiDiem = 18000, ThanhTien = 198000 },
        new() { MaChiTiet = 4, MaHoaDon = 1, MaDichVu = 3, MoTa = "Internet", SoLuong = 1, DonGiaTaiThoiDiem = 100000, ThanhTien = 100000 },
        new() { MaChiTiet = 5, MaHoaDon = 2, MaDichVu = 0, MoTa = "Tiền thuê phòng A102 tháng 6/2026", SoLuong = 1, DonGiaTaiThoiDiem = 2600000, ThanhTien = 2600000 },
        new() { MaChiTiet = 6, MaHoaDon = 2, MaDichVu = 1, MoTa = "Điện 70 kWh", SoLuong = 70, DonGiaTaiThoiDiem = 4000, ThanhTien = 280000 },
        new() { MaChiTiet = 7, MaHoaDon = 3, MaDichVu = 0, MoTa = "Tiền thuê phòng A101 tháng 5/2026", SoLuong = 1, DonGiaTaiThoiDiem = 1800000, ThanhTien = 1800000 }
    ];

    public List<ThanhToan> ThanhToans { get; } =
    [
        new() { MaThanhToan = 1, MaHoaDon = 1, MaNguoiThu = 2, SoTien = 1200000, NgayThu = new DateOnly(2026, 6, 8), HinhThuc = "Chuyển khoản" },
        new() { MaThanhToan = 2, MaHoaDon = 3, MaNguoiThu = 2, SoTien = 1800000, NgayThu = new DateOnly(2026, 5, 6), HinhThuc = "Tiền mặt" }
    ];

    public MockDataService()
    {
        RebuildLookups();
    }

    public void RebuildLookups()
    {
        foreach (var phong in PhongTros)
        {
            phong.LoaiPhongTen = LoaiPhongs.FirstOrDefault(x => x.MaLoaiPhong == phong.MaLoaiPhong)?.TenLoai;
            if (phong.TrangThai != AppStatuses.Phong.BaoTri && HopDongs.Any(x => x.MaPhong == phong.MaPhong && x.TrangThai == AppStatuses.HopDong.HieuLuc))
            {
                phong.TrangThai = AppStatuses.Phong.DangThue;
            }
        }

        foreach (var hopDong in HopDongs)
        {
            hopDong.SoPhong = PhongTros.FirstOrDefault(x => x.MaPhong == hopDong.MaPhong)?.SoPhong;
            hopDong.KhachDaiDienHoTen = KhachThues.FirstOrDefault(x => x.MaKhach == hopDong.MaKhachDaiDien)?.HoTen;
        }

        foreach (var chiSo in ChiSoDienNuocs)
        {
            chiSo.SoPhong = PhongTros.FirstOrDefault(x => x.MaPhong == chiSo.MaPhong)?.SoPhong;
            chiSo.DichVuTen = DichVus.FirstOrDefault(x => x.MaDichVu == chiSo.MaDichVu)?.TenDichVu;
            chiSo.NguoiNhapHoTen = NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == chiSo.MaNguoiNhap)?.HoTen;
            chiSo.TieuThu = Math.Max(0, chiSo.ChiSoMoi - chiSo.ChiSoCu);
        }

        foreach (var chiTiet in ChiTietHoaDons)
        {
            chiTiet.TenDichVu = DichVus.FirstOrDefault(x => x.MaDichVu == chiTiet.MaDichVu)?.TenDichVu ?? "Tiền phòng";
            chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGiaTaiThoiDiem;
        }

        foreach (var hoaDon in HoaDons)
        {
            var hopDong = HopDongs.FirstOrDefault(x => x.MaHopDong == hoaDon.MaHopDong);
            hoaDon.HopDongMoTa = hopDong is null ? $"HĐ #{hoaDon.MaHopDong}" : $"HĐ #{hopDong.MaHopDong} - Phòng {hopDong.SoPhong}";
            hoaDon.SoPhong = hopDong?.SoPhong;
            hoaDon.KhachDaiDienHoTen = hopDong?.KhachDaiDienHoTen;
            hoaDon.NguoiLapHoTen = NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == hoaDon.MaNguoiLap)?.HoTen;

            // Demo phần việc thường do trigger xử lý: tổng tiền, đã thanh toán, còn lại, trạng thái công nợ.
            hoaDon.TongTien = ChiTietHoaDons.Where(x => x.MaHoaDon == hoaDon.MaHoaDon).Sum(x => x.ThanhTien);
            hoaDon.DaThanhToan = ThanhToans.Where(x => x.MaHoaDon == hoaDon.MaHoaDon).Sum(x => x.SoTien);
            hoaDon.ConLai = Math.Max(0, hoaDon.TongTien - hoaDon.DaThanhToan);
            hoaDon.TrangThai = ResolveHoaDonStatus(hoaDon);
        }

        foreach (var thanhToan in ThanhToans)
        {
            thanhToan.NguoiThuHoTen = NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == thanhToan.MaNguoiThu)?.HoTen;
            var hoaDon = HoaDons.FirstOrDefault(x => x.MaHoaDon == thanhToan.MaHoaDon);
            thanhToan.HoaDonMoTa = hoaDon is null ? $"HĐơn #{thanhToan.MaHoaDon}" : $"HĐơn #{hoaDon.MaHoaDon} - {hoaDon.Thang}/{hoaDon.Nam}";
        }
    }

    private static string ResolveHoaDonStatus(HoaDon hoaDon)
    {
        if (hoaDon.ConLai <= 0)
        {
            return AppStatuses.HoaDon.DaThanhToan;
        }

        if (hoaDon.HanThanhToan < DateOnly.FromDateTime(DateTime.Today))
        {
            return AppStatuses.HoaDon.QuaHan;
        }

        return hoaDon.DaThanhToan > 0 ? AppStatuses.HoaDon.MotPhan : AppStatuses.HoaDon.ChuaThanhToan;
    }
}
