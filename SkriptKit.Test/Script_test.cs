using System;
using Moq;
using NUnit.Framework;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Objects;
using SkriptKit.Core.Objects.Helpers;
using SkriptKit.Core.Shells;

namespace SkriptKit.Test
{
    [TestFixture]
    [Platform(Exclude="Linux,Unix,MacOsX")]
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
            script.Shell = new PowerShell(3);
            script.ScriptBlock = "ping google.com";
            Console.WriteLine($"stdout:\n{script.Shell.STDOut}");
            Console.WriteLine($"stderr:\n{script.Shell.STDErr}");
            Assert.AreEqual(0, script.Run(), "Script did not exit with an exitcode of 0");
        }


        [Test]
        public void Test_Run_FailingScriptShouldNotReturnZero()
        {
            script.Shell = new PowerShell(3);
            script.ScriptBlock = "png google.com";
            Assert.AreNotEqual(0, script.Run(), "Script did not exit with a non-zero exit code");
            Console.WriteLine($"stdout:\n{script.Shell.STDOut}");
            Console.WriteLine($"stderr:\n{script.Shell.STDErr}");
        }

        [Test]
        public void Test_Run_RequiresAdminShouldFailIfNotAdmin()
        {
            var mock = new Mock<PowerShell>(3);
            mock.SetupGet(shell => shell.IsElevated).Returns(false);
            script.Shell = mock.Object;
            script.ScriptBlock = @"echo ""test""";
            script.RequireAdministrator = true;
            Assert.Throws<InsufficientPermissionsException>(() => script.Run(), "Script did not throw an exception");
            Console.WriteLine($"stdout:\n{script.Shell.STDOut}");
            Console.WriteLine($"stderr:\n{script.Shell.STDErr}");
        }

        [Test]
        public void Test_Run_RequiresAdminShouldNotFailIfAdmin()
        {
            var mock = new Mock<PowerShell>(3);
            mock.SetupGet(shell => shell.IsElevated).Returns(true);
            script.Shell = mock.Object;
            script.ScriptBlock = @"echo ""test""";
            script.RequireAdministrator = true;
            Assert.AreEqual(0, script.Run(), "User was not considered admin");
            Console.WriteLine($"stdout:\n{script.Shell.STDOut}");
            Console.WriteLine($"stderr:\n{script.Shell.STDErr}");
        }
    }
}