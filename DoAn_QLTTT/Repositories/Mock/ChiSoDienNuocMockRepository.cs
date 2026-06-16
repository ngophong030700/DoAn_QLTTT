using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class ChiSoDienNuocMockRepository : MockCrudRepository<ChiSoDienNuoc>, IChiSoDienNuocRepository
{
    public ChiSoDienNuocMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<ChiSoDienNuoc> Items => Data.ChiSoDienNuocs;
    protected override int GetId(ChiSoDienNuoc entity) => entity.MaChiSo;
    protected override void SetId(ChiSoDienNuoc entity, int id) => entity.MaChiSo = id;

    protected override bool Matches(ChiSoDienNuoc entity, string keyword)
    {
        return (entity.SoPhong?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || (entity.DichVuTen?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || $"{entity.Thang}/{entity.Nam}".Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
