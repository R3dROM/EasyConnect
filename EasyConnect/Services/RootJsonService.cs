using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EasyConnect.Services
{
    public class RootJsonService
    {
        private static Dictionary<string, DeviceList> _index;
        public static async Task LoadJsonFile(string json)
        {
            try
            {
                Debug.WriteLine("Start");
                var root = JsonSerializer.Deserialize<Root>(json);
                if (root != null)
                {
                    _index = root.sheetData.row
                                .Skip(1)
                                .Where(r => r.c?.Count >= 4)
                                .ToDictionary(
                                    r => r.c[0].v,
                                    r => new DeviceList
                                    {
                                        Name = r.c[1].v,
                                        Group = r.c[3].v,
                                        Number = ExtractNumber(r.c[1].v)
                                    }
                                );
                }
                Debug.WriteLine("Finish");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Path: {ex.Source}");
            }
        }
        public static DeviceList? Get(string sn)
        {
            return _index != null && _index.TryGetValue(sn, out var device)
                ? device
                : null;
        }
        private static string ExtractNumber(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";

            int i = name.LastIndexOfAny(['-','_']);
            if (i < 0 || i == name.Length - 1) return "";

            return name.Substring(i + 1);
        }
    }
}
