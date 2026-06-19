# DoAn_QLTTT - Quản lý thuê phòng trọ

Dự án ASP.NET Core MVC .NET 8 cho nghiệp vụ quản lý phòng trọ: phòng, khách thuê, hợp đồng, chỉ số điện nước, hóa đơn, thanh toán và nhắc nợ.

Connection string mặc định:

```json
"DefaultConnection": "Server=KYDAT\\SQLEXPRESS;Database=QL_ThuePhongTro;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
```

Script tạo database, bảng, dữ liệu mẫu, stored procedure, function, trigger và cursor nằm tại:

```text
Planning/SQLScript_QL_ThuePhongTro.sql
```

## SQLDemo

Màn `SqlDemo/Index.cshtml` dùng để trình bày và chạy các kịch bản SQL theo format:

- B1: Trình bày bài toán
- B2: Câu truy vấn SQL có thể chỉnh sửa
- B3: Dữ liệu trước khi chạy
- B4: Nút thực thi
- B5: Kết quả sau khi chạy

Các câu SQL mặc định cho B2 được lưu tại:

```text
DoAn_QLTTT/SqlScripts/DemoSql/
```

Danh sách kịch bản hiện có:

1. `01_them_khach_thue.sql`
2. `02_lap_hop_dong.sql`
3. `03_ghi_chi_so.sql`
4. `04_lap_hoa_don_thang.sql`
5. `05_ghi_nhan_thanh_toan.sql`
6. `06_tinh_cong_no_hop_dong.sql`
7. `07_quet_hoa_don_qua_han.sql`
8. `08_quet_hop_dong_het_han.sql`

Luồng thực thi:

- B2 đọc SQL mặc định từ file `.sql`.
- Người dùng có thể sửa SQL trực tiếp trên giao diện.
- Khi bấm `Thực thi`, server chạy đúng nội dung SQL đang có trong textarea.
- Nếu SQL có `SELECT`, B5 hiển thị result set trả về từ chính câu vừa chạy.
- Các script tạo/cập nhật dữ liệu đã được viết để `SELECT` lại đúng dòng vừa tạo hoặc vừa bị tác động.

## Lưu ý khi demo

- SQLDemo chạy trực tiếp trên database thật, nên các lệnh `INSERT`, `UPDATE`, `EXEC` sẽ thay đổi dữ liệu.
- Nếu chạy lại cùng kịch bản nhiều lần, cần chú ý các ràng buộc unique như CCCD, hóa đơn trùng tháng/năm, chỉ số trùng phòng/dịch vụ/tháng/năm.
- Nếu cần reset dữ liệu demo, chạy lại script database trong `Planning/SQLScript_QL_ThuePhongTro.sql`.

## Build

```powershell
dotnet build DoAn_QLTTT.sln --configuration Release --no-restore
```

Build gần nhất đã kiểm tra thành công với `0 Warning(s), 0 Error(s)`.
