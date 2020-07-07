using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using System.IO;
using SkriptKit.Core.Objects.Helpers;

namespace SkriptKit.Core.Shells
{
    public class PowerShell : IShell
    {
        private PSVersion _version { get; set; }
        private string _interpreter { get; set; }
        private string _exitCodeVariable { get; set; }
        public string StandardOutput {get;private set;}
        public string StandardError {get;private set;}
        public virtual bool IsElevated {get;private set;}

        public PowerShell(int version)
        {
            IsElevated = RootHelper.IsAdministrator;
            switch (version)
            {
                case 3:
                    _version = PSVersion.V3;
                    break;
                case 6:
                    _version = PSVersion.V6;
                    break;
                case 7:
                    _version = PSVersion.V7;
                    break;

            }
        }

        public int RunScript(string script)
        {
            switch (_version)
            {
                case PSVersion.V3:
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        _interpreter = "powershell";
                    }
                    else
                    {
                        throw new InvalidOSException("PowerShell V3.x scripts must be executed on a Windows machine.");
                    }
                    break;
                case PSVersion.V6:
                case PSVersion.V7:
                    _interpreter = "pwsh";
                    break;
            }
            Process proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = _interpreter,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                }
            };
            proc.StartInfo.ArgumentList.Add("-c");
            proc.StartInfo.ArgumentList.Add(script);
            proc.Start();
            proc.WaitForExit();
            StandardOutput = proc.StandardOutput.ReadToEnd();
            StandardError = proc.StandardError.ReadToEnd();
            return proc.ExitCode;
        }
    }
}