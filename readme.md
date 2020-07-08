<h1 style="text-align:center">SkriptKit</h1>

<p style="text-align:center">A tool for storing and running scripts in .NET</p>

## Purpose

The purpose of the SkriptKit library is as follows: To make it easier to run scripts from a given interpreter or shell programmatically.

The library will be used in future .NET projects where an interface between the application and a given shell or script interpreter is needed.

## Storing Scripts

Using SkriptKit, script objects can be stored in JSON format, for dynamic loading of scripts. Furthermore, scripts can contain placeholder values, which can be used in dynamic execution in applications where the user might want to enter values for the placeholder.

## Example Code

#### Reading A Script From A File
```cs
string content;
using (StreamReader sr = new StreamReader(@".\setADUserEmail.ps1"))
{
    content = sr.ReadToEnd();
    /*

    $a = 3
    $b = 4
    $c = $a + $b
    $c | out-host
    
    */
}
Script script = new Script("pwsh", content, false);
script.Run();
Console.WriteLine(script.Shell.StandardOutput); // 7
```