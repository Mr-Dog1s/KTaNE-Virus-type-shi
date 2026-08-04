using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KTaNE_Virus_type_shi
{
    internal class InstructionGen
    {
        public InstructionGen() {

            Random rng = new();
            int index = 0;

            string[] Instructions = {
                "Odd Item",
                "Capcha",
                "Wire cuttin",
                "Are you gae?",
                "Pick a date, twin",
            };

            var Folders = new List<string>
            {
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            };

            foreach (string folder in Folders.OrderBy(_ => rng.Next()).Take(4))
            {
                string file = Path.Combine(folder, "instruction.txt");
                index = index + 1;
                File.WriteAllText(file, Instructions[index]);
                Debug.WriteLine(folder);
            }
        }
    }
}
