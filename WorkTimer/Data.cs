using Newtonsoft.Json;
using System;
using System.IO;

namespace WorkTimer
{
    public class Data
    {
        public static Data Instance;

        static string DATA_FILE_PATH
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + "/../../data.json"; }
        }

        public TimeSpan LeftTime = new TimeSpan(0, 0, 0);
        public TimeSpan TotalLeftTime = new TimeSpan(0, 0, 0);
        public TimeSpan MaxWorkTime = new TimeSpan(8, 0, 0);

        public int RoundMinutes = 15;

        public static void Load()
        {
            if (!File.Exists(DATA_FILE_PATH))
            {
                Instance = new Data();
                Save();
                return;
            }

            Instance = JsonConvert.DeserializeObject<Data>(File.ReadAllText(DATA_FILE_PATH));
        }

        public static void Save()
        {
            File.WriteAllText(DATA_FILE_PATH, JsonConvert.SerializeObject(Instance, Formatting.Indented));
        }
    }
}
