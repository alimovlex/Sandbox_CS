#!/usr/bin/env dotnet-script
#r "nuget: ProcessX, 1.5.5"
using Cysharp.Diagnostics; // using namespace

public static async Task start()
{
    await ProcessX.StartAsync("dotnet --info").WriteLineAllAsync();
    var version = await ProcessX.StartAsync("dotnet --version").FirstAsync();
    //string[] result = await ProcessX.StartAsync("dotnet --info").ToTask();
    Console.WriteLine(version);
    /*
    // async iterate.
    await foreach (string item in ProcessX.StartAsync("dotnet --info"))
    {
    Console.WriteLine(item);
    }
    */
}

await start();

