using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WorkTimer
{
    public partial class FormTimer : Form
    {
        readonly Pen penFormBorder = new Pen(Color.FromArgb(16, 16, 16));
        IEnumerator<Color> coroutineBlink;

        App app;

        public FormTimer()
        {
            InitializeComponent();
        }

        private void FormTimer_Load(object sender, EventArgs e)
        {
            /*if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                Close();
                return;
            }*/

            Data.Load();

            if (Data.Instance.TotalLeftTime >= Data.Instance.MaxWorkTime)
                Data.Instance.TotalLeftTime = new TimeSpan(0, 0, 0);

            app = new App();

            btnNew.Click += (o, a) => {
                if (MessageBox.Show("Начать новое задание?", "WorkTimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Add();
                }
            };

            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Сколько прошло с начала рабочего дня", null, (o, a) => { SetTotalLeftTime(); });
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Начать новый день", null, (o, a) => {
                if (MessageBox.Show("Начать новый день?", "WorkTimer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Reset(false);
                }
            });
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Закрыть", null, (o, a) => { CloseApp(); });
            ContextMenuStrip = ctxMenu;

            CalcView();
            Reset(true);
        }

        void CalcView()
        {
            Size = new Size(80, 32);

            labelTime.Location = new Point(Width / 2 - labelTime.Width / 2, (Height - Config.TOTAL_LEFT_TIME_BAR_HEIGHT) / 2 - labelTime.Height / 2);

            btnNew.Location = new Point(Width - btnNew.Width - 2, 2);
            btnResume.Location = new Point(Width - btnResume.Width - 2, Height - btnResume.Height - 2);
            btnPause.Location = btnResume.Location;

            btnNew.MouseEnter += (o, a) => { btnNew.Location = new Point(btnNew.Location.X + 1, btnNew.Location.Y + 1); };
            btnResume.MouseEnter += (o, a) => { btnResume.Location = new Point(btnResume.Location.X + 1, btnResume.Location.Y + 1); };
            btnPause.MouseEnter += (o, a) => { btnPause.Location = new Point(btnPause.Location.X + 1, btnPause.Location.Y + 1); };

            btnNew.MouseLeave += (o, a) => { btnNew.Location = new Point(btnNew.Location.X - 1, btnNew.Location.Y - 1); };
            btnResume.MouseLeave += (o, a) => { btnResume.Location = new Point(btnResume.Location.X - 1, btnResume.Location.Y - 1); };
            btnPause.MouseLeave += (o, a) => { btnPause.Location = new Point(btnPause.Location.X - 1, btnPause.Location.Y - 1); };

            var screenBounds = Screen.PrimaryScreen.Bounds;
            Location = new Point(screenBounds.Width - Width - 150, 0);

            TopMost = true;
        }

        void CloseApp()
        {
            if (app.IsEndWorkDay)
            {
                Data.Instance = new Data();
            }
            else
            {
                Data.Instance.LeftTime = app.Timer.GetLeftTime();
            }

            Data.Save();
            Close();
        }

        void Reset(bool firstLoad)
        {
            app.Reset(firstLoad);

            timerMinute.Enabled = true;
            timerBlink.Enabled = false;

            UpdateLabelTime();
            UpdateView();
        }

        void Add()
        {
            var addResult = app.Add();
            labelTime.Text = addResult.DisplayTime;

            if (addResult.IsEndWorkDay)
            {
                coroutineBlink = CoroutineGetBlinkColor(int.MaxValue);
                btnNew.Visible = false;
            }
            else
            {
                coroutineBlink = CoroutineGetBlinkColor(3);
            }

            timerMinute.Enabled = false;
            timerBlink.Enabled = true;

            Refresh();
        }

        void SetTotalLeftTime()
        {
            var dlg = new FormSetTime();
            dlg.Text = "Сколько прошло с начала рабочего дня";
            dlg.Time = Data.Instance.TotalLeftTime;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Data.Instance.TotalLeftTime = dlg.Time;
                UpdateLabelTime();
                Refresh();
            }
        }

        void UpdateView()
        {
            btnNew.Visible = !app.IsEndWorkDay;
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
            if (app.IsEndWorkDay)
                Add();
            else
                UpdateLabelTime();
        }
        
        void UpdateLabelTime()
        {
            var getDisplayTimeResult = app.GetDisplayTime();

            if (getDisplayTimeResult.IsChanged)
            {
                labelTime.Text = getDisplayTimeResult.DisplayTime;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(penFormBorder, 0, 0, Width - 1, Height - 1);

            var leftTime = app.Timer.GetLeftTime();
            var totalSeconds = Data.Instance.TotalLeftTime.TotalSeconds + leftTime.TotalSeconds;
            var percent = totalSeconds / Data.Instance.MaxWorkTime.TotalSeconds;
            e.Graphics.FillRectangle(Brushes.Green, 0, Height - Config.TOTAL_LEFT_TIME_BAR_HEIGHT, (int)(Width * percent), Config.TOTAL_LEFT_TIME_BAR_HEIGHT);
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
