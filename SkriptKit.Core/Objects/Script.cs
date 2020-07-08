using System.Linq;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Interfaces;
using SkriptKit.Core.Objects.Helpers;
using SkriptKit.Core.Shells;

namespace SkriptKit.Core.Objects
{
    public class Script : IExecutable
    {
        public string ScriptName {get;set;}
        [JsonIgnore]
        public IShell Shell { get; set; }
        public string Interpreter { get; set; }
        public string ScriptBlock { get; set; }
        public string[] ShellArgs { get; set; }
        public bool RequireAdministrator { get; set; }
        public Dictionary<string, string> Placeholders {get;set;} = new Dictionary<string, string>();

        public Script(IShell shell, string scriptBlock, bool requiresAdmin)
        {
            Shell = shell;
            ScriptBlock = scriptBlock;
            RequireAdministrator = requiresAdmin;
        }

        public Script() {}

        public int Run()
        {
            foreach (KeyValuePair<string,string> kvp in Placeholders)
            {
                ScriptBlock = ScriptBlock.Replace(kvp.Key, kvp.Value);
            }
            if (Shell == null && string.IsNullOrEmpty(Interpreter))
            {
                Shell = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? (IShell)new PowerShell(3) : (IShell)new Bash(false);
            }
            else if (Shell == null)
            {
                SetShell();
            }
            if (RequireAdministrator)
            {
                if (Shell.IsElevated)
                {
                    return Shell.RunScript(ScriptBlock);
                }
                else
                {
                    throw new InsufficientPermissionsException("The script must be run using elevated priviledges");
                }
            }
            else
            {
                return Shell.RunScript(ScriptBlock);
            }
        }

        public static Script FromJson(string json)
        {
            Script script = JsonConvert.DeserializeObject<Script>(json);
            script.SetShell();
            return script;
        }

        public void SetShell()
        {
            switch (Interpreter.ToLower())
            {
                case "powershell":
                case "powershell3":
                case "pwsh3":
                    this.Shell = new PowerShell(3);
                    break;
                case "pwsh":
                case "pwsh7":
                case "powershell7":
                    this.Shell = new PowerShell(7);
                    break;
                case "pwsh6":
                case "powershell6":
                    this.Shell = new PowerShell(6);
                    break;
                case "bin/bash":
                case "bash":
                case "usr/bin/bash":
                case "/bin/bash":
                case "/usr/bin/bash":
                case "#!/bin/bash":
                case "#!/usr/bin/bash":
                    this.Shell = new Bash(false);
                    break;
                case "wsl":
                    this.Shell = new Bash(true);
                    break;
                case "cmd":
                    this.Shell = new CommandPrompt();
                    break;
                default:
                    if (!string.IsNullOrEmpty(Interpreter))
                    {
                        this.Shell = new CustomShell(Interpreter, ShellArgs);
                    }
                    break;
            }
        }
    }
}