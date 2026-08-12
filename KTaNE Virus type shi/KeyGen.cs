using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using static System.Windows.Forms.Design.AxImporter;

namespace KTaNE_Virus_type_shi
{
    internal class KeyGen
    {
        Random rng = new Random();

        private readonly string[] capchasList =
        {
            "capcha1.png",
            "capcha2.png",
            "capcha3.png",
            "capcha4.png",
            "capcha6.png"
        };

        private readonly string[] capchaKeys =
        {
            "bananabread",
            "garlic",
            "pornhub",
            "hubabuba",
            "cia gangstalking"
        };


        public string[] OddPuzzle =
        {
            "Creamy Peanut Butter",
            "Beans",
            "Honey",
            "Sugar",
            "Croissant",
            "Nutella",
            "Mitsubishi Materials",
            "Popcorn"
        };

        public string[] Options { get; private set; } = Array.Empty<string>();

        public int[] OddCorrectAnswers { get; private set; } = Array.Empty<int>();

        public int SelectedCapcha {  get; private set; }

        public string CapchaKey { get; private set; }


        public void GenerateOddOne()
        {
            Options = (string[])OddPuzzle.Clone();

            int first;
            int second;

            do
            {
                first = rng.Next(Options.Length);
                second = rng.Next(Options.Length);
            }
            while (first == second);

            OddCorrectAnswers = new[] { first, second };
            
        }

        public string GenerateCapcha()
        {
            SelectedCapcha = rng.Next(capchasList.Length);

            CapchaKey = capchaKeys[SelectedCapcha];

            return Path.Combine(
                AppContext.BaseDirectory, "Assets",
                capchasList[SelectedCapcha] 
                );
        }

    }
}
