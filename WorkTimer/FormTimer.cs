using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WorkTimer
{
    public partial class FormTimer : Form
    {
        readonly string DATA_FILE_PATH = AppDomain.CurrentDomain.BaseDirectory + "/../../data.json";

        public bool IsEndWorkDay
        {
            get
            {
                TimeSpan leftTime = timer.GetLeftTime();
                return data.TotalLeftTime + leftTime >= data.MaxWorkTime;
            }
        }

        Timer timer;
        readonly Pen penFormBorder = new Pen(Color.FromArgb(16, 16, 16));
        IEnumerator<Color> coroutineBlink;
        string oldLabelTimeText = "00:00";

        Data data;

        public FormTimer()
        {
            InitializeComponent();
        }

        void LoadData()
        {
            if (!File.Exists(DATA_FILE_PATH))
            {
                data = new Data();
                return;
            }

            data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(DATA_FILE_PATH));

            if (data.TotalLeftTime >= data.MaxWorkTime)
                data.TotalLeftTime = new TimeSpan(0, 0, 0);
        }

        void SaveData()
        {
            if (data.IsChanged)
            {
                File.WriteAllText(DATA_FILE_PATH, JsonConvert.SerializeObject(data));
                data.IsChanged = false;
            }
        }

        private void FormTimer_Load(object sender, EventArgs e)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                Close();
                return;
            }

            LoadData();

            Size = new Size(85, 32);

            labelTime.Location = new Point(Width / 2 - labelTime.Width / 2, Height / 2 - labelTime.Height / 2);

            btnNew.Location = new Point(Width - btnNew.Width - 2, 2);
            btnResume.Location = new Point(Width - btnResume.Width - 2, Height - btnResume.Height - 2);
            btnPause.Location = btnResume.Location;

            btnNew.MouseEnter += (o, a) => { btnNew.Location = new Point(btnNew.Location.X + 1, btnNew.Location.Y + 1); };
            btnResume.MouseEnter += (o, a) => { btnResume.Location = new Point(btnResume.Location.X + 1, btnResume.Location.Y + 1); };
            btnPause.MouseEnter += (o, a) => { btnPause.Location = new Point(btnPause.Location.X + 1, btnPause.Location.Y + 1); };

            btnNew.MouseLeave += (o, a) => { btnNew.Location = new Point(btnNew.Location.X - 1, btnNew.Location.Y - 1); };
            btnResume.MouseLeave += (o, a) => { btnResume.Location = new Point(btnResume.Location.X - 1, btnResume.Location.Y - 1); };
            btnPause.MouseLeave += (o, a) => { btnPause.Location = new Point(btnPause.Location.X - 1, btnPause.Location.Y - 1); };

            btnNew.Click += (o, a) => {
                if (MessageBox.Show("Начать новое задание?", "WorkTimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Add();
                }
            };

            TopMost = true;

            Reset(true);

            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Сколько прошло с начала рабочего дня", null, (o, a) => { SetTotalLeftTime(); });
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Начать новый день", null, (o, a) => {
                if (MessageBox.Show("Начать новый день?", "WorkTimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ResetDay();
                }
            });
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Закрыть", null, (o, a) => { CloseApp(); });
            ContextMenuStrip = ctxMenu;

            var screenBounds = Screen.PrimaryScreen.Bounds;
            Location = new Point(screenBounds.Width - Width - 150, 0);
        }

        void CloseApp()
        {
            if (IsEndWorkDay)
            {
                data = new Data();
            }
            else
            {
                data.LeftTime = timer.GetLeftTime();
            }

            SaveData();
            Close();
        }

        void ResetDay()
        {
            data = new Data();

            SaveData();
            Reset(false);
        }

        void Reset(bool firstLoad)
        {
            if (firstLoad)
            {
                var dt = DateTime.Now - data.LeftTime;
                timer = new Timer(dt);
            }
            else
            {
                timer = new Timer(DateTime.Now);
            }

            timerMinute.Enabled = true;
            timerBlink.Enabled = false;

            UpdateLabelTime();
            UpdateView();
        }

        void Add()
        {
            TimeSpan leftTime = timer.GetLeftTime();

            if (IsEndWorkDay)
            {
                labelTime.Text = leftTime.Hours.ToString("00") + ":" + leftTime.Minutes.ToString("00");
                coroutineBlink = CoroutineGetBlinkColor(int.MaxValue);
                btnNew.Visible = false;
            }
            else
            {
                TimeSpan roundedTime = TimeUtils.RoundTimespan(leftTime, data.RoundMinutes);
                TimeSpan diffTime;

                if (leftTime > roundedTime)
                {
                    diffTime = leftTime - roundedTime;
                }
                else
                {
                    diffTime = new TimeSpan(0, 0, 0);
                }

                data.TotalLeftTime += roundedTime;
                data.LeftTime = new TimeSpan(0, 0, 0);

                coroutineBlink = CoroutineGetBlinkColor(3);
                labelTime.Text = roundedTime.Hours.ToString("00") + ":" + roundedTime.Minutes.ToString("00");

                var dt = DateTime.Now - diffTime;
                timer = new Timer(dt);
            }

            timerMinute.Enabled = false;
            timerBlink.Enabled = true;

            SaveData();
            Refresh();
        }

        void SetTotalLeftTime()
        {
            var dlg = new FormSetTime();
            dlg.Text = "Сколько прошло с начала рабочего дня";
            dlg.Time = data.TotalLeftTime;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                data.TotalLeftTime = dlg.Time;
                UpdateLabelTime();
                Refresh();
            }
        }

        void UpdateView()
        {
            btnNew.Visible = !IsEndWorkDay;
            btnResume.Visible = false; // isPause && !IsEndWorkDay;
            btnPause.Visible = false; // !isPause && !IsEndWorkDay;
        }

        private void FormTimer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                User32.ReleaseCapture();
                User32.SendMessage(Handle, User32.WM_NCLBUTTONDOWN, User32.HT_CAPTION, 0);
            }
        }

        private void timerMinute_Tick(object sender, EventArgs e)
        {
            if (IsEndWorkDay)
                Add();
            else
                UpdateLabelTime();
        }
        
        void UpdateLabelTime()
        {
            var time = timer.GetLeftTime();
            string timeStr = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00");

            if (oldLabelTimeText != timeStr)
            {
                labelTime.Text = timeStr;
                Refresh();
            }

            oldLabelTimeText = timeStr;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(penFormBorder, 0, 0, Width - 1, Height - 1);

            var leftTime = timer.GetLeftTime();
            var totalSeconds = data.TotalLeftTime.TotalSeconds + leftTime.TotalSeconds;
            var percent = totalSeconds / data.MaxWorkTime.TotalSeconds;
            e.Graphics.FillRectangle(Brushes.Green, 0, Height - 3, (int)(Width * percent), 3);
        }

        // Скрывает окно от Alt+Tab
        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;

                return Params;
            }
        }

        IEnumerator<Color> CoroutineGetBlinkColor(int count)
        {
            int i = 0;

            while (i < count)
            {
                yield return Color.Green;
                yield return Color.Silver;
                i++;
            }
        }

        private void timerBlink_Tick(object sender, EventArgs e)
        {
            if (coroutineBlink.MoveNext())
            {
                labelTime.ForeColor = coroutineBlink.Current;
            }
            else
            {
                UpdateLabelTime();
                coroutineBlink = null;
                timerBlink.Enabled = false;
                timerMinute.Enabled = true;
            }
        }
    }
}
