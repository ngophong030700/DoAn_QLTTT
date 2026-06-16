namespace DoAn_QLTTT.Services;

public class SqlScriptReader : ISqlScriptReader
{
    private readonly IWebHostEnvironment _environment;

    public SqlScriptReader(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> ReadAsync(string scriptFileName)
    {
        var safeFileName = Path.GetFileName(scriptFileName);
        var path = Path.Combine(_environment.ContentRootPath, "SqlScripts", "DemoSql", safeFileName);

        if (!File.Exists(path))
        {
            return $"-- TODO: Tao file placeholder {safeFileName} trong SqlScripts/DemoSql.";
        }

        return await File.ReadAllTextAsync(path);
    }
}
