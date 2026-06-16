using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class DichVuMockRepository : MockCrudRepository<DichVu>, IDichVuRepository
{
    public DichVuMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<DichVu> Items => Data.DichVus;
    protected override int GetId(DichVu entity) => entity.MaDichVu;
    protected override void SetId(DichVu entity, int id) => entity.MaDichVu = id;

    protected override bool Matches(DichVu entity, string keyword)
    {
        return entity.TenDichVu.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || entity.DonVi.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || entity.LoaiTinhPhi.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
