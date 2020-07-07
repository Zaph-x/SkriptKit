using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using System.IO;
using SkriptKit.Core.Objects.Helpers;

namespace SkriptKit.Core.Shells
{
    public class CustomShell : IShell
    {
        private string _interpreter { get; set; }
        private string _exitCodeVariable { get; set; }
        public string StandardOutput { get; private set; }
        public string StandardError { get; private set; }
        public virtual bool IsElevated { get; private set; }
        public string[] Arguments { get; private set; }

        public CustomShell(string interpreter, params string[] arguments)
        {
            _interpreter = interpreter;
            Arguments = arguments;
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
            // foreach (string arg in Arguments) { proc.StartInfo.ArgumentList.Add(arg); }
            proc.Start();
            using (StreamWriter sw = proc.StandardInput)
                sw.WriteLine(script);
            proc.WaitForExit();
            StandardOutput = proc.StandardOutput.ReadToEnd();
            StandardError = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            return proc.ExitCode;
        }
    }
}