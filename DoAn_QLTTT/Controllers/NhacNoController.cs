using DoAn_QLTTT.Repositories;
using DoAn_QLTTT.Services;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_QLTTT.Controllers;

public class NhacNoController : AdminControllerBase
{
    private readonly IHoaDonRepository _hoaDonRepository;
    private readonly INhacNoService _nhacNoService;

    public NhacNoController(IHoaDonRepository hoaDonRepository, INhacNoService nhacNoService)
    {
        _hoaDonRepository = hoaDonRepository;
        _nhacNoService = nhacNoService;
    }

    public async Task<IActionResult> Index()
    {
        return View(new NhacNoViewModel
        {
            HoaDonQuaHans = await _hoaDonRepository.GetOverdueAsync(),
            SoHoaDonDaQuet = TempData["ScannedCount"] as int?
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuetQuaHan()
    {
        var count = await _nhacNoService.QuetHoaDonQuaHanAsync();
        TempData["ScannedCount"] = count;
        return RedirectToAction(nameof(Index));
    }
}
