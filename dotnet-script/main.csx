#!/usr/bin/env dotnet-script

#r "System.Xml.Linq"                    // reference assemblies                                      
#r "nuget: Newtonsoft.Json"             // and nuget packages is fully supported
//#load "Other.csx"                       // You can load other script files
//#load ~/ExampleScripts/CmdSyntax.nsh   // (both absolute and relative paths are fine)

using System;                           // many namespaces are loaded by default
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CSharp;

using static System.Console;            // static members smake your scripts shorter
WriteLine("Are you ready? Y/N:");

// You can run a system command just like in Bash
Print("Hello world");
