using System;

namespace WorkTimer
{
    public class App
    {
        public bool IsEndWorkDay
        {
            get
            {
                TimeSpan leftTime = Timer.GetLeftTime();
                return Data.Instance.TotalLeftTime + leftTime >= Data.Instance.MaxWorkTime;
            }
        }

        public Timer Timer { get; private set; }

        public void Reset(bool firstLoad)
        {
            if (firstLoad)
            {
                var dt = DateTime.Now - Data.Instance.LeftTime;
                Timer = new Timer(dt);
            }
            else
            {
                Timer = new Timer(DateTime.Now);

                Data.Instance.LeftTime = new TimeSpan(0, 0, 0);
                Data.Instance.TotalLeftTime = new TimeSpan(0, 0, 0);
                Data.Save();
            }
        }

        public AddResult Add()
        {
            AddResult res;
            res.IsEndWorkDay = IsEndWorkDay;

            TimeSpan leftTime = Timer.GetLeftTime();

            if (IsEndWorkDay)
            {
                res.DisplayTime = leftTime.Hours.ToString("00") + ":" + leftTime.Minutes.ToString("00");
            }
            else
            {
                TimeSpan roundedTime = TimeUtils.RoundTimespan(leftTime, Data.Instance.RoundMinutes);
                TimeSpan diffTime;

                if (leftTime > roundedTime)
                {
                    diffTime = leftTime - roundedTime;
                }
                else
                {
                    diffTime = new TimeSpan(0, 0, 0);
                }

                Data.Instance.TotalLeftTime += roundedTime;
                Data.Instance.LeftTime = new TimeSpan(0, 0, 0);

                res.DisplayTime = roundedTime.Hours.ToString("00") + ":" + roundedTime.Minutes.ToString("00");

                var dt = DateTime.Now - diffTime;
                Timer = new Timer(dt);
            }

            Data.Save();

            return res;
        }

        string oldDisplayTimeStr = "00:00";
        public GetDisplayTimeResult GetDisplayTime()
        {
            GetDisplayTimeResult res;

            var time = Timer.GetLeftTime();
            res.DisplayTime = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00");
            res.IsChanged = oldDisplayTimeStr != res.DisplayTime;
            oldDisplayTimeStr = res.DisplayTime;

            return res;
        }
    }

    public struct AddResult
    {
        public string DisplayTime;
        public bool IsEndWorkDay;
    }

    public struct GetDisplayTimeResult
    {
        public string DisplayTime;
        public bool IsChanged;
    }
}
