namespace SkriptKit.Core.Interfaces
{
    public interface IShell
    {
        string STDOut { get; }
        string STDErr { get; }
        bool IsElevated {get;}
        int RunScript(string script);
    }
}