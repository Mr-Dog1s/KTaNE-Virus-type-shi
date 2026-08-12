using System.Diagnostics;
using System.Drawing.Text;
using System.Drawing;
using static KTaNE_Virus_type_shi.InstructionGen;

namespace KTaNE_Virus_type_shi
{
    public partial class Form1 : Form
    {
        private readonly TimerHandler timerHandler;

        private readonly KeyGen keyGen = new();

        private readonly CheckBox[] checkBoxes;

        public Form1()
        {
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
                }

            }


            //----------------------------------------------- Debug -----------------------------------------------\\


            Debug.WriteLine(string.Join(", ", keyGen.OddCorrectAnswers));

            Debug.WriteLine(keyGen.CapchaKey);

            Debug.WriteLine(string.Join(", ", keyGen.WireOrder));

            Debug.WriteLine(keyGen.CorrectWire);
        }


        //---------------------------------------------- Compts -----------------------------------------------\\


        private void button2_Click(object sender, EventArgs e)
        {
            bool key1 = checkedListBox1.CheckedIndices.Contains(keyGen.OddCorrectAnswers[0]);
            bool key2 = checkedListBox1.CheckedIndices.Contains(keyGen.OddCorrectAnswers[1]);

            if (key1 && key2)
            {
                progressBar1.Value += 20;
                button2.Enabled = false;
                button2.BackColor = Color.Green;
                checkedListBox1.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string answerCapcha = textBox1.Text.ToLower();
            Debug.WriteLine(answerCapcha);
            if (answerCapcha == keyGen.CapchaKey)
            {
                progressBar1.Value += 20;
                button3.Enabled = false;
                button3.BackColor = Color.Green;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBoxes[keyGen.CorrectWire].Checked)
            {
                progressBar1.Value += 20;
                button4.Enabled = false;
                button4.BackColor = Color.Green;
            }
        }
    }
}
