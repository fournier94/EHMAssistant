using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EHMAssistant
{
    public partial class PlayerViewerForm : Form
    {
        private PlayerReader playerReader;

        public PlayerViewerForm()
        {
            InitializeComponent();
            playerReader = new PlayerReader();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Read the file and get player count
                    int playerCount = playerReader.ReadEHMFile(openFileDialog.FileName);

                    // Display players in the DataGridView
                    playerReader.DisplayPlayersInDataGridView(dgvPlayers);

                    // Update status label
                    lblStatus.Text = $"Loaded {playerCount} players from file";

                    // Enable export button if needed
                    // btnExport.Enabled = playerCount > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Error loading file";
                }
            }
        }
    }
}