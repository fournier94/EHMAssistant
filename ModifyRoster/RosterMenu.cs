using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EHMAssistant.ModifyRoster.Tabs;

namespace EHMAssistant.ModifyRoster
{
    public partial class RosterMenu : Form
    {
        public RosterMenu()
        {
            InitializeComponent();
        }

        private void RosterMenu_Load(object sender, EventArgs e)
        {
            TabControllerTeamsRoster teamsTab = new TabControllerTeamsRoster();
            teamsTab.Dock = DockStyle.Fill;

            // Populate cboTeams with NHL team names
            cboTeams.DataSource = Teams.TeamList.Values.Where(t => t.IsNHL).OrderBy(t => t.Number).ToList();
            cboTeams.DisplayMember = "Name";  // Display the team name
            cboTeams.ValueMember = "Number";  // Store the team number as value

            // Center the ComboBox horizontally within its parent container
            cboTeams.Left = (this.Width - cboTeams.Width) / 2;
        }

        private void cboTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
