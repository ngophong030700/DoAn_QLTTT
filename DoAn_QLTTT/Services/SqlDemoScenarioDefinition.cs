namespace DoAn_QLTTT.Services;

internal record SqlDemoScenarioDefinition(
    string Id,
    string Title,
    string Type,
    string ScriptFileName,
    string Problem,
    string Note,
    string ExecuteName);

internal static class SqlDemoScenarioCatalog
{
    public static readonly IReadOnlyList<SqlDemoScenarioDefinition> Items =
    [
        new(
            "them-khach-thue",
            "Tiếp nhận khách thuê mới",
            "Stored Procedure",
            "01_them_khach_thue.sql",
            "Một khách mới đến đăng ký thuê phòng. Nhân viên nhập họ tên, CCCD, số điện thoại và địa chỉ. Procedure kiểm tra CCCD không trùng và trả về mã khách mới.",
            "Object chính: SP_THEM_KHACHTHUE. Kết quả dùng tiếp cho bước lập hợp đồng.",
            "SP_THEM_KHACHTHUE"),
        new(
            "lap-hop-dong",
            "Lập hợp đồng và cập nhật phòng",
            "Procedure + Trigger + Function",
            "02_lap_hop_dong.sql",
            "Nhân viên chọn một phòng đang trống để lập hợp đồng cho khách đại diện. Sau khi hợp đồng hiệu lực được tạo, phòng phải tự chuyển sang trạng thái đang thuê.",
            "Object chính: SP_LAP_HOPDONG, TRG_HOPDONG_LAP_CAPNHAT_PHONG, FN_SO_NGUOI_TRONG_HOPDONG.",
            "SP_LAP_HOPDONG"),
        new(
            "ghi-chi-so",
            "Ghi chỉ số và tính tiền điện nước",
            "Procedure + Trigger + Function",
            "03_ghi_chi_so.sql",
            "Cuối tháng, nhân viên nhập chỉ số điện hoặc nước cho phòng đang thuê. Hệ thống tự lấy chỉ số cũ, tính tiêu thụ và tính tiền theo đơn giá dịch vụ.",
            "Object chính: SP_GHI_CHISO_DICHVU, TRG_CHISODIENNUOC_TINH_TIEUTHU, FN_TINH_TIEN_CHISO.",
            "SP_GHI_CHISO_DICHVU"),
        new(
            "lap-hoa-don-thang",
            "Lập hóa đơn tháng",
            "Procedure + Trigger",
            "04_lap_hoa_don_thang.sql",
            "Sau khi có hợp đồng hiệu lực và chỉ số dịch vụ, nhân viên lập hóa đơn tháng. Hệ thống tự thêm tiền phòng, dịch vụ theo chỉ số, dịch vụ cố định và cập nhật tổng tiền.",
            "Object chính: SP_LAP_HOADON_THANG, TRG_CHITIETHOADON_CAPNHAT_TONGTIEN.",
            "SP_LAP_HOADON_THANG"),
        new(
            "ghi-nhan-thanh-toan",
            "Ghi nhận thanh toán",
            "Procedure + Trigger",
            "05_ghi_nhan_thanh_toan.sql",
            "Khách thuê thanh toán một phần hoặc toàn bộ hóa đơn. Procedure ghi nhận phiếu thu, trigger cập nhật đã thanh toán, còn lại và trạng thái hóa đơn.",
            "Object chính: SP_GHI_NHAN_THANHTOAN, TRG_THANHTOAN_CAPNHAT_HOADON.",
            "SP_GHI_NHAN_THANHTOAN"),
        new(
            "tinh-cong-no-hop-dong",
            "Tính tổng công nợ hợp đồng",
            "Function",
            "06_tinh_cong_no_hop_dong.sql",
            "Chủ nhà trọ cần biết một hợp đồng còn nợ tổng cộng bao nhiêu để nhắc nợ hoặc quyết định có cho kết thúc hợp đồng hay không.",
            "Object chính: FN_TONG_CONGNO_HOPDONG. Output bằng tổng ConLai của các hóa đơn thuộc hợp đồng.",
            "FN_TONG_CONGNO_HOPDONG"),
        new(
            "quet-hoa-don-qua-han",
            "Quét hóa đơn quá hạn",
            "Cursor",
            "07_quet_hoa_don_qua_han.sql",
            "Hệ thống định kỳ quét các hóa đơn còn nợ và đã quá hạn thanh toán để chuyển trạng thái sang quá hạn.",
            "Object chính: SP_QUET_HOADON_QUAHAN, cursor cur_HoaDonQuaHan.",
            "SP_QUET_HOADON_QUAHAN"),
        new(
            "quet-hop-dong-het-han",
            "Quét hợp đồng hết hạn",
            "Cursor + Trigger",
            "08_quet_hop_dong_het_han.sql",
            "Hệ thống rà soát hợp đồng đã hết hạn. Nếu hợp đồng không còn công nợ, hệ thống kết thúc hợp đồng và giải phóng phòng.",
            "Object chính: SP_QUET_HOPDONG_HETHAN, cur_HopDongHetHan, TRG_HOPDONG_KETTHUC_CAPNHAT_PHONG.",
            "SP_QUET_HOPDONG_HETHAN")
    ];

    public static SqlDemoScenarioDefinition Find(string? id)
    {
        return Items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) ?? Items[0];
    }
}
