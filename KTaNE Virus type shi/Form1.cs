using System.Diagnostics;
using System.Drawing.Text;
using System.Drawing;
using static KTaNE_Virus_type_shi.InstructionGen;
using System.Windows.Forms.VisualStyles;

namespace KTaNE_Virus_type_shi
{
    public partial class Form1 : Form
    {
        private readonly TimerHandler timerHandler;

        private readonly KeyGen keyGen = new();

        private readonly CheckBox[] checkBoxes;

        public int index;

        public bool DMSSafetySwitch { get; private set; } = false;

        private WatchDogClient watchDog = new WatchDogClient();

        public Form1()
        {

            DialogResult dialogResult = MessageBox.Show("CAUTION! \n " +
                "Read carefully! This is selection of difficulty mode! \n " +
                "While this software is purely recreational piece of loosely packed text that does " +
                "magic and is NOT malicious in any way, if you press YES right now, this may impact your stuff a bit, " +
                "since in this case the failure will result in OS Shutdown and may result in premature termination of " +
                "all currently run applications, which by coincidence may cause you to lose all unsaved progress.\n" +
                "With that being said, if you wish to purely look around or tamper a bit with the app itself, PLEASE PRESS NO.\n" +
                "Otherwise if you just wish to play high-stake game, press yes, but beware, it may or may not shutdown your pc prematurely if error occures\n" +
                "YOU HAVE BEEN WARNED", "CAUTION", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if(dialogResult == DialogResult.Yes)
            {
                DMSSafetySwitch = true;
            }


            MessageBox.Show("Im a very stronk virus, you have around 5 minutes to defuse the bomb " +
                "and you have to defuse me or you will get your lil PC shut down, " +
                "though without any consequences. Instructions are scattered around your C:// " +
                "and you have to find them, search documents, videos etc, you got the idea" +
                "and dont try to shut me down with Task manager or process hacker or Alt + F4 " +
                "or you will get powered off sooner than 5 minutes, have fun" +
                "         -- Sincerely, poor polish virus",
                "Hello saaar!",
                MessageBoxButtons.OK
                );


            //----------------------------------------------- Inits -----------------------------------------------\\


            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;

            WatchDogStartup();

            _ = ConnectWatchDogAsync();

            

            checkBoxes =
                [
                    checkBox1,
                    checkBox2,
                    checkBox3,
                    checkBox4,
                    checkBox5,
                    checkBox6,
                    checkBox7
                ];

            toolTip1.SetToolTip(labelToolTip1, "Elite ball knowledge correlation, but two are not related, maybe");

            toolTip1.SetToolTip(labelToolTip2, "Simple CAPCHA, find what you gotta look for");

            toolTip1.SetToolTip(labelToolTip3, "Cliché, isnt it? Cut one wire");

            toolTip1.SetToolTip(labelToolTip4, "One question, obvious really, but what is it?");

            toolTip1.SetToolTip(labelToolTip5, "Hope you were good with history, twin");

            progressBar1.Minimum = 0;

            progressBar1.Maximum = 100;

            progressBar1.Value = 0;

            timerHandler = new TimerHandler(60);

            timerHandler.TimeChanged += seconds =>
            {
                label1.Text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
            };

            timerHandler.Start();

            keyGen.GenerateOddOne();

            keyGen.WiresGenerator();

            keyGen.DateGenerator();

            string ImagePath = keyGen.GenerateCapcha();

            Debug.WriteLine(ImagePath);

            pictureBox1.Image = Image.FromFile(ImagePath);

            //new InstructionGen();

            checkedListBox1.Items.AddRange(keyGen.Options);


            for (int i = 0; i < checkBoxes.Length; i++)
            {

                if (keyGen.WireOrder[i] == null)
                {

                    checkBoxes[i].Text = "";

                    checkBoxes[i].Enabled = false;

                }
                else
                {

                    checkBoxes[i].Text = "=========";

                    checkBoxes[i].ForeColor = Color.FromName(keyGen.WireOrder[i]);

                    checkBoxes[i].Tag = i;

                    checkBoxes[i].CheckedChanged += Wire_CheckedChanged;
                }


            }


            //----------------------------------------------- Debug -----------------------------------------------\\


            Debug.WriteLine(string.Join(", ", keyGen.OddCorrectAnswers));

            Debug.WriteLine(keyGen.CapchaKey);

            Debug.WriteLine(string.Join(", ", keyGen.WireOrder));

            Debug.WriteLine(keyGen.CorrectWire);

            Debug.WriteLine(keyGen.correctDate);


        }




        //---------------------------------------------- Compts -----------------------------------------------\\


        private void button2_Click(object sender, EventArgs e)
        {

            bool key1 = checkedListBox1.CheckedIndices.Contains(keyGen.OddCorrectAnswers[0]);

            bool key2 = checkedListBox1.CheckedIndices.Contains(keyGen.OddCorrectAnswers[1]);

            if (key1 && key2)
            {

                UpdateProgress(20);

                button2.Enabled = false;

                button2.BackColor = Color.Green;

                checkedListBox1.Enabled = false;

            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            string answerCapcha = textBox1.Text.ToLower();

            if (answerCapcha == keyGen.CapchaKey)
            {

                UpdateProgress(20);

                button3.Enabled = false;

                button3.BackColor = Color.Green;

            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBoxes[keyGen.CorrectWire].Checked)
            {

                UpdateProgress(20);

                button4.Enabled = false;

                button4.BackColor = Color.Green;

                checkBoxes[keyGen.CorrectWire].Text = "====   ====";

                foreach (CheckBox checkBox in checkBoxes)
                {
                    checkBox.Enabled = false;
                }

            }
            else
            {
                checkBoxes[index].Text = "====   ====";
            }
        }


        private void Wire_CheckedChanged(object? sender, EventArgs e)
        {
            CheckBox wire = (CheckBox)sender!;

            index = (int)wire.Tag;

            if (wire.Checked)
            {
                Debug.WriteLine($"Wire {index} was selected");
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == keyGen.SimpleQuestion)
            {

                UpdateProgress(100);

                button5.Enabled = false;

                button5.BackColor = Color.Green;

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date == keyGen.correctDate)
            {

                UpdateProgress(20);

                button1.Enabled = false;

                button1.BackColor = Color.Green;

                dateTimePicker1.Enabled = false;

            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.F4))
            {
                MessageBox.Show("You really thought this would work, didnt you?", "Seriously?");
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void WatchDogStartup()
        {
            string watchdogPath = @"C:\\Users\\scher\\source\\repos\\KTaNE Virus type shi\\KTaNEWatchDog\\bin\\Debug\\net10.0\\KTaNEWatchDog.exe";

            Process.Start(new ProcessStartInfo
            {
                FileName = watchdogPath,

                Arguments = Environment.ProcessId.ToString(),
                UseShellExecute = false
            });
        }


        private async Task ConnectWatchDogAsync()
        {
            await watchDog.ConnectAsync();
            await watchDog.DeadMansSwitchSafety(DMSSafetySwitch);
            watchDog.HeartBeat();
            _ = watchDog.ListenAsync();
            watchDog.WatchdogECG();
        }


        private async Task UpdateProgress(int progress)
        {
            progressBar1.Value += progress;

            if(progressBar1.Value == 100)
            {
                Console.WriteLine("Bomb Defused");
                timerHandler.Stop();
                await DefusalComplete();
            }
        }


        private async Task DefusalComplete()
        {
            await watchDog.SendMessageAsync("SHUTDOWN_APPROVED");

            MessageBox.Show("Congrats, you have defused the bomb, the program will be terminated shortly");
            Thread.Sleep(3000);

            this.Close();
        }
    }
}
