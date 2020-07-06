using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using System.IO;

namespace SkriptKit.Core.Shells
{
    public class PowerShell : IShell
    {
        private PSVersion _version { get; set; }
        private string _interpreter { get; set; }
        private string _exitCodeVariable { get; set; }

        public string STDOut {get;private set;}

        public string STDErr {get;private set;}

        public PowerShell(int version)
        {
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
                        throw new InvalidOSException("PowerShell V3 scripts must be executed on a Windows machine.");
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
            proc.Start();
            using (StreamWriter sw = proc.StandardInput)
                sw.WriteLine(script);
            proc.WaitForExit();
            STDOut = proc.StandardOutput.ReadToEnd();
            STDErr = proc.StandardError.ReadToEnd();
            return proc.ExitCode;
        }
    }
}