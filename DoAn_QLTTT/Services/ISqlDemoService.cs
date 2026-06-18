using DoAn_QLTTT.ViewModels;

namespace DoAn_QLTTT.Services;

public interface ISqlDemoService
{
    string DataProvider { get; }
    Task<IReadOnlyList<SqlDemoScenarioSummaryViewModel>> GetScenariosAsync();
    Task<SqlDemoScenarioViewModel> GetScenarioAsync(string? id, bool execute, string? sqlScript = null);
}
