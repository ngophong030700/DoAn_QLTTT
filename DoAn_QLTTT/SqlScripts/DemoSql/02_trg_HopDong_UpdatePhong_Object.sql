CREATE OR ALTER TRIGGER dbo.trg_HopDong_UpdatePhong
ON dbo.HopDong
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET TrangThai = N'Dang thue'
    FROM dbo.PhongTro p
    INNER JOIN inserted i ON i.MaPhong = p.MaPhong
    WHERE i.TrangThai = N'Hieu luc';

    UPDATE p
    SET TrangThai = N'Trong'
    FROM dbo.PhongTro p
    INNER JOIN inserted i ON i.MaPhong = p.MaPhong
    WHERE i.TrangThai <> N'Hieu luc'
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.HopDong hd
          WHERE hd.MaPhong = p.MaPhong
            AND hd.TrangThai = N'Hieu luc'
      );
END;
