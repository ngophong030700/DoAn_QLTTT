namespace DoAn_QLTTT.Services;

public interface ISqlScriptReader
{
    Task<string> ReadAsync(string scriptFileName);
}
