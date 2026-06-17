CREATE OR ALTER PROCEDURE dbo.sp_HoaDon_QuetQuaHan
    @NgayQuet DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaHoaDon NVARCHAR(20);
    DECLARE @NgayKiemTra DATE = CAST(ISNULL(@NgayQuet, GETDATE()) AS DATE);

    DECLARE cur_HoaDonQuaHan CURSOR LOCAL FAST_FORWARD FOR
        SELECT MaHoaDon
        FROM dbo.HoaDon
        WHERE ConLai > 0
          AND HanThanhToan < @NgayKiemTra
          AND TrangThai <> N'Qua han';

    OPEN cur_HoaDonQuaHan;
    FETCH NEXT FROM cur_HoaDonQuaHan INTO @MaHoaDon;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        UPDATE dbo.HoaDon
        SET TrangThai = N'Qua han'
        WHERE MaHoaDon = @MaHoaDon;

        FETCH NEXT FROM cur_HoaDonQuaHan INTO @MaHoaDon;
    END;

    CLOSE cur_HoaDonQuaHan;
    DEALLOCATE cur_HoaDonQuaHan;
END;
