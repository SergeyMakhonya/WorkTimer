using System;
using System.Drawing;
using System.Windows.Forms;

namespace WorkTimer
{
    public partial class FormSetTime : Form
    {
        public TimeSpan Time
        {
            get { return new TimeSpan((int)numHours.Value, (int)numMinutes.Value, 0); }
            set { numHours.Value = value.Hours; numMinutes.Value = value.Minutes; }
        }

        public FormSetTime()
        {
            InitializeComponent();
        }

        private void FormSetTime_Load(object sender, EventArgs e)
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            Location = new Point(screenBounds.Width / 2 - Width / 2, screenBounds.Height / 2 - Height / 2);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
