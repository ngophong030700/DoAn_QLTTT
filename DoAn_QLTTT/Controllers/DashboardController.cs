using DoAn_QLTTT.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_QLTTT.Controllers;

public class DashboardController : AdminControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly IConfiguration _configuration;

    public DashboardController(IDashboardService dashboardService, IConfiguration configuration)
    {
        _dashboardService = dashboardService;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var dataProvider = _configuration["DataProvider"] ?? "Mock";
        ViewBag.DataProvider = dataProvider;
        ViewBag.IsMockData = !dataProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
            && !dataProvider.Equals("Dapper", StringComparison.OrdinalIgnoreCase);
        ViewData["Title"] = "Báo cáo";
        return View(await _dashboardService.GetDashboardAsync());
    }
}
