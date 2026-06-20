using DoAn_QLTTT.Services;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoAn_QLTTT.Controllers;

public class SqlDemoController : AdminControllerBase
{
    private readonly ISqlDemoService _sqlDemoService;

    public SqlDemoController(ISqlDemoService sqlDemoService)
    {
        _sqlDemoService = sqlDemoService;
    }

    public async Task<IActionResult> Index(string? id)
    {
        return View(await BuildPageAsync(id, execute: false));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Execute(string id, string? sqlScript)
    {
        var model = await BuildPageAsync(id, execute: true, sqlScript);
        if (model.SelectedScenario?.CreatedCustomerId is int customerId)
        {
            HttpContext.Session.SetInt32("SqlDemo.LatestCustomerId", customerId);
            model.SelectedScenario.PreferredCustomerId = customerId;
        }

        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> PreviousReading(int roomId, int serviceId, int month, int year)
    {
        if (roomId <= 0 || serviceId <= 0 || month is < 1 or > 12 || year is < 2000 or > 2100)
        {
            return BadRequest();
        }

        return Json(new
        {
            value = await _sqlDemoService.GetPreviousReadingAsync(roomId, serviceId, month, year)
        });
    }

    private async Task<SqlDemoPageViewModel> BuildPageAsync(string? id, bool execute, string? sqlScript = null)
    {
        var currentUserId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var parsedUserId)
            ? parsedUserId
            : 1;

        return new SqlDemoPageViewModel
        {
            Scenarios = await _sqlDemoService.GetScenariosAsync(),
            SelectedScenario = await _sqlDemoService.GetScenarioAsync(
                id,
                execute,
                sqlScript,
                HttpContext.Session.GetInt32("SqlDemo.LatestCustomerId"),
                currentUserId)
        };
    }
}
