using DoAn_QLTTT.Repositories;

namespace DoAn_QLTTT.Services;

public class NhacNoService : INhacNoService
{
    private readonly IHoaDonRepository _hoaDonRepository;

    public NhacNoService(IHoaDonRepository hoaDonRepository)
    {
        _hoaDonRepository = hoaDonRepository;
    }

    public Task<int> QuetHoaDonQuaHanAsync()
    {
        // TODO: DB thật có thể triển khai sp_HoaDon_QuetQuaHan bằng cursor/job để đổi trạng thái hóa đơn quá hạn.
        return _hoaDonRepository.ScanOverdueAsync();
    }
}
