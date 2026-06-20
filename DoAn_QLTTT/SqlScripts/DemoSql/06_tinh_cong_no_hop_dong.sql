DECLARE @MaHopDong INT = 0;

SELECT
    @MaHopDong AS MaHopDong,
    dbo.FN_TONG_CONGNO_HOPDONG(@MaHopDong) AS TongCongNo;
