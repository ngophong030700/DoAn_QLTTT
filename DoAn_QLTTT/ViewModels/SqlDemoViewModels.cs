namespace DoAn_QLTTT.ViewModels;

public class SqlDemoPageViewModel
{
    public string DataProvider { get; set; } = "Mock";
    public bool IsMock => DataProvider.Equals("Mock", StringComparison.OrdinalIgnoreCase);
    public IReadOnlyList<SqlDemoScenarioSummaryViewModel> Scenarios { get; set; } = [];
    public SqlDemoScenarioViewModel? SelectedScenario { get; set; }
}

public class SqlDemoScenarioSummaryViewModel
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Type { get; set; } = "";
}

public class SqlDemoScenarioViewModel
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Type { get; set; } = "";
    public string Problem { get; set; } = "";
    public string SqlScript { get; set; } = "";
    public string ObjectScript { get; set; } = "";
    public string Note { get; set; } = "";
    public bool HasExecuted { get; set; }
    public string? OutputTitle { get; set; }
    public string? OutputMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public IReadOnlyList<SqlDemoTableViewModel> BeforeTables { get; set; } = [];
    public IReadOnlyList<SqlDemoTableViewModel> AfterTables { get; set; } = [];
}

public class SqlDemoTableViewModel
{
    public string Title { get; set; } = "";
    public IReadOnlyList<string> Columns { get; set; } = [];
    public IReadOnlyList<IReadOnlyDictionary<string, object?>> Rows { get; set; } = [];
}
