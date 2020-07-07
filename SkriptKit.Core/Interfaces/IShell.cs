namespace SkriptKit.Core.Interfaces
{
    public interface IShell
    {
        string StandardOutput { get; }
        string StandardError { get; }
        bool IsElevated {get;}
        int RunScript(string script);
    }
}