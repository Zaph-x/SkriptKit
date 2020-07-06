using System;
using NUnit.Framework;
using SkriptKit.Core.Objects;
using SkriptKit.Core.Shells;

namespace SkriptKit.Test
{
    public class Tests
    {
        Script script;
        [SetUp]
        public void Setup()
        {
            script = new Script();
        }

        [Test]
        public void Test_Run_CanRunPowerShellScript()
        {
            script.Shell = new PowerShell(7);
            script.ScriptBlock = "ping google.com";
            Console.WriteLine($"stdout:\n{script.Shell.STDOut}");
            Console.WriteLine($"stderr:\n{script.Shell.STDErr}");
            Assert.AreEqual(0, script.Run(), "Script did not exit with an exitcode of 0");
        }

        
        [Test]
        public void Test_Run_FailingScriptShouldNotReturnZero()
        {
            script.Shell = new PowerShell(7);
            script.ScriptBlock = "png google.com";
            Console.WriteLine($"stdout:\n{script.Shell.STDOut}");
            Console.WriteLine($"stderr:\n{script.Shell.STDErr}");
            Assert.AreNotEqual(0, script.Run(), "Script did not exit with a non-zero exit code");
        }
    }
}