using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_QLTTT.Controllers;

public class NhacNoController : AdminControllerBase
{
    private readonly IHoaDonRepository _hoaDonRepository;

    public NhacNoController(IHoaDonRepository hoaDonRepository)
    {
        _hoaDonRepository = hoaDonRepository;
    }

    public async Task<IActionResult> Index()
    {
        return View(new NhacNoViewModel
        {
            HoaDonQuaHans = await _hoaDonRepository.GetOverdueAsync()
        });
    }
}
