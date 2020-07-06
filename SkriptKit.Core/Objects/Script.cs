using SkriptKit.Core.Interfaces;

namespace SkriptKit.Core.Objects
{
    public class Script : IExecutable
    {
        public IShell Shell {get;set;}
        public string ScriptBlock {get;set;}

        public int Run()
        {
            return Shell.RunScript(ScriptBlock);
        }
    }
}