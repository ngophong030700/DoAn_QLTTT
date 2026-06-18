DECLARE @HopDongCanQuet TABLE (MaHopDong INT PRIMARY KEY);

INSERT INTO @HopDongCanQuet(MaHopDong)
SELECT MaHopDong
FROM HOPDONG HD
WHERE HD.TrangThai = N'Hiệu lực'
    AND HD.NgayKetThuc < CAST(GETDATE() AS DATE)
    AND NOT EXISTS
    (
        SELECT 1
        FROM HOADON
        WHERE HOADON.MaHopDong = HD.MaHopDong
            AND HOADON.ConLai > 0
    );

EXEC dbo.SP_QUET_HOPDONG_HETHAN;

SELECT
    HD.MaHopDong,
    HD.MaPhong,
    P.SoPhong,
    HD.NgayKetThuc,
    HD.TrangThai,
    dbo.FN_TONG_CONGNO_HOPDONG(HD.MaHopDong) AS TongCongNo,
    P.TrangThai AS TrangThaiPhong
FROM HOPDONG HD
INNER JOIN PHONGTRO P ON HD.MaPhong = P.MaPhong
INNER JOIN @HopDongCanQuet Q ON HD.MaHopDong = Q.MaHopDong
ORDER BY HD.MaHopDong;
