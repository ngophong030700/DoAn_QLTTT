using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class LoaiPhongMockRepository : MockCrudRepository<LoaiPhong>, ILoaiPhongRepository
{
    public LoaiPhongMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<LoaiPhong> Items => Data.LoaiPhongs;
    protected override int GetId(LoaiPhong entity) => entity.MaLoaiPhong;
    protected override void SetId(LoaiPhong entity, int id) => entity.MaLoaiPhong = id;

    protected override bool Matches(LoaiPhong entity, string keyword)
    {
        return entity.TenLoai.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
