-- Placeholder: Trigger - Cap nhat trang thai phong khi lap hop dong
-- Trigger khong goi truc tiep tu code. Trigger se tu chay khi INSERT/UPDATE bang HopDong.

INSERT INTO dbo.HopDong (MaPhong, MaKhachThue, NgayLap, TrangThai)
VALUES ('P102', 'KT001', GETDATE(), N'Hieu luc');
