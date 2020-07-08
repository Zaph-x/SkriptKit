using System.IO;
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
    public class CustomShell_Tests
    {
        Script script;
        [SetUp]
        public void Setup()
        {
            script = new Script();
        }

        [Test]
        public void Test_Python_CanExecuteScript()
        {
            script.ScriptBlock = @"a = 1 + 3
print(a)";
            CustomShell custom = new CustomShell("python", "-c");
            script.Shell = custom;
            Assert.AreEqual(0, script.Run(), "Python did not run the script");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }

        [Test]
        public void Test_Python_ShouldFailOnInvalidSyntax()
        {
            script.ScriptBlock = @"a = 1 + 1
print(a";
            CustomShell custom = new CustomShell("python", "-c");
            script.Shell = custom;
            Assert.AreNotEqual(0, script.Run(), "Python ran the script without returning a non-zero exit code");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }
    }
}