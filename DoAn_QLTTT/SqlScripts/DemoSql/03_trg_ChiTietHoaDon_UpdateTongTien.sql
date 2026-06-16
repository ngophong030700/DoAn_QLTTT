-- Placeholder: Trigger - Cap nhat tong tien hoa don
-- Trigger tren ChiTietHoaDon se roll-up TongTien va ConLai cho HoaDon.

INSERT INTO dbo.ChiTietHoaDon (MaHoaDon, LoaiPhi, ThanhTien)
VALUES
    ('HD001', N'Tien phong', 2500000),
    ('HD001', N'Tien dien', 180000),
    ('HD001', N'Tien nuoc', 90000),
    ('HD001', N'Wifi', 100000);
