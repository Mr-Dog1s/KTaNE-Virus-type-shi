using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Timer = System.Windows.Forms.Timer;

namespace KTaNE_Virus_type_shi
{
    internal class TimerHandler
    {

        private readonly Timer timer = new Timer();
        private int secondsLeft;
        private int startingSeconds = 3000;

        public event Action<int>? TimeChanged;
        public event Action? TimerFinished;


        public TimerHandler(int startingSeconds)
        {
            secondsLeft = startingSeconds;
            
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            secondsLeft--;

            TimeChanged?.Invoke(secondsLeft);

            if (secondsLeft <= 0) {
                timer.Stop();
                MessageBox.Show("Disarming failed, you gonna lose power soon", "You done fucked up boy", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }


    }
}
