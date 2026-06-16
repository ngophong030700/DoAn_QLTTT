using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class NguoiDungMockRepository : MockCrudRepository<NguoiDung>, INguoiDungRepository
{
    public NguoiDungMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<NguoiDung> Items => Data.NguoiDungs;
    protected override int GetId(NguoiDung entity) => entity.MaNguoiDung;
    protected override void SetId(NguoiDung entity, int id) => entity.MaNguoiDung = id;

    protected override bool Matches(NguoiDung entity, string keyword)
    {
        return entity.TenDangNhap.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || entity.HoTen.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || entity.VaiTro.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
