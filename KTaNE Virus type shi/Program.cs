using static KTaNE_Virus_type_shi.InstructionGen;

namespace KTaNE_Virus_type_shi

{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}