DECLARE @MaPhong INT = 0;
DECLARE @MaDichVu INT = 0;
DECLARE @ChiSoCu INT;
DECLARE @ChiSoMoi INT = 0;

SELECT TOP 1 @ChiSoCu = ChiSoMoi
FROM CHISODIENNUOC
WHERE MaPhong = @MaPhong
    AND MaDichVu = @MaDichVu
    AND DATEFROMPARTS(Nam, Thang, 1) < DATEFROMPARTS(2026, 6, 1)
ORDER BY Nam DESC, Thang DESC;

SET @ChiSoCu = ISNULL(@ChiSoCu, 0);

EXEC dbo.SP_GHI_CHISO_DICHVU
    @MaPhong = @MaPhong,
    @MaDichVu = @MaDichVu,
    @MaNguoiNhap = 1,
    @Thang = 6,
    @Nam = 2026,
    @ChiSoMoi = @ChiSoMoi;

SELECT dbo.FN_TINH_TIEN_CHISO(@MaPhong, @MaDichVu, 6, 2026) AS TienDichVuThang;

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
    AND CS.MaDichVu = @MaDichVu
    AND CS.Thang = 6
    AND CS.Nam = 2026
ORDER BY CS.MaChiSo DESC;
