DECLARE @MaHopDong INT;
DECLARE @MaHoaDonMoi INT;

SELECT TOP 1 @MaHopDong = MaHopDong
FROM HOPDONG
WHERE TrangThai = N'Hiệu lực'
ORDER BY MaHopDong DESC;

EXEC dbo.SP_LAP_HOADON_THANG
    @MaHopDong = @MaHopDong,
    @MaNguoiLap = 1,
    @Thang = 7,
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
