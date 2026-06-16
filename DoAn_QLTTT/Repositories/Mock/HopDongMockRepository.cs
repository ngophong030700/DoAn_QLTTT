using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class HopDongMockRepository : MockCrudRepository<HopDong>, IHopDongRepository
{
    public HopDongMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<HopDong> Items => Data.HopDongs;
    protected override int GetId(HopDong entity) => entity.MaHopDong;
    protected override void SetId(HopDong entity, int id) => entity.MaHopDong = id;

    protected override bool Matches(HopDong entity, string keyword)
    {
        return (entity.SoPhong?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || (entity.KhachDaiDienHoTen?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || entity.TrangThai.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
