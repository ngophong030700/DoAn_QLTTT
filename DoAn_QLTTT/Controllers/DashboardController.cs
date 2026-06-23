using DoAn_QLTTT.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_QLTTT.Controllers;

public class DashboardController : AdminControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Báo cáo";
        return View(await _dashboardService.GetDashboardAsync());
    }

    [HttpGet]
    public async Task<IActionResult> GetExtraChartsData()
    {
        var data = await _dashboardService.GetDashboardChartsDataAsync();
        return Json(data);
    }
}
