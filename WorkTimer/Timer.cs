using System;

namespace WorkTimer
{
    public class Timer
    {
        public DateTime StartedAt { get; set; }

        public Timer(DateTime dt)
        {
            StartedAt = dt;
        }

        public TimeSpan GetLeftTime()
        {
            var ts = DateTime.Now.Subtract(StartedAt);
            return new TimeSpan(ts.Hours, ts.Minutes, 0);
        }
    }

    public static class TimeUtils
    {
        public static TimeSpan RoundTimespan(TimeSpan timeSpan, int minutes)
        {
            var totalMinutes = (int)(timeSpan + new TimeSpan(0, minutes / 2, 0)).TotalMinutes;
            return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
        }
    }
}
