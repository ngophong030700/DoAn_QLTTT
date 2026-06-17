CREATE OR ALTER PROCEDURE dbo.sp_ThanhToan_Insert
    @MaHoaDon NVARCHAR(20),
    @SoTien DECIMAL(18, 2),
    @PhuongThuc NVARCHAR(50),
    @NgayThu DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @SoTien <= 0
    BEGIN
        THROW 50002, 'So tien thanh toan phai lon hon 0.', 1;
    END;

    INSERT INTO dbo.ThanhToan (MaHoaDon, SoTien, PhuongThuc, NgayThu)
    VALUES (@MaHoaDon, @SoTien, @PhuongThuc, ISNULL(@NgayThu, GETDATE()));

    UPDATE hd
    SET
        DaThanhToan = hd.DaThanhToan + @SoTien,
        ConLai = hd.TongTien - (hd.DaThanhToan + @SoTien),
        TrangThai = CASE
            WHEN hd.TongTien - (hd.DaThanhToan + @SoTien) <= 0 THEN N'Da TT'
            ELSE N'Mot phan'
        END
    FROM dbo.HoaDon hd
    WHERE hd.MaHoaDon = @MaHoaDon;
END;
