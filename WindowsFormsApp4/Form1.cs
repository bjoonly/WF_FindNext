using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        Timer Timer = new Timer();
        int timeLeft = 0;
        int maxTime = 40;
        Color defColor;

        private List<int> numbers;
        public Form1()
        {
            InitializeComponent();
            Timer.Tick += TimerTick;
            numbers = new List<int>(ButtonsPanel.Controls.Count);
            for(int i=1;i<= ButtonsPanel.Controls.Count; i++)
            {
                numbers.Add(i);

            }
            defColor = StatrtButton.BackColor;
            timeToolStripStatusLabel1.Text = maxTime + " s";
        }
        void TimerTick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                --timeLeft;
                timeToolStripStatusLabel1.Text = timeLeft.ToString() + " s";

            }
            else
            {
                Timer.Stop();
                timeToolStripStatusLabel1.Text = "0 s";
                MessageBox.Show("You lose!", "Sorry",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                DifficultyTrackBar.Enabled = true;
                foreach (Button item in ButtonsPanel.Controls.OfType<Button>())
                {
                    item.Enabled = false;
                }
            }

        }
        private void StatrtButton_Click(object sender, EventArgs e)
        {

            DifficultyTrackBar_Scroll(DifficultyTrackBar, e);
            toolStripProgressBar.Value = 0;
            Timer.Start();
            DifficultyTrackBar.Enabled = false;
            NextNumberToolStripStatusLabel.Text = "1";
            for (int i = numbers.Count - 1; i >= 0; i--)
            {

                int j = random.Next(i + 1);
                int tmp = numbers[j];
                numbers[j] = numbers[i];
                numbers[i] = tmp;
            }
            int count = 0;
            foreach (Button item in ButtonsPanel.Controls)
            {
                item.BackColor = defColor;
                item.Text = numbers[count].ToString();
                item.Enabled = true;
                count++;
            }

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (String.IsNullOrEmpty(button.Text))
                return;

            if ( Convert.ToInt32(NextNumberToolStripStatusLabel.Text) == Convert.ToInt32(button.Text))
            {
                foreach (Button item in ButtonsPanel.Controls)
                {
                    if (item.BackColor == Color.Red)
                    {
                        item.BackColor = defColor;

                    }
                }
                if (toolStripProgressBar.Value + 7 > toolStripProgressBar.Maximum)
                    toolStripProgressBar.Value = toolStripProgressBar.Maximum;
                else
                    toolStripProgressBar.Value += 7;

                button.BackColor = Color.Green;

                button.Enabled = false;
                if (Convert.ToInt32(NextNumberToolStripStatusLabel.Text) < ButtonsPanel.Controls.Count)
                    NextNumberToolStripStatusLabel.Text = (Convert.ToInt32(button.Text) + 1).ToString();
                else
                {
                    Timer.Stop();
                    MessageBox.Show($"You win. It took you {maxTime - timeLeft} seconds", "Congratulations");
                    DifficultyTrackBar.Enabled = true;

                }
            }
            else
            {
                button.BackColor = Color.Red;
            }
        }

        private void DifficultyTrackBar_Scroll(object sender, EventArgs e)
        {
            Timer.Interval = 1000;
            
            var trackBar = (TrackBar)sender;
            if (trackBar.Value == 1)
            {
                maxTime = timeLeft = 40;

            }
            else if (trackBar.Value == 2)
            {
                maxTime = timeLeft = 25;
            }
            else
            {
                maxTime = timeLeft = 15;
            }
            timeToolStripStatusLabel1.Text = maxTime + " s";
        }
    }
}
