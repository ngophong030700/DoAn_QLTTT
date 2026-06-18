DECLARE @MaHopDong INT;

SELECT TOP 1 @MaHopDong = MaHopDong
FROM HOPDONG
ORDER BY MaHopDong DESC;

SELECT
    @MaHopDong AS MaHopDong,
    dbo.FN_TONG_CONGNO_HOPDONG(@MaHopDong) AS TongCongNo;
