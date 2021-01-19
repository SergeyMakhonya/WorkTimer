using Newtonsoft.Json;
using System;

namespace WorkTimer
{
    public class Data
    {
        TimeSpan leftTime = new TimeSpan(0, 0, 0);
        TimeSpan totalLeftTime = new TimeSpan(0, 0, 0);
        TimeSpan maxWorkTime = new TimeSpan(8, 0, 0);
        int roundMinutes = 15;

        [JsonIgnore]
        public bool IsChanged { get; set; } = true;

        public TimeSpan LeftTime { get => leftTime; set { leftTime = value; IsChanged = true; } }
        public TimeSpan TotalLeftTime { get => totalLeftTime; set { totalLeftTime = value; IsChanged = true; } }
        public TimeSpan MaxWorkTime { get => maxWorkTime; set { maxWorkTime = value; IsChanged = true; } }
        public int RoundMinutes { get => roundMinutes; set { roundMinutes = value; IsChanged = true; } }
    }
}
