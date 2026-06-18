DECLARE @MaPhong INT;
DECLARE @MaDichVuDien INT;
DECLARE @ChiSoCu INT;

SELECT TOP 1 @MaPhong = MaPhong
FROM HOPDONG
WHERE TrangThai = N'Hiệu lực'
ORDER BY MaHopDong DESC;

SELECT @MaDichVuDien = MaDichVu
FROM DICHVU
WHERE TenDichVu = N'Điện';

SELECT TOP 1 @ChiSoCu = ChiSoMoi
FROM CHISODIENNUOC
WHERE MaPhong = @MaPhong
    AND MaDichVu = @MaDichVuDien
    AND DATEFROMPARTS(Nam, Thang, 1) < DATEFROMPARTS(2026, 7, 1)
ORDER BY Nam DESC, Thang DESC;

SET @ChiSoCu = ISNULL(@ChiSoCu, 0);

EXEC dbo.SP_GHI_CHISO_DICHVU
    @MaPhong = @MaPhong,
    @MaDichVu = @MaDichVuDien,
    @MaNguoiNhap = 1,
    @Thang = 7,
    @Nam = 2026,
    @ChiSoMoi = @ChiSoCu + 80;

SELECT dbo.FN_TINH_TIEN_CHISO(@MaPhong, @MaDichVuDien, 7, 2026) AS TienDienThang;

SELECT TOP 1
    CS.MaChiSo,
    CS.MaPhong,
    DV.TenDichVu,
    CS.Thang,
    CS.Nam,
    CS.ChiSoCu,
    CS.ChiSoMoi,
    CS.TieuThu
FROM CHISODIENNUOC CS
INNER JOIN DICHVU DV ON CS.MaDichVu = DV.MaDichVu
WHERE CS.MaPhong = @MaPhong
    AND CS.MaDichVu = @MaDichVuDien
    AND CS.Thang = 7
    AND CS.Nam = 2026
ORDER BY CS.MaChiSo DESC;
