CREATE OR ALTER FUNCTION dbo.fn_TinhCongNo
(
    @MaHoaDon NVARCHAR(20)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @CongNo DECIMAL(18, 2);

    SELECT @CongNo = TongTien - DaThanhToan
    FROM dbo.HoaDon
    WHERE MaHoaDon = @MaHoaDon;

    RETURN ISNULL(@CongNo, 0);
END;
