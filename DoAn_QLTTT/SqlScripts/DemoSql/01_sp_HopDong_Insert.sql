-- Placeholder: Procedure - Lap hop dong thue phong
-- Sau nay thay bang stored procedure that: sp_HopDong_Insert.

EXEC dbo.sp_HopDong_Insert
    @MaPhong = 'P101',
    @MaKhachThue = 'KT001',
    @NgayLap = GETDATE();
