using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class KhachThueMockRepository : MockCrudRepository<KhachThue>, IKhachThueRepository
{
    public KhachThueMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<KhachThue> Items => Data.KhachThues;
    protected override int GetId(KhachThue entity) => entity.MaKhach;
    protected override void SetId(KhachThue entity, int id) => entity.MaKhach = id;

    protected override bool Matches(KhachThue entity, string keyword)
    {
        return entity.HoTen.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || entity.CCCD.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || entity.SoDienThoai.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || entity.DiaChi.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
