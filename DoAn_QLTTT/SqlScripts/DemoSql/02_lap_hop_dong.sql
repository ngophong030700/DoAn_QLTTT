DECLARE @MaPhong INT;
DECLARE @MaKhachDaiDien INT;
DECLARE @MaHopDongMoi INT;

SELECT TOP 1 @MaPhong = MaPhong
FROM PHONGTRO
WHERE TrangThai = N'Trống'
ORDER BY MaPhong;

SELECT @MaKhachDaiDien = MaKhach
FROM KHACHTHUE
WHERE CCCD = '079199999999';

EXEC dbo.SP_LAP_HOPDONG
    @MaPhong = @MaPhong,
    @MaKhachDaiDien = @MaKhachDaiDien,
    @NgayBatDau = '2026-06-18',
    @NgayKetThuc = '2027-06-18',
    @TienCoc = 3000000,
    @MaHopDongMoi = @MaHopDongMoi OUTPUT;

SELECT
    @MaHopDongMoi AS MaHopDongMoi,
    dbo.FN_SO_NGUOI_TRONG_HOPDONG(@MaHopDongMoi) AS SoNguoiTrongHopDong;

SELECT
    HD.MaHopDong,
    HD.MaPhong,
    P.SoPhong,
    HD.MaKhachDaiDien,
    KT.HoTen,
    HD.NgayBatDau,
    HD.NgayKetThuc,
    HD.TienThueThang,
    HD.TienCoc,
    HD.TrangThai
FROM HOPDONG HD
INNER JOIN PHONGTRO P ON HD.MaPhong = P.MaPhong
INNER JOIN KHACHTHUE KT ON HD.MaKhachDaiDien = KT.MaKhach
WHERE HD.MaHopDong = @MaHopDongMoi;

SELECT P.MaPhong, P.SoPhong, P.TrangThai
FROM PHONGTRO P
WHERE P.MaPhong = @MaPhong;
