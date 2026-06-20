DECLARE @MaHoaDon INT = 0;
DECLARE @SoTienThanhToan DECIMAL(18,0) = 0;

EXEC dbo.SP_GHI_NHAN_THANHTOAN
    @MaHoaDon = @MaHoaDon,
    @MaNguoiThu = 1,
    @SoTien = @SoTienThanhToan,
    @NgayThu = '2026-06-20',
    @HinhThuc = 'ChuyenKhoan';

SELECT TOP 1 MaThanhToan, MaHoaDon, MaNguoiThu, SoTien, NgayThu, HinhThuc
FROM THANHTOAN
WHERE MaHoaDon = @MaHoaDon
ORDER BY MaThanhToan DESC;

SELECT MaHoaDon, TongTien, DaThanhToan, ConLai, TrangThai
FROM HOADON
WHERE MaHoaDon = @MaHoaDon;
