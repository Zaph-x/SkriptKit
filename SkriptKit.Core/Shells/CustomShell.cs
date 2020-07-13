using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using System.IO;
using SkriptKit.Core.Objects.Helpers;
using SkriptKit.Core.Objects;
using System.Linq;

namespace SkriptKit.Core.Shells
{
    public class CustomShell : IShell
    {
        private string _interpreter { get; set; }
        public string StandardOutput { get; private set; }
        public Script Script { get; private set; }
        public string StandardError { get; private set; }
        public virtual bool IsElevated { get; private set; }
        public List<string> Arguments { get; private set; }

        public CustomShell(string interpreter)
        {
            _interpreter = interpreter;
            IsElevated = RootHelper.IsAdministrator;
        }

        public CustomShell(string interpreter, string[] arguments)
        {
            IsElevated = RootHelper.IsAdministrator;
            _interpreter = interpreter;
            Arguments = arguments.ToList();
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
            foreach (string arg in Arguments) { proc.StartInfo.ArgumentList.Add(arg); }
            proc.StartInfo.ArgumentList.Add(script);
            proc.Start();
            proc.WaitForExit();
            StandardOutput = proc.StandardOutput.ReadToEnd();
            StandardError = proc.StandardError.ReadToEnd();
            return proc.ExitCode;
        }
    }
}