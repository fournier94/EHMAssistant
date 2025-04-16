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

            // Disable visual styles to allow custom header alignment
            dgvTeamRoster.EnableHeadersVisualStyles = false;

            // Center header text (set this ONCE before modifying columns)
            dgvTeamRoster.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Remove row headers
            dgvTeamRoster.RowHeadersVisible = false;

            #region Customize Columns
            
            dgvTeamRoster.Columns["Name"].HeaderText = "Nom";
            dgvTeamRoster.Columns["Name"].Width = 220;
            dgvTeamRoster.Columns["Height"].HeaderText = "Grandeur";
            dgvTeamRoster.Columns["Height"].Width = 80;
            dgvTeamRoster.Columns["PlayerPosition"].HeaderText = "Position";
            dgvTeamRoster.Columns["PlayerPosition"].Width = 75;
            dgvTeamRoster.Columns["PlayerType"].HeaderText = "Type";
            dgvTeamRoster.Columns["PlayerType"].Width = 100;
            dgvTeamRoster.Columns["PositionStrength"].HeaderText = "Force";
            dgvTeamRoster.Columns["PositionStrength"].Width = 100;
            dgvTeamRoster.Columns["StartingFighting"].HeaderText = "FI";
            dgvTeamRoster.Columns["StartingFighting"].Width = 40;
            dgvTeamRoster.Columns["StartingShooting"].HeaderText = "SH";
            dgvTeamRoster.Columns["StartingShooting"].Width = 50;
            dgvTeamRoster.Columns["StartingPlaymaking"].HeaderText = "PL";
            dgvTeamRoster.Columns["StartingPlaymaking"].Width = 50;
            dgvTeamRoster.Columns["StartingStickhandling"].HeaderText = "ST";
            dgvTeamRoster.Columns["StartingStickhandling"].Width = 50;
            dgvTeamRoster.Columns["StartingChecking"].HeaderText = "CH";
            dgvTeamRoster.Columns["StartingChecking"].Width = 50;
            dgvTeamRoster.Columns["StartingPositioning"].HeaderText = "PO";
            dgvTeamRoster.Columns["StartingPositioning"].Width = 50;
            dgvTeamRoster.Columns["StartingHitting"].HeaderText = "HI";
            dgvTeamRoster.Columns["StartingHitting"].Width = 50;
            dgvTeamRoster.Columns["StartingSkating"].HeaderText = "SK";
            dgvTeamRoster.Columns["StartingSkating"].Width = 50;
            dgvTeamRoster.Columns["StartingEndurance"].HeaderText = "EN";
            dgvTeamRoster.Columns["StartingEndurance"].Width = 50;
            dgvTeamRoster.Columns["StartingPenalty"].HeaderText = "PE";
            dgvTeamRoster.Columns["StartingPenalty"].Width = 50;
            dgvTeamRoster.Columns["StartingFaceoffs"].HeaderText = "FA";
            dgvTeamRoster.Columns["StartingFaceoffs"].Width = 50;
            dgvTeamRoster.Columns["StartingLeadership"].HeaderText = "LE";
            dgvTeamRoster.Columns["StartingLeadership"].Width = 50;
            dgvTeamRoster.Columns["StartingAttributeStrength"].HeaderText = "SR";
            dgvTeamRoster.Columns["StartingAttributeStrength"].Width = 50;
            dgvTeamRoster.Columns["Potential"].HeaderText = "POT";
            dgvTeamRoster.Columns["Potential"].Width = 50;
            dgvTeamRoster.Columns["Constance"].HeaderText = "CON";
            dgvTeamRoster.Columns["Constance"].Width = 50;
            dgvTeamRoster.Columns["StartingOffense"].HeaderText = "OFF";
            dgvTeamRoster.Columns["StartingOffense"].Width = 50;
            dgvTeamRoster.Columns["StartingDefense"].HeaderText = "DEF";
            dgvTeamRoster.Columns["StartingDefense"].Width = 50;
            dgvTeamRoster.Columns["StartingOverall"].HeaderText = "OA";
            dgvTeamRoster.Columns["StartingOverall"].Width = 50;
            dgvTeamRoster.Columns["Greed"].Width = 55;

            // Set background color for Shooting, Playmaking, Stickhandling and OFF columns to light red
            dgvTeamRoster.Columns["StartingShooting"].DefaultCellStyle.BackColor = Color.LightSalmon;
            dgvTeamRoster.Columns["StartingPlaymaking"].DefaultCellStyle.BackColor = Color.LightSalmon;
            dgvTeamRoster.Columns["StartingStickhandling"].DefaultCellStyle.BackColor = Color.LightSalmon;
            dgvTeamRoster.Columns["StartingOffense"].DefaultCellStyle.BackColor = Color.LightSalmon;

            // Set background color for Shooting, Playmaking, Stickhandling and OFF column headers to light red
            dgvTeamRoster.Columns["StartingShooting"].HeaderCell.Style.BackColor = Color.LightSalmon;
            dgvTeamRoster.Columns["StartingPlaymaking"].HeaderCell.Style.BackColor = Color.LightSalmon;
            dgvTeamRoster.Columns["StartingStickhandling"].HeaderCell.Style.BackColor = Color.LightSalmon;
            dgvTeamRoster.Columns["StartingOffense"].HeaderCell.Style.BackColor = Color.LightSalmon;

            // Set background color for Checking, Positioning, Hitting and DEF columns to light green
            dgvTeamRoster.Columns["StartingChecking"].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvTeamRoster.Columns["StartingPositioning"].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvTeamRoster.Columns["StartingHitting"].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvTeamRoster.Columns["StartingDefense"].DefaultCellStyle.BackColor = Color.LightGreen;

            // Set background color for Checking, Positioning, Hitting and DEF column headers to light green
            dgvTeamRoster.Columns["StartingChecking"].HeaderCell.Style.BackColor = Color.LightGreen;
            dgvTeamRoster.Columns["StartingPositioning"].HeaderCell.Style.BackColor = Color.LightGreen;
            dgvTeamRoster.Columns["StartingHitting"].HeaderCell.Style.BackColor = Color.LightGreen;
            dgvTeamRoster.Columns["StartingDefense"].HeaderCell.Style.BackColor = Color.LightGreen;

            // Set background color for POT CON GREED OA
            dgvTeamRoster.Columns["Potential"].DefaultCellStyle.BackColor = Color.LightBlue;
            dgvTeamRoster.Columns["Constance"].DefaultCellStyle.BackColor = Color.Thistle;
            dgvTeamRoster.Columns["Greed"].DefaultCellStyle.BackColor = Color.Lavender;
            dgvTeamRoster.Columns["StartingOverall"].DefaultCellStyle.BackColor = Color.LightBlue;
            dgvTeamRoster.Columns["Potential"].HeaderCell.Style.BackColor = Color.LightBlue;
            dgvTeamRoster.Columns["Constance"].HeaderCell.Style.BackColor = Color.Thistle;
            dgvTeamRoster.Columns["Greed"].HeaderCell.Style.BackColor = Color.Lavender;
            dgvTeamRoster.Columns["StartingOverall"].HeaderCell.Style.BackColor = Color.LightBlue;

            // Hide some columns
            dgvTeamRoster.Columns["TeamNumber"].Visible = false;
            dgvTeamRoster.Columns["PlayerCountry"].Visible = false;
            dgvTeamRoster.Columns["Rank"].Visible = false;
            dgvTeamRoster.Columns["Handedness"].Visible = false;
            dgvTeamRoster.Columns["Fighting"].Visible = false;
            dgvTeamRoster.Columns["Shooting"].Visible = false;
            dgvTeamRoster.Columns["Playmaking"].Visible = false;
            dgvTeamRoster.Columns["Stickhandling"].Visible = false;
            dgvTeamRoster.Columns["Checking"].Visible = false;
            dgvTeamRoster.Columns["Positioning"].Visible = false;
            dgvTeamRoster.Columns["Hitting"].Visible = false;
            dgvTeamRoster.Columns["Skating"].Visible = false;
            dgvTeamRoster.Columns["Endurance"].Visible = false;
            dgvTeamRoster.Columns["Penalty"].Visible = false;
            dgvTeamRoster.Columns["Faceoffs"].Visible = false;
            dgvTeamRoster.Columns["Leadership"].Visible = false;
            dgvTeamRoster.Columns["AttributeStrength"].Visible = false;
            dgvTeamRoster.Columns["Offense"].Visible = false;
            dgvTeamRoster.Columns["Defense"].Visible = false;
            dgvTeamRoster.Columns["Overall"].Visible = false;

            foreach (DataGridViewColumn col in dgvTeamRoster.Columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (col.Name.StartsWith("Ceiling") || col.Name.StartsWith("Target") || col.Name.StartsWith("Birth"))
                {
                    col.Visible = false;
                }
            }

            // Move Potential, Constance, and Greed columns
            dgvTeamRoster.Columns["Potential"].DisplayIndex = 11;
            dgvTeamRoster.Columns["Constance"].DisplayIndex = 12;
            dgvTeamRoster.Columns["Greed"].DisplayIndex = 13;

            #endregion
        }
    }
}
