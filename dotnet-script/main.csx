#!/usr/bin/env dotnet-script
#r "nuget: ProcessX, 1.5.5"
#r "nuget: Newtonsoft.Json, 13.0.3"

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Configuration;

using Cysharp.Diagnostics; // using namespace
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static async Task<string> getWeatherInfo(string uri)
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
    //await ProcessX.StartAsync("ls").WriteLineAllAsync();
    //var version = await ProcessX.StartAsync("git branch").FirstAsync();
    //string[] result = await ProcessX.StartAsync("dotnet --info").ToTask();
    //Console.WriteLine(version);
    const string BASE_URL = "https://api.openweathermap.org/data/2.5/weather?q=";
    const string CITY_NAME = "London";
    const string API_KEY = "dd901d59fd590a54f070075a96812a94";
    const string URL_REQUEST = BASE_URL + CITY_NAME + "&appid=" + API_KEY + "&units=metric";
    var response = await getWeatherInfo(URL_REQUEST);
    dynamic data = JsonConvert.DeserializeObject(response);  
    string city = data.name;  
    float windSpeed = data.wind.speed;
    float temperature = data.main.temp;
    string weatherDescription = data.weather[0].description;
    int humidity = data.main.humidity;
    /*
    var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
    foreach (KeyValuePair<string, object> value in dict)
    {
        Console.WriteLine(value.Key + "->" + value.Value);
    }
    */
    Console.WriteLine("The weather forecast for: " + city);
    Console.WriteLine("Current weather: " + weatherDescription); 
    Console.WriteLine("Current temperature: " + temperature + " C");
    Console.WriteLine("Current wind speed: " + windSpeed + " m/s");
    Console.WriteLine("Current humidity: " + humidity + "%");

}

public static async Task systemInfo() {
// TEST CODE NETWORK INTERFACES INFO AND STATS
    try
    {
        IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        Console.WriteLine("Interface information for {0}.{1}     ", computerProperties.HostName, computerProperties.DomainName);
    if (nics == null || nics.Length < 1)
    {
         Console.WriteLine($"  No network interfaces found.");
    }

         Console.WriteLine($"  Number of interfaces .................... : {nics.Length}");

    foreach (NetworkInterface adapter in nics)
    {
        IPInterfaceProperties properties = adapter.GetIPProperties();
        Console.WriteLine();
        Console.WriteLine(adapter.Description);
        Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
        Console.WriteLine($"  Adapter ID ................................ : {adapter.Id}");
        Console.WriteLine($"  Adapter Desc............................... : {adapter.Description}");
        Console.WriteLine($"  Adapter name............................... : {adapter.Name}");
        Console.WriteLine($"  Interface type ............................ : {adapter.NetworkInterfaceType}");
        Console.WriteLine($"  Physical Address .......................... : {adapter.GetPhysicalAddress()}");
        Console.WriteLine($"  Operational status ........................ : {adapter.OperationalStatus}");
        string versions = "";

    // Create a display string for the supported IP versions.
    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
    {
       versions = "IPv4";
    }

    if (adapter.Supports(NetworkInterfaceComponent.IPv6))
    {
    if (versions.Length > 0)
    {
        versions += " ";
    }

        versions += "IPv6";
    }

    Console.WriteLine($"  IP version ................................ : {versions}");
     // The following information is not useful for loopback adapters.
     if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
     {
        continue;
     }

     Console.WriteLine($"  DNS suffix ................................ : {properties.DnsSuffix}");
     string label;
     
     if (adapter.Supports(NetworkInterfaceComponent.IPv4))
     {
        IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
        Console.WriteLine($"  MTU........................................ : {ipv4.Mtu}");
     if (ipv4.UsesWins)
     {
        IPAddressCollection winsServers = properties.WinsServersAddresses;
     if (winsServers.Count > 0)
     {
        label = $"  WINS Servers .............................. :";
     }
     }
     }
     Console.WriteLine($"  DNS enabled ............................... : {properties.IsDnsEnabled}");
     Console.WriteLine($"  Speed ..................................... : {adapter.Speed}");
     Console.WriteLine($"  Receive Only .............................. : {adapter.IsReceiveOnly}");
     Console.WriteLine($"  Multicast ................................. : {adapter.SupportsMulticast}");

     var stats = adapter.GetIPv4Statistics();
     Console.WriteLine($"  Packet Received ........................... : {stats.UnicastPacketsReceived}");
     Console.WriteLine($"  Packet Sent ............................... : {stats.UnicastPacketsSent}");
     Console.WriteLine($"  Bytes Received ............................ : {stats.BytesReceived}");
     Console.WriteLine($"  Bytes Sent ................................ : {stats.BytesSent}");
     }
     }
     catch (Exception ex)
     {
        Console.WriteLine($"Error on Getting Info of Network Adapters: {ex.Message}");
     }
}

public static async Task GetMemoryMetricsAsync()
{
    var output = "";
    try
    {
        var info = new ProcessStartInfo("free -m");
        info.FileName = "/bin/sh";
        info.Arguments = "-c \"free -m\"";
        info.RedirectStandardOutput = true;

        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
        }
        Console.WriteLine($"\n{output}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}\n{e}");
        }
}

public static async Task GetCPUMetricsAsync()
{
    try
    {
        var output = "";
        var info = new ProcessStartInfo("lscpu");
        //info.FileName = "/bin/sh";
        //info.Arguments = "lscpu";
        info.RedirectStandardOutput = true;
        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
        }
            Console.WriteLine($"\n{output}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}\n{e}");
        }
}

await start();
await systemInfo();
await GetMemoryMetricsAsync();
await GetCPUMetricsAsync();

/*
// async iterate.
await foreach (string item in ProcessX.StartAsync("dotnet --info"))
{
Console.WriteLine(item);
}
*/
