#!/usr/bin/env dotnet-script
#r "nuget: ProcessX, 1.5.5"
#r "nuget: Newtonsoft.Json, 13.0.3"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Cysharp.Diagnostics; // using namespace
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static async Task<string> weatherInfo(string uri)
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
    await ProcessX.StartAsync("ls").WriteLineAllAsync();
    var version = await ProcessX.StartAsync("git branch").FirstAsync();
    //string[] result = await ProcessX.StartAsync("dotnet --info").ToTask();
    Console.WriteLine(version);
    const string BASE_URL = "https://api.openweathermap.org/data/2.5/weather?q=";
    const string CITY_NAME = "London";
    const string API_KEY = "dd901d59fd590a54f070075a96812a94";
    const string URL_REQUEST = BASE_URL + CITY_NAME + "&appid=" + API_KEY + "&units=metric";
    var response = await weatherInfo(URL_REQUEST);
    dynamic data = JsonConvert.DeserializeObject(response);  
    string city = data.name;  
    double temperature = data.main.temp;
    double windSpeed = data.wind.speed;
    string weatherDescription = data.weather[0].description;
    /*
    var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
    foreach (KeyValuePair<string, object> value in dict)
    {
        Console.WriteLine(value.Key + "->" + value.Value);
    }
    */
    Console.WriteLine("The weather forecast for: " + city);
    Console.WriteLine("Current weather: " + weatherDescription); 
    Console.WriteLine("Current temperature: ", temperature);
    Console.WriteLine("Current wind speed: ", windSpeed);
}

await start();


/*
// async iterate.
await foreach (string item in ProcessX.StartAsync("dotnet --info"))
{
Console.WriteLine(item);
}
*/
