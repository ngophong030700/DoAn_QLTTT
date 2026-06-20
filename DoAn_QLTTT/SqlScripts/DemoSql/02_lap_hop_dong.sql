DECLARE @MaPhong INT = 0;
DECLARE @MaKhachDaiDien INT = 0;
DECLARE @MaHopDongMoi INT;

EXEC dbo.SP_LAP_HOPDONG
    @MaPhong = @MaPhong,
    @MaKhachDaiDien = @MaKhachDaiDien,
    @NgayBatDau = '2026-06-20',
    @NgayKetThuc = '2027-06-20',
    @TienCoc = 0,
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
