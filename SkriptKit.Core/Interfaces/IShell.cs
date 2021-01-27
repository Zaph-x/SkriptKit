using SkriptKit.Core.Objects;

namespace SkriptKit.Core.Interfaces
{
    public interface IShell
    {
        Script Script { get; }
        string StandardOutput { get; }
        string StandardError { get; }
        bool IsElevated {get;}
        int RunScript(string script);
        int Run();
        int Stop();
    }
}