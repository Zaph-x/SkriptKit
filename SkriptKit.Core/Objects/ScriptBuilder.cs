using SkriptKit.Core.Interfaces;

namespace SkriptKit.Core.Objects
{
    public class ScriptBuilder : Builder<Script>
    {
        private Script _script = new Script();
        public Builder<Script> SetShell(IShell item)
        {
            _script.Shell = item;
            return this;
        }

        public Builder<Script> ReplacePlaceholder(string placeholder, string newValue)
        {
            _script.ScriptBlock.Replace(placeholder, newValue);
            return this;
        }

        public Builder<Script> SetRequiresAdmin(bool requiresAdmin)
        {
            _script.RequireAdministrator = requiresAdmin;
            return this;
        }
        public Builder<Script> SetRequiresAdmin()
        {
            _script.RequireAdministrator = true;
            return this;
        }

        public Builder<Script> AddLines(string lines)
        {
            _script.ScriptBlock += lines;
            return this;
        }

        public override Script Build()
        {
            return _script;
        }
    }
}