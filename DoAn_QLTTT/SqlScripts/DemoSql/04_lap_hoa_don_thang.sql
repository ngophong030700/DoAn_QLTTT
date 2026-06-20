DECLARE @MaHopDong INT = 0;
DECLARE @MaHoaDonMoi INT;

EXEC dbo.SP_LAP_HOADON_THANG
    @MaHopDong = @MaHopDong,
    @MaNguoiLap = 1,
    @Thang = 6,
    @Nam = 2026,
    @MaHoaDonMoi = @MaHoaDonMoi OUTPUT;

SELECT @MaHoaDonMoi AS MaHoaDonMoi;

SELECT MaHoaDon, MaHopDong, Thang, Nam, TongTien, DaThanhToan, ConLai, HanThanhToan, TrangThai
FROM HOADON
WHERE MaHoaDon = @MaHoaDonMoi;

SELECT
    CT.MaChiTiet,
    CT.MaHoaDon,
    DV.TenDichVu,
    CT.MoTa,
    CT.SoLuong,
    CT.DonGiaTaiThoiDiem,
    CT.ThanhTien
FROM CHITIETHOADON CT
INNER JOIN DICHVU DV ON CT.MaDichVu = DV.MaDichVu
WHERE CT.MaHoaDon = @MaHoaDonMoi
ORDER BY CT.MaChiTiet;
