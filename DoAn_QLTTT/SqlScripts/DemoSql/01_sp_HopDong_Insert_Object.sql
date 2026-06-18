CREATE OR ALTER PROCEDURE dbo.sp_HopDong_Insert
    @MaPhong NVARCHAR(20),
    @MaKhachThue NVARCHAR(20),
    @NgayLap DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PhongTro
        WHERE MaPhong = @MaPhong
          AND TrangThai = N'Trong'
    )
    BEGIN
        THROW 50001, 'Phong khong o trang thai trong.', 1;
    END;

    INSERT INTO dbo.HopDong (MaPhong, MaKhachThue, NgayLap, TrangThai)
    VALUES (@MaPhong, @MaKhachThue, ISNULL(@NgayLap, GETDATE()), N'Hieu luc');

    SELECT SCOPE_IDENTITY() AS MaHopDongMoi;
END;
