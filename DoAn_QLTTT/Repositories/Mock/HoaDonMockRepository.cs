using DoAn_QLTTT.Models;

namespace DoAn_QLTTT.Repositories.Mock;

public class HoaDonMockRepository : MockCrudRepository<HoaDon>, IHoaDonRepository
{
    public HoaDonMockRepository(MockDataService data) : base(data)
    {
    }

    protected override List<HoaDon> Items => Data.HoaDons;
    protected override int GetId(HoaDon entity) => entity.MaHoaDon;
    protected override void SetId(HoaDon entity, int id) => entity.MaHoaDon = id;

    protected override bool Matches(HoaDon entity, string keyword)
    {
        return (entity.HopDongMoTa?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || (entity.SoPhong?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || (entity.KhachDaiDienHoTen?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false)
            || entity.TrangThai.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || $"{entity.Thang}/{entity.Nam}".Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public Task<IReadOnlyList<HoaDon>> GetRecentAsync(int take)
    {
        Data.RebuildLookups();
        return Task.FromResult<IReadOnlyList<HoaDon>>(
            Data.HoaDons.OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang).ThenByDescending(x => x.MaHoaDon).Take(take).ToList());
    }

    public Task<IReadOnlyList<HoaDon>> GetOverdueAsync()
    {
        Data.RebuildLookups();
        return Task.FromResult<IReadOnlyList<HoaDon>>(
            Data.HoaDons.Where(x => x.ConLai > 0 && x.HanThanhToan < DateOnly.FromDateTime(DateTime.Today)).ToList());
    }

    public Task<IReadOnlyList<ChiTietHoaDon>> GetChiTietAsync(int maHoaDon)
    {
        Data.RebuildLookups();
        return Task.FromResult<IReadOnlyList<ChiTietHoaDon>>(
            Data.ChiTietHoaDons.Where(x => x.MaHoaDon == maHoaDon).ToList());
    }

    public Task<int> AddChiTietAsync(ChiTietHoaDon chiTiet)
    {
        if (chiTiet.MaChiTiet == 0)
        {
            chiTiet.MaChiTiet = Data.ChiTietHoaDons.Count == 0 ? 1 : Data.ChiTietHoaDons.Max(x => x.MaChiTiet) + 1;
        }

        Data.ChiTietHoaDons.Add(chiTiet);
        Data.RebuildLookups();
        return Task.FromResult(chiTiet.MaChiTiet);
    }

    public Task<int> ScanOverdueAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var count = 0;
        foreach (var hoaDon in Data.HoaDons.Where(x => x.ConLai > 0 && x.HanThanhToan < today))
        {
            hoaDon.TrangThai = AppStatuses.HoaDon.QuaHan;
            count++;
        }

        Data.RebuildLookups();
        return Task.FromResult(count);
    }
}
