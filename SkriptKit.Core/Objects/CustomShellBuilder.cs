using System.Linq;
using System;
using System.Collections.Generic;
using SkriptKit.Core.Shells;

namespace SkriptKit.Core.Objects
{
    public class CustomShellBuilder : Builder<CustomShell>
    {
        private string _interpreter { get; set; }
        private string _scriptBlock { get; set; }
        private bool _isElevated { get; set; }
        private bool _runNow { get; set; }
        private List<string> _arguments { get; set; } = new List<string>();

        public CustomShellBuilder SetInterpreter(string interpreter)
        {
            _interpreter = interpreter;
            return this;
        }

        public CustomShellBuilder AddArgument(string argument)
        {
            _arguments.Add(argument);
            return this;
        }
        public CustomShellBuilder SetScript(string script)
        {
            _scriptBlock = script + Environment.NewLine;
            return this;
        }

        public CustomShellBuilder SetRequiresAdmin(bool requiresAdmin)
        {
            _isElevated = requiresAdmin;
            return this;
        }
        public CustomShellBuilder SetRequiresAdmin()
        {
            _isElevated = true;
            return this;
        }
        
        public CustomShellBuilder AddScriptLine(string line)
        {
            _scriptBlock += line + Environment.NewLine;
            return this;
        }

        public CustomShellBuilder SetRunNow(bool runNow)
        {
            _runNow = runNow;
            return this;
        }
        public CustomShellBuilder SetRunNow()
        {
            _runNow = true;
            return this;
        }

        public override CustomShell Build()
        {
            CustomShell shell = new CustomShell(_interpreter, _isElevated, string.Join(" ", _arguments.ToArray()), _scriptBlock, _runNow);
            return shell;
        }
    }
}