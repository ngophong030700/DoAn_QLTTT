-- Placeholder: Procedure - Ghi nhan thanh toan
-- Sau nay thay bang stored procedure that: sp_ThanhToan_Insert.

EXEC dbo.sp_ThanhToan_Insert
    @MaHoaDon = 'HD002',
    @SoTien = 1500000,
    @PhuongThuc = N'Chuyen khoan';
