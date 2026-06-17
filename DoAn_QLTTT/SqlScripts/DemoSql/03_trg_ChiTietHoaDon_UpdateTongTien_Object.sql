CREATE OR ALTER TRIGGER dbo.trg_ChiTietHoaDon_UpdateTongTien
ON dbo.ChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH ChangedInvoices AS (
        SELECT MaHoaDon FROM inserted
        UNION
        SELECT MaHoaDon FROM deleted
    ),
    InvoiceTotals AS (
        SELECT
            hd.MaHoaDon,
            TongTienMoi = ISNULL(SUM(ct.ThanhTien), 0)
        FROM dbo.HoaDon hd
        INNER JOIN ChangedInvoices c ON c.MaHoaDon = hd.MaHoaDon
        LEFT JOIN dbo.ChiTietHoaDon ct ON ct.MaHoaDon = hd.MaHoaDon
        GROUP BY hd.MaHoaDon
    )
    UPDATE hd
    SET
        TongTien = t.TongTienMoi,
        ConLai = t.TongTienMoi - hd.DaThanhToan,
        TrangThai = CASE
            WHEN t.TongTienMoi - hd.DaThanhToan <= 0 THEN N'Da TT'
            WHEN hd.DaThanhToan > 0 THEN N'Mot phan'
            ELSE N'Chua TT'
        END
    FROM dbo.HoaDon hd
    INNER JOIN InvoiceTotals t ON t.MaHoaDon = hd.MaHoaDon;
END;
