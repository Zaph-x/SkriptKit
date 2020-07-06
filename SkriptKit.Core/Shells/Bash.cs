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
        public string STDOut {get;private set;}
        public string STDErr {get;private set;}
        public virtual bool IsElevated {get;private set;}

        public Bash()
        {
            _interpreter = "/bin/bash";
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
            proc.Start();
            using (StreamWriter sw = proc.StandardInput)
                sw.WriteLine(script);
            proc.WaitForExit();
            STDOut = proc.StandardOutput.ReadToEnd();
            STDErr = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            return proc.ExitCode;
        }
    }
}