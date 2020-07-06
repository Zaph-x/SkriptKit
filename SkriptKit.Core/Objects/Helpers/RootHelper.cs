using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SkriptKit.Core.Objects.Helpers
{
    public class RootHelper
    {

        [DllImport("libc")]
        public static extern uint geteuid();

        public static bool IsAdministrator =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator) :
            geteuid() == 0;
    }
}