using NUnit.Framework;
using SkriptKit.Core.Exceptions;
using SkriptKit.Core.Objects;
using SkriptKit.Core.Shells;

namespace SkriptKit.Test
{
    public class ScriptBuilder_test
    {
        ScriptBuilder builder;
        [SetUp]
        public void SetUp()
        {
            builder = new ScriptBuilder();
        }

        [Test]
        public void Test_ScriptBuilder_ShouldThrowExceptionOnNoShell()
        {
            Assert.Throws<NoShellException>(() => builder.Build(), "Builder built script even though no shell was provided.");
        }

        [Test]
        public void Test_ScriptBuilder_CanAddALine()
        {
            string line = @"echo ""Test""";
            string expectedLine = @"echo ""Test""";

            builder.SetShell(new Bash(false));
            builder.AddLines(line);

            Assert.AreEqual(expectedLine, builder.Build().ScriptBlock, "The lines did not match");
        }

        [Test]
        public void Test_ScriptBuilder_CanAssignRunAsNoParam()
        {
            bool expectedRunAs = true;

            builder.SetShell(new Bash(false));
            builder.SetRequiresAdmin();


            Assert.AreEqual(expectedRunAs, builder.Build().RequireAdministrator, "Administrator setting was not set correct");
        }

        [Test]
        public void Test_ScriptBuilder_CanAssignRunAsWithParam()
        {
            bool expectedRunAs = true;

            builder.SetShell(new Bash(false));
            builder.SetRequiresAdmin(true);


            Assert.AreEqual(expectedRunAs, builder.Build().RequireAdministrator, "Administrator setting was not set correct");
        }

        [Test]
        public void Test_ScriptBuilder_CanAssignCorrectShell()
        {
            builder.SetShell(new Bash(false));

            Assert.IsInstanceOf<Bash>(builder.Build().Shell, "The shells did not match");
        }

        [Test]
        [Platform(Exclude = "MacOsX,Linux,Unix")]
        public void Test_ScriptBuilder_CanProduceRunablePowerShellScript()
        {
            builder.SetShell(new PowerShell(7));
            builder.AddLines(@"echo ""hello""");
            builder.SetRequiresAdmin(false);
            Script script = builder.Build();

            Assert.AreEqual(0, script.Run(), "Script was unable to run");
        }

        [Test]
        [Platform(Exclude = "Win")]
        public void Test_ScriptBuilder_CanProduceRunableBashScript()
        {
            builder.SetShell(new Bash(false));
            builder.AddLines(@"echo ""hello""");
            builder.SetRequiresAdmin(false);
            Script script = builder.Build();

            Assert.AreEqual(0, script.Run(), "Script was unable to run");
        }
    }
}