using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EHMAssistant.ModifyRoster;

namespace EHMAssistant
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }

        // Static properties to store the player data
        public static Dictionary<int, string> PlayerDictionary { get; set; }
        public static Dictionary<string, Team> PlayerTeamDictionary { get; set; }
        public static List<Player> Players { get; set; }
    }
}
