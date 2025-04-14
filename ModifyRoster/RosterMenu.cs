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

            if (Program.Players != null && Program.PlayerDictionary != null && Program.PlayerTeamDictionary != null)
            {
                LoadPlayerData();
            }
        }

        private void LoadPlayerData()
        {
            // Here you can access:
            // Program.Players - List of Player objects
            // Program.PlayerDictionary - Dictionary<int, string> mapping line numbers to player names
            // Program.PlayerTeamDictionary - Dictionary<string, Team> mapping player names to teams

            // Populate your form with the data
            // For example, if you have a DataGridView:
            // dataGridView1.DataSource = Program.Players;

            // Or if you have a ComboBox for filtering by team:
            // comboBoxTeams.DataSource = Program.PlayerTeamDictionary.Values.Distinct().ToList();
            // comboBoxTeams.DisplayMember = "Name";
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

            // Suppress the SelectedIndexChanged event during form load
            // Temporarily unsubscribe from the event handler
            cboTeams.SelectedIndexChanged -= cboTeams_SelectedIndexChanged;

            if (cboTeams.SelectedValue != null)
            {
                LoadTeamRoster((int)cboTeams.SelectedValue);
            }

            // Re-subscribe to the event handler
            cboTeams.SelectedIndexChanged += cboTeams_SelectedIndexChanged;
        }

        private void cboTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure there's a valid selection before proceeding
            if (cboTeams.SelectedValue == null || !(cboTeams.SelectedValue is int selectedTeamNumber))
                return;

            // Load the players for the selected team
            LoadTeamRoster(selectedTeamNumber);
        }

        private void LoadTeamRoster(int teamNumber)
        {
            // Filter the players based on the selected team number
            var filteredPlayers = Program.Players
                .Where(p => p.TeamNumber == teamNumber)
                .ToList();

            // Set the DataSource for the DataGridView to the filtered players
            dgvTeamRoster.DataSource = filteredPlayers;

            // Optionally, you can set the columns for better presentation
            dgvTeamRoster.Columns["TeamNumber"].Visible = false;  // Hide TeamNumber column
        }
    }
}
