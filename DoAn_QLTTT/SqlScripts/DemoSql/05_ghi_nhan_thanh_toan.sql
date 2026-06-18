DECLARE @MaHoaDon INT;
DECLARE @SoTienThanhToan DECIMAL(18,0);

SELECT TOP 1
    @MaHoaDon = MaHoaDon,
    @SoTienThanhToan =
        CASE
            WHEN ConLai >= 1000000 THEN 1000000
            ELSE ConLai
        END
FROM HOADON
WHERE ConLai > 0
ORDER BY MaHoaDon DESC;

EXEC dbo.SP_GHI_NHAN_THANHTOAN
    @MaHoaDon = @MaHoaDon,
    @MaNguoiThu = 1,
    @SoTien = @SoTienThanhToan,
    @NgayThu = '2026-06-18',
    @HinhThuc = 'ChuyenKhoan';

SELECT TOP 1 MaThanhToan, MaHoaDon, MaNguoiThu, SoTien, NgayThu, HinhThuc
FROM THANHTOAN
WHERE MaHoaDon = @MaHoaDon
ORDER BY MaThanhToan DESC;

SELECT MaHoaDon, TongTien, DaThanhToan, ConLai, TrangThai
FROM HOADON
WHERE MaHoaDon = @MaHoaDon;
