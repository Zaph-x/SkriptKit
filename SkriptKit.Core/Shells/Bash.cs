using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using System.IO;
using SkriptKit.Core.Objects.Helpers;

namespace SkriptKit.Core.Shells
{
    public class Bash : IShell
    {
        private string _interpreter { get; set; }
        private string _exitCodeVariable { get; set; }
        public string StandardOutput { get; private set; }
        public string StandardError { get; private set; }
        public virtual bool IsElevated { get; private set; }
        public bool WSL { get; set; }

        public Bash(bool wsl)
        {
            WSL = wsl;
            _interpreter = wsl ? "wsl" : "/bin/bash";
        }
        public int RunScript(string script)
        {
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