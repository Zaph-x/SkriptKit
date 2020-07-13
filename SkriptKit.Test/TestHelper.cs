using System;
using System.IO;
using NUnit.Framework;
using SkriptKit.Core.Interfaces;

namespace SkriptKit.Test
{
    public class TestHelper
    {
        public static void WriteOutput(IShell shell, TestContext ctx)
        {
            if (!Directory.Exists(@".\output"))
            {
                Directory.CreateDirectory(@".\output");
            }
            try {
                File.Delete(".\\output\\"+ctx.Test.Name+".txt");
            } catch {}
            using (StreamWriter sw = new StreamWriter(".\\output\\"+ctx.Test.Name+".txt"))
            {
                sw.WriteLine("STDOUT:");
                sw.WriteLine(shell.StandardOutput);
                sw.WriteLine("STDERR:");
                sw.WriteLine(shell.StandardError);
            }
        }

        public static void IsInEnvironment(string variable, string errorMessage)
        {
            if (!Environment.GetEnvironmentVariable("path")?.ToLower()?.Contains(variable) ?? false)
            {
                Assert.Inconclusive(errorMessage);
            }
        }
    }
}