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

        public int[] CorrectAnswers { get; private set; } = Array.Empty<int>();


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

            CorrectAnswers = new[] { first, second };
            
        }

    }
}
