using System.IO;
using System.Runtime.InteropServices;
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
    public class Script_Tests
    {
        Script script;
        [SetUp]
        public void Setup()
        {
            script = new Script();
        }

        [Test]
        [Platform(Exclude = "Linux,Unix,MacOsX")]
        public void Test_Run_CanRunPowerShellScript()
        {
            script.Shell = new PowerShell(3);
            script.ScriptBlock = "ping google.com";
            Assert.AreEqual(0, script.Run(), "Script did not exit with an exitcode of 0");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }


        [Test]
        [Platform(Exclude = "Linux,Unix,MacOsX")]
        public void Test_Run_FailingScriptShouldNotReturnZero()
        {
            script.Shell = new PowerShell(3);
            script.ScriptBlock = "png google.com";
            Assert.AreNotEqual(0, script.Run(), "Script did not exit with a non-zero exit code");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }

        [Test]
        [Platform(Exclude = "Linux,Unix,MacOsX")]
        public void Test_Run_RequiresAdminShouldFailIfNotAdmin()
        {
            var mock = new Mock<PowerShell>(3);
            mock.SetupGet(shell => shell.IsElevated).Returns(false);
            script.Shell = mock.Object;
            script.ScriptBlock = @"echo ""test""";
            script.RequireAdministrator = true;
            Assert.Throws<InsufficientPermissionsException>(() => script.Run(), "Script did not throw an exception");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }

        [Test]
        [Platform(Exclude = "Linux,Unix,MacOsX")]
        public void Test_Run_RequiresAdminShouldNotFailIfAdmin()
        {
            var mock = new Mock<PowerShell>(3);
            mock.SetupGet(shell => shell.IsElevated).Returns(true);
            script.Shell = mock.Object;
            script.ScriptBlock = @"echo ""test""";
            script.RequireAdministrator = true;
            Assert.AreEqual(0, script.Run(), "User was not considered admin");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }

        [Test]
        [Platform(Exclude = "Linux,Unix,MacOsX")]
        public void Test_Run_ShouldHaveIOAccessPowerShell()
        {
            var mock = new Mock<PowerShell>(3);
            mock.SetupGet(shell => shell.IsElevated).Returns(true);
            script.Shell = mock.Object;
            script.ScriptBlock = @"echo ""test"" > .\test.txt";
            script.RequireAdministrator = true;
            Assert.AreEqual(0, script.Run(), "User was not considered admin");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }
        [Test]
        [Platform(Exclude = "Win")]
        public void Test_Run_ShouldHaveIOAccessBash()
        {
            var mock = new Mock<Bash>(false);
            mock.SetupGet(shell => shell.IsElevated).Returns(true);
            script.Shell = mock.Object;
            script.ScriptBlock = @"echo ""test"" > .\test.txt";
            script.RequireAdministrator = true;
            Assert.AreEqual(0, script.Run(), "User was not considered admin");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }

        [Test]
        [Platform(Exclude = "Linux,MacOsX,Unix")]
        public void Test_Script_ShouldChooseDefaultInterpreterWhenNull()
        {
            script.ScriptBlock = @"echo ""Hello""";
            script.Shell = null;
            script.Run();
            Assert.IsInstanceOf(typeof(PowerShell), script.Shell, "Correct interpreter was not chosen");
            TestHelper.WriteOutput(script.Shell, TestContext.CurrentContext);
        }

        [Test]
        public void Test_Script_CanDeserializeInterpreter()
        {
            string json = @"{
                'Interpreter': 'pwsh',
                'RequiresAdministrator':false,
                'ScriptBlock':'echo ""hello""'
            }";
            Script script = Script.FromJson(json);

            Assert.AreEqual("pwsh", script.Interpreter, "Interpreter was not deserialised correct");
        }
        [Test]
        public void Test_Script_CanDeserializeScriptBlock()
        {
            string json = @"{
                'Interpreter': 'pwsh',
                'RequiresAdministrator':false,
                'ScriptBlock':'echo ""hello""'
            }";
            Script script = Script.FromJson(json);

            Assert.AreEqual(@"echo ""hello""", script.ScriptBlock, "ScriptBlock was not deserialised correct");
        }
        [Test]
        public void Test_Script_CanDeserializeRequiresAdmin()
        {
            string json = @"{
                'Interpreter': 'pwsh',
                'RequiresAdministrator':false,
                'ScriptBlock':'echo ""hello""'
            }";
            Script script = Script.FromJson(json);

            Assert.AreEqual(false, script.RequireAdministrator, "Interpreter was not deserialised correct");
        }
        [Test]
        public void Test_Script_CanDeserializeShell()
        {
            string json = @"{
                'Interpreter': 'pwsh',
                'RequiresAdministrator':false,
                'ScriptBlock':'echo ""hello""'
            }";
            Script script = Script.FromJson(json);

            Assert.IsInstanceOf<PowerShell>(script.Shell, "Shell was not correct on deserialise");
        }

        
        [Test]
        public void Test_Script_CanDeserializePlaceholders()
        {
            string json = @"{
                'Interpreter': 'pwsh',
                'RequiresAdministrator':false,
                'ScriptBlock':'echo ""hello""',
                'Placeholders':{
                    'hello':'world'
                }
            }";
            Script script = Script.FromJson(json);

            Assert.AreEqual(1, script.Placeholders.Count, "Placeholders was were deserialised correct");
        }
        
        
        [Test]
        public void Test_Script_PlaceholdersConvertCorrectly()
        {
            string json = @"{
                'Interpreter': 'pwsh',
                'RequiresAdministrator':false,
                'ScriptBlock':'echo ""hello""',
                'Placeholders':{
                    'hello':'world'
                }
            }";
            Script script = Script.FromJson(json);
            script.Run();
            Assert.AreEqual("world"+Environment.NewLine, script.Shell.StandardOutput, "Placeholders did not convert correct");
        }

        [Test]
        [Platform(Exclude="Unix")]
        public void Test_Script_CanBeReadFromFile()
        {
            string content;
            try { File.Delete(@".\test.ps1"); }
            catch{}
            using (StreamWriter sw = new StreamWriter(@".\test.ps1"))
            {
                sw.WriteLine(@"$a = 3
$b = 4
$c = $a + $b
$c");
            }
            using (StreamReader sr = new StreamReader(@".\test.ps1"))
            {
                content = sr.ReadToEnd();
            }

            Script script = new Script("pwsh", content, false);
            script.Run();
            Assert.AreEqual("7"+Environment.NewLine, script.Shell.StandardOutput, "Script did not execute correct");
        }
    }
}