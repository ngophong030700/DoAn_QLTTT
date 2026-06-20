namespace DoAn_QLTTT.ViewModels;

public class SqlDemoPageViewModel
{
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
    public int CurrentUserId { get; set; } = 1;
    public int? PreferredCustomerId { get; set; }
    public int? CreatedCustomerId { get; set; }
    public decimal? DebtAmount { get; set; }
    public IReadOnlyList<SqlDemoOptionViewModel> RoomOptions { get; set; } = [];
    public IReadOnlyList<SqlDemoOptionViewModel> CustomerOptions { get; set; } = [];
    public IReadOnlyList<SqlDemoOptionViewModel> ServiceOptions { get; set; } = [];
    public IReadOnlyList<SqlDemoOptionViewModel> ContractOptions { get; set; } = [];
    public IReadOnlyList<SqlDemoOptionViewModel> InvoiceOptions { get; set; } = [];
    public IReadOnlyList<SqlDemoTableViewModel> BeforeTables { get; set; } = [];
    public IReadOnlyList<SqlDemoTableViewModel> AfterTables { get; set; } = [];
}

public class SqlDemoOptionViewModel
{
    public int Value { get; set; }
    public string Label { get; set; } = "";
    public decimal? Amount { get; set; }
}

public class SqlDemoTableViewModel
{
    public string Title { get; set; } = "";
    public IReadOnlyList<string> Columns { get; set; } = [];
    public IReadOnlyList<IReadOnlyDictionary<string, object?>> Rows { get; set; } = [];
}
