using DoAn_QLTTT.ViewModels;

namespace DoAn_QLTTT.Services;

public interface ISqlDemoService
{
    Task<IReadOnlyList<SqlDemoScenarioSummaryViewModel>> GetScenariosAsync();
    Task<SqlDemoScenarioViewModel> GetScenarioAsync(
        string? id,
        bool execute,
        string? sqlScript = null,
        int? preferredCustomerId = null,
        int? currentUserId = null);
    Task<int> GetPreviousReadingAsync(int roomId, int serviceId, int month, int year);
}
