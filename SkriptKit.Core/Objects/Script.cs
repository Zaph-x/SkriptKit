using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using SkriptKit.Core.Objects.Helpers;

namespace SkriptKit.Core.Objects
{
    public class Script : IExecutable
    {
        public IShell Shell { get; set; }
        public string ScriptBlock { get; set; }
        public bool RequireAdministrator { get; set; }

        public int Run()
        {
            if (RequireAdministrator)
            {
                if (Shell.IsElevated)    
                {
                    return Shell.RunScript(ScriptBlock);
                }
                else
                {
                    throw new InsufficientPermissionsException("The script must be run using elevated priviledges");
                }
            }
            else
            {
                return Shell.RunScript(ScriptBlock);
            }
        }
    }
}