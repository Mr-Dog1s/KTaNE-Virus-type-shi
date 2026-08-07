using static KTaNE_Virus_type_shi.InstructionGen;

namespace KTaNE_Virus_type_shi
{
    public partial class Form1 : Form
    {
        private readonly TimerHandler timerHandler;

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

            InitializeComponent();

            timerHandler = new TimerHandler(60);

            timerHandler.TimeChanged += seconds =>
            {
                label1.Text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
            };

            timerHandler.Start();
            //new InstructionGen();
        
            
        
        
        
        }
    }
}
