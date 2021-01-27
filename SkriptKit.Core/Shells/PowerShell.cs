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
    public class PowerShell : IShell
    {
        private PSVersion _version { get; set; }
        private string _interpreter { get; set; }
        public string Arguments { get; private set; }
        public string StandardOutput { get; private set; }
        public int ExitCode { get; private set; }
        public string StandardError { get; private set; }
        public virtual bool IsElevated { get; private set; }
        public bool IsRunning { get => !_process.HasExited; }
        public bool RedirectStdIn { get; set; }
        public Script Script { get; set; }
        private Process _process { get; set; }

        private Action<object, DataReceivedEventArgs> _outputHandle = (s, e) => Debug.WriteLine(e.Data);
        private Action<object, DataReceivedEventArgs> _errHandle = (s, e) => Debug.WriteLine(e.Data);

        public void SetOutputHandle(Action<object,DataReceivedEventArgs> handler)
        {
            _outputHandle = handler;
        }

        public void SetErrorHandle(Action<object, DataReceivedEventArgs> handler)
        {
            _errHandle = handler;
        }

        public PowerShell(int version)
        {
            IsElevated = RootHelper.IsAdministrator;
            switch (version)
            {
                case 3:
                    _version = PSVersion.V3;
                    break;
                case 5:
                case 6:
                case 7:
                    _version = PSVersion.Core;
                    break;
                default:
                    throw new InvalidShellException("No PowerShell version with that version tag");
            }
        }

        public PowerShell(int version, bool requiresAdmin, string arguments, string scriptBlock, bool runNow)
        {
            Arguments = arguments;
            IsElevated = RootHelper.IsAdministrator;
            switch (version)
            {
                case 3:
                    _version = PSVersion.V3;
                    break;
                case 5:
                case 6:
                case 7:
                    _version = PSVersion.Core;
                    break;
                default:
                    throw new InvalidShellException("No PowerShell version with that version tag");
            }
            Script = new Script() { RequireAdministrator = requiresAdmin, ScriptBlock = scriptBlock, Shell = this };
            if (runNow)
                Script.Run();
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
                case PSVersion.Core:
                    _interpreter = "pwsh";
                    break;
            }
            _process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = _interpreter,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = RedirectStdIn,
                }
            };

            _process.OutputDataReceived += new DataReceivedEventHandler(_outputHandle);
            _process.ErrorDataReceived += new DataReceivedEventHandler(_errHandle);
            _process.OutputDataReceived += (s, e) => {
                StandardOutput += $"{e.Data}{Environment.NewLine}";
                Debug.WriteLine(e.Data);
            };
            _process.ErrorDataReceived += (s, e) => {
                StandardError += $"{e.Data}{Environment.NewLine}";
                Debug.WriteLine(e.Data);
            };
            _process.StartInfo.Arguments = $"{Arguments} -File \"{script}\"";
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _process.WaitForExit();

            return ExitCode = _process.ExitCode;
        }

        public int Run()
        {
            return ExitCode = Script.Run();
        }

        public int Stop()
        {
            _process.Kill();
            return ExitCode = _process.ExitCode;
        }
    }
}