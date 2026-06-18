using DoAn_QLTTT.Services;
using DoAn_QLTTT.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        return View("Index", await BuildPageAsync(id, execute: true, sqlScript));
    }

    private async Task<SqlDemoPageViewModel> BuildPageAsync(string? id, bool execute, string? sqlScript = null)
    {
        return new SqlDemoPageViewModel
        {
            DataProvider = _sqlDemoService.DataProvider,
            Scenarios = await _sqlDemoService.GetScenariosAsync(),
            SelectedScenario = await _sqlDemoService.GetScenarioAsync(id, execute, sqlScript)
        };
    }
}
