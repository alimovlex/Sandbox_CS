#!/usr/bin/env dotnet-script
#r "nuget: ProcessX, 1.5.5"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Cysharp.Diagnostics; // using namespace

public static async Task<string> GetAsync(string uri)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

    using(HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
    using(Stream stream = response.GetResponseStream())
    using(StreamReader reader = new StreamReader(stream))
    {
        return await reader.ReadToEndAsync();
    }
}

public static async Task start()
{
    await ProcessX.StartAsync("dotnet --info").WriteLineAllAsync();
    var version = await ProcessX.StartAsync("dotnet --version").FirstAsync();
    //string[] result = await ProcessX.StartAsync("dotnet --info").ToTask();
    Console.WriteLine(version);
    var response = await GetAsync("https://api.openweathermap.org/data/2.5/weather?q=London&appid=dd901d59fd590a54f070075a96812a94&units=metric");
    Console.WriteLine(response);
}

await start();


/*
// async iterate.
await foreach (string item in ProcessX.StartAsync("dotnet --info"))
{
Console.WriteLine(item);
}
*/
