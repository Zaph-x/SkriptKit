namespace SkriptKit.Core.Interfaces
{
    public interface IShell
    {
        string STDOut { get; }
        string STDErr { get; }
        int RunScript(string script);
    }
}