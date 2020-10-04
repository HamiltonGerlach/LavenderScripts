using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace Lavender.Debug
{
    public static class Log
    {
        public static List<string> Listed = new List<string>();
        public static Dictionary<string, string> Named = new Dictionary<string, string>();
        
        
        public static void Add(string Item) => Listed.Add(Item);
        public static void Add(GameObject Item) => Listed.Add(Item.name);
        public static void Add<T>(T Item) => Listed.Add(Item?.ToString() ?? "_null");
        
        
        public static void Add<T>(string Key, T Value) => Named.Add(Key, Value?.ToString() ?? "_nullvalue");
        public static void Add(string Key, GameObject Value) => Named.Add(Key, Value.name);
        public static void Add<T1, T2>(T1 Key, T2 Value) =>
            Named.Add(Key?.ToString() ?? "_nullkey", Value?.ToString() ?? "_nullvalue");
        
        
        public static string GetLast() => Listed.Last();
        public static string Get(string Key) => Named[Key];
        public static GameObject? GetGameObject(string Key) => GameObject.Find(Named[Key]) ?? null;
        
        
        public static void PurgeList() => Listed.Clear();
        public static void PurgeNamed() => Named.Clear();
        public static void Purge()
        {
            PurgeList(); PurgeNamed();
        }
    }
}