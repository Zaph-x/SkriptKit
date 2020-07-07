using System.Runtime.InteropServices;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using SkriptKit.Core.Objects.Helpers;
using SkriptKit.Core.Shells;

namespace SkriptKit.Core.Objects
{
    public class Script : IExecutable
    {
        public IShell Shell { get; set; }
        public string ScriptBlock { get; set; }
        public bool RequireAdministrator { get; set; }

        public int Run()
        {
            if (Shell == null)
            {
                Shell = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? (IShell)new PowerShell(3) : (IShell)new Bash(false);
            }
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