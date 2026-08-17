using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KTaNE_Virus_type_shi
{
    internal class ShutdownHandler
    {
        private readonly bool dmsSafetySwitch;

        private readonly Form1 form1;

        public ShutdownHandler(Form1 form1)
        {
            this.form1 = form1;
        }

            public void ShutdownHandlerExec(bool dmsSafetySwitch)
        {

            if (dmsSafetySwitch)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "shutdown.exe",
                    Arguments = "/s /t 10",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            else
            {
                Debug.WriteLine("Safety enabled, exiting process");
                Thread.Sleep(1000);
                form1.Close();
            }
        }
    }
}

