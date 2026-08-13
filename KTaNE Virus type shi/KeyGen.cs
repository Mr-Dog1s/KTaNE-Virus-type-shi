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

        public List<string> Wires = new()
        {
            "red",
            "blue",
            "green",
            "yellow",
            "purple",
            "black",
            "brown"
        };



        public string[] Options { get; private set; } = Array.Empty<string>();

        public int[] OddCorrectAnswers { get; private set; } = Array.Empty<int>();

        public int SelectedCapcha {  get; private set; }

        public string? CapchaKey { get; private set; }

        public List<string>? WireOrder { get; private set; } = new();

        public int CorrectWire { get; private set; } = -1;

        public bool SimpleQuestion { get; private set; } = true;

        public DateTime correctDate { get; private set; }

        private readonly List<(DateTime Date, string Clue)> Dates = new()
        {
            (new DateTime(1889, 4, 20),"One artistic man heard 'BLAZE IT' and did so literaly, when he was born tho?"),
            (new DateTime(1953, 3, 5), "GULAG? Never happened, oh shi? i got a stroke call the doctor.....ah.... they are all jews"),
            (new DateTime(2001, 9, 11), "Turning page of history, and it had George Bush present, but which George?"),
            (new DateTime(1773, 12, 16), "Largest tea-party in history that made Brits very angry"),
            (new  DateTime(1986, 4, 26),"Was politburo retarded? Probably, but this was the last retarded moment resulting in 3 being replaced with 15.000 ")
        };


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

        public void WiresGenerator()
        {
            for (int i = 0; i < 7; i++)
            {
                int selected = rng.Next(Wires.Count);
                WireOrder.Add(Wires[selected]);
                Wires.RemoveAt(selected);
            }
            for (int i = 0; i < 2; i++)
            {
                WireOrder[rng.Next(WireOrder.Count)] = null;
            }
            while (CorrectWire == -1)
            {
                int candidate = rng.Next(WireOrder.Count);
                if (!string.IsNullOrEmpty(WireOrder[candidate]))
                {
                    CorrectWire = candidate;
                }
            }
        }

        public void DateGenerator()
        {
            int selected = rng.Next(Dates.Count);
            correctDate = Dates[selected].Date;
        }
    }
}
