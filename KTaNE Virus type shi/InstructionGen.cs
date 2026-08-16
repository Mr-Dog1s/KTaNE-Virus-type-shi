using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KTaNE_Virus_type_shi
{
    internal class InstructionGen
    {

        private readonly KeyGen keyGen = new();

        public InstructionGen() {

            Random rng = new();
            int index = 0;

            string[] Instructions =
            {
                "",
                "",
                "",
                "",
                ""
            };


            //-------------------------------ODD PUZZLE-------------------------------\\


            for (int i = 0; i < keyGen.OddCorrectAnswers.Length; i++)
            {
                switch (keyGen.OddCorrectAnswers[i])
                {
                    case 0:
                        {
                            Instructions[0] += "hope you dont have a peanut allergy \n";
                            break;
                        }
                    case 1:
                        {
                            Instructions[0] += "Pairs badly with toast \n";
                            break;
                        }
                    case 2:
                        {
                            Instructions[0] += "10.000 bees asked for you to come outside \n";
                            break;
                        }
                    case 3:
                        {
                            Instructions[0] += "Diabetes? Never heard of him \n";
                            break;
                        }
                    case 4:
                        {
                            Instructions[0] += "EWWWWW, french \n";
                            break;
                        }
                    case 5:
                        {
                            Instructions[0] += "OHHHH, ARIGATOU GOZAIMASU type shi \n";
                            break;
                        }
                    case 6:
                        {
                            Instructions[0] += "We AINT watching shi \n";
                            break;
                        }
                }
            }


            //---------------------------------CAPCHA---------------------------------\\


            switch (keyGen.SelectedCapcha)
            {
                case 0:
                    {
                        Instructions[1] = "BWAH BWAH TUNG TUNG TUNG SAHUUUUR";
                        break;
                    }
                case 1:
                    {
                        Instructions[1] = "Master of this skvorechnik";
                        break;
                    }
                case 2:
                    {
                        Instructions[1] = "Take the hint already";
                        break;
                    }
                case 3:
                    {
                        Instructions[1] = "Hey, that gum sucks ass";
                        break;
                    }
                case 4:
                    {
                        Instructions[1] = "Just MC build, nuthin else";
                        break;
                    }
            }


            //----------------------------------WIRE----------------------------------\\


            if(keyGen.CorrectWire <= 4)
            {
                Instructions[2] = "Cut somewhere in upper half, idk";
            }
            else
            {
                Instructions[2] = "OOOOF, wire is in the lower half";
            }


            //----------------------------------MISC----------------------------------\\


            // Simple question

            Instructions[3] = "Are you gay?";

            //Date picker

            Instructions[4] = keyGen.Dates[keyGen.correctDateIndex].Clue;




            //------------------------------------------------------------------------\\




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
                Debug.WriteLine(Instructions[index]);
            }
        }
    }
}
