using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class PhongTroMockRepository : MockCrudRepository<PhongTro>, IPhongTroRepository
{
    public PhongTroMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<PhongTro> Items => Data.PhongTros;
    protected override int GetId(PhongTro entity) => entity.MaPhong;
    protected override void SetId(PhongTro entity, int id) => entity.MaPhong = id;

    protected override bool Matches(PhongTro entity, string keyword)
    {
        return entity.SoPhong.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || (entity.LoaiPhongTen?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || entity.TrangThai.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
