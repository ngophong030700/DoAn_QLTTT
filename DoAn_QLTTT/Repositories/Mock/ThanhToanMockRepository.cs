using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class ThanhToanMockRepository : MockCrudRepository<ThanhToan>, IThanhToanRepository
{
    public ThanhToanMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<ThanhToan> Items => Data.ThanhToans;
    protected override int GetId(ThanhToan entity) => entity.MaThanhToan;
    protected override void SetId(ThanhToan entity, int id) => entity.MaThanhToan = id;

    protected override bool Matches(ThanhToan entity, string keyword)
    {
        return (entity.HoaDonMoTa?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || (entity.NguoiThuHoTen?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || entity.HinhThuc.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public Task<IReadOnlyList<ThanhToan>> GetByHoaDonAsync(int maHoaDon)
    {
        Data.RebuildLookups();
        return Task.FromResult<IReadOnlyList<ThanhToan>>(
            Data.ThanhToans.Where(x => x.MaHoaDon == maHoaDon).OrderByDescending(x => x.NgayThu).ToList());
    }
}
