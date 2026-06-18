DECLARE @MaKhachMoi INT;

EXEC dbo.SP_THEM_KHACHTHUE
    @HoTen = N'Nguyễn Văn Demo',
    @CCCD = '079199999999',
    @SoDienThoai = '0909999999',
    @DiaChi = N'TP.HCM',
    @MaKhachMoi = @MaKhachMoi OUTPUT;

SELECT @MaKhachMoi AS MaKhachMoi;

SELECT MaKhach, HoTen, CCCD, SoDienThoai, DiaChi
FROM KHACHTHUE
WHERE MaKhach = @MaKhachMoi;
