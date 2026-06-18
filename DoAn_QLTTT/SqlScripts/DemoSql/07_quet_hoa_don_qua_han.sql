DECLARE @HoaDonCanQuet TABLE (MaHoaDon INT PRIMARY KEY);

INSERT INTO @HoaDonCanQuet(MaHoaDon)
SELECT MaHoaDon
FROM HOADON
WHERE ConLai > 0
    AND TrangThai <> N'Đã TT'
    AND HanThanhToan < CAST(GETDATE() AS DATE);

EXEC dbo.SP_QUET_HOADON_QUAHAN;

SELECT HD.MaHoaDon, HD.MaHopDong, HD.TongTien, HD.DaThanhToan, HD.ConLai, HD.HanThanhToan, HD.TrangThai
FROM HOADON HD
INNER JOIN @HoaDonCanQuet Q ON HD.MaHoaDon = Q.MaHoaDon
ORDER BY HD.HanThanhToan;
