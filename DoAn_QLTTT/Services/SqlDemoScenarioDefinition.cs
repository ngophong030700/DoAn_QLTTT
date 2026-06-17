namespace DoAn_QLTTT.Services;

internal record SqlDemoScenarioDefinition(
    string Id,
    string Title,
    string Type,
    string ScriptFileName,
    string ObjectScriptFileName,
    string Problem,
    string Note,
    string ExecuteName);

internal static class SqlDemoScenarioCatalog
{
    public static readonly IReadOnlyList<SqlDemoScenarioDefinition> Items =
    [
        new(
            "lap-hop-dong",
            "Procedure - Lap hop dong thue phong",
            "Procedure",
            "01_sp_HopDong_Insert.sql",
            "01_sp_HopDong_Insert_Object.sql",
            "Lap hop dong thue phong cho khach da ton tai, dam bao phong dang trong truoc khi tao hop dong hieu luc.",
            "Sau nay thao tac nay se goi sp_HopDong_Insert va trigger cap nhat trang thai phong.",
            "sp_HopDong_Insert"),
        new(
            "trigger-trang-thai-phong",
            "Trigger - Cap nhat trang thai phong khi lap hop dong",
            "Trigger",
            "02_trg_HopDong_UpdatePhong.sql",
            "02_trg_HopDong_UpdatePhong_Object.sql",
            "Kiem chung trigger tu dong cap nhat trang thai phong khi co hop dong hieu luc moi.",
            "Trigger khong goi truc tiep tu code, ma tu chay khi INSERT/UPDATE bang HopDong.",
            "sp_Demo_LapHopDongChoTrigger"),
        new(
            "trigger-tong-tien-hoa-don",
            "Trigger - Cap nhat tong tien hoa don",
            "Trigger",
            "03_trg_ChiTietHoaDon_UpdateTongTien.sql",
            "03_trg_ChiTietHoaDon_UpdateTongTien_Object.sql",
            "Them chi tiet hoa don va kiem tra TongTien, ConLai cua hoa don duoc roll-up tu dong.",
            "Sau nay trigger tren ChiTietHoaDon se tu roll-up TongTien, ConLai.",
            "sp_Demo_ThemChiTietHoaDon"),
        new(
            "ghi-nhan-thanh-toan",
            "Procedure - Ghi nhan thanh toan",
            "Procedure",
            "04_sp_ThanhToan_Insert.sql",
            "04_sp_ThanhToan_Insert_Object.sql",
            "Ghi nhan mot khoan thanh toan cho hoa don con no va cap nhat trang thai thanh toan.",
            "Sau nay thao tac nay se goi sp_ThanhToan_Insert.",
            "sp_ThanhToan_Insert"),
        new(
            "function-tinh-cong-no",
            "Function - Tinh cong no",
            "Function",
            "05_fn_TinhCongNo.sql",
            "05_fn_TinhCongNo_Object.sql",
            "Tinh cong no con lai cua hoa don theo cong thuc TongTien - DaThanhToan.",
            "Sau nay se goi fn_TinhCongNo tu SQL Server.",
            "fn_TinhCongNo"),
        new(
            "cursor-quet-qua-han",
            "Cursor - Quet hoa don qua han",
            "Cursor",
            "06_sp_HoaDon_QuetQuaHan.sql",
            "06_sp_HoaDon_QuetQuaHan_Object.sql",
            "Quet danh sach hoa don chua thanh toan da qua han va chuyen trang thai sang Qua han.",
            "Sau nay se goi sp_HoaDon_QuetQuaHan hoac cursor xu ly trong SQL Server.",
            "sp_HoaDon_QuetQuaHan")
    ];

    public static SqlDemoScenarioDefinition Find(string? id)
    {
        return Items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) ?? Items[0];
    }
}
