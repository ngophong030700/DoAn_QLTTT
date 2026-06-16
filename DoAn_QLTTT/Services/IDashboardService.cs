using DoAn_QLTTT.ViewModels;

namespace DoAn_QLTTT.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardAsync();
}
