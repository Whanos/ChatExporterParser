using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Parsing...");
            string fileText;
            using (StreamReader r = new StreamReader("messages.json"))
            {
                fileText = r.ReadToEnd();
            }
            JObject data = JObject.Parse(fileText);
            Console.WriteLine($"Guild Name: {data["guild"]["name"]}\nGuild ID: {data["guild"]["id"]}\nChannel Name: {data["channel"]["name"]}\nChannel ID: {data["channel"]["id"]}\nMessage Count: {data["messageCount"]}");
            Dictionary<string, string> idiots = new Dictionary<string, string>();
            foreach (var idiot in data["messages"])
            {
                if (!idiots.ContainsValue($"{idiot["author"]["id"]}"))
                {
                    //value is username#discriminator, value is id
                    idiots.Add($"{idiot["author"]["name"]}#{idiot["author"]["discriminator"]}", $"{idiot["author"]["id"]}");
                }
            }
            Console.WriteLine("Done parsing... Now writing!");
            using (StreamWriter w = new StreamWriter("ids.txt", false))
            {
                foreach (var idiot in idiots)
                {
                    w.WriteLine(idiot.Value);
                }
            }
            using (StreamWriter w = new StreamWriter("both.txt", false))
            {
                foreach (var idiot in idiots)
                {
                    w.WriteLine($"{idiot.Key}|{idiot.Value}");
                }
            }
            using (StreamWriter w = new StreamWriter("mentionable.txt", false))
            {
                foreach (var idiot in idiots)
                {
                    w.WriteLine($"<@{idiot.Value}>");
                }
            }
            Console.WriteLine("Done! :)");
        }
    }
}