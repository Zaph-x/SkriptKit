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
        public Script Script { get; }
        public string StandardOutput { get; private set; }
        public string StandardError { get; private set; }
        public virtual bool IsElevated { get; private set; }
        public List<string> Arguments { get; private set; }
        private Process _process { get; set; }

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
            foreach (string arg in Arguments) { _process.StartInfo.ArgumentList.Add(arg); }
            _process.StartInfo.ArgumentList.Add(script);
            _process.Start();
            _process.WaitForExit();
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