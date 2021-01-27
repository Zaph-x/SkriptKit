using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using System.IO;
using SkriptKit.Core.Objects.Helpers;
using SkriptKit.Core.Objects;

namespace SkriptKit.Core.Shells
{
    public class Bash : IShell
    {
        private string _interpreter { get; set; }
        public string StandardOutput { get; private set; }
        public Script Script { get; }
        public string StandardError { get; private set; }
        public virtual bool IsElevated { get; private set; }
        public bool WSL { get; set; }
        private Process _process {get;set; }

        private Action<object, DataReceivedEventArgs> _outputHandle = (s, e) => Debug.WriteLine(e.Data);
        private Action<object, DataReceivedEventArgs> _errHandle = (s, e) => Debug.WriteLine(e.Data);

        public void SetOutputHandle(Action<object, DataReceivedEventArgs> handler)
        {
            _outputHandle = handler;
        }

        public void SetErrorHandle(Action<object, DataReceivedEventArgs> handler)
        {
            _errHandle = handler;
        }
        public Bash(bool wsl)
        {
            if (!wsl && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOSException("Bash can only be run on Windows with WSL set to true");
            }
            WSL = wsl;
            _interpreter = wsl ? "wsl" : "/bin/bash";
        }
        public int RunScript(string script)
        {
            _process = new Process()
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
            _process.StartInfo.ArgumentList.Add("-c");
            _process .StartInfo.ArgumentList.Add(script);
            _process .Start();
            _process .WaitForExit();
            StandardOutput = _process.StandardOutput.ReadToEnd();
            StandardError = _process.StandardError.ReadToEnd();
            return _process.ExitCode;
        }

        public int Run()
        {
            return Script.Run();
        }

        public int Stop()
        {
            _process.Kill();
            return _process.ExitCode;
        }
    }
}