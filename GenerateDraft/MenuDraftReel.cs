using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EHMAssistant
{
    public partial class MenuDraftReel : Form
    {
        private GenerateDraft _generateDraft;

        public DataGridView dgvDraftReel;

        // List of countries for the dropdown
        private readonly List<string> countries = new List<string>
        {
            "Canada", "USA", "Suède", "Russie", "Finlande",
            "République tchèque", "Slovaquie", "Suisse",
            "Allemagne", "Danemark", "Lettonie", "Belarus", "Slovénie"
        };

        // List of positions for the dropdown
        private readonly List<string> positions = new List<string>
        {
            "AG / AD", "AD / AG", "C / AG", "C / AD", "D", "G"
        };

        // List of heights for the dropdown
        private readonly List<string> heights = new List<string>
        {
            "5'6", "5'7", "5'8", "5'9", "5'10", "5'11", "6'",
            "6'1", "6'2", "6'3", "6'4", "6'5", "6'6", "6'7"
        };

        // Forward player types
        private readonly List<string> forwardTypes = new List<string>
        {
            "Marqueur naturel", "Fabricant de Jeu", "Attaquant Offensif",
            "Attaquant Polyvalent", "Joueur de Caractère"
        };

        // Defender player types
        private readonly List<string> defenderTypes = new List<string>
        {
            "Défenseur Offensif", "Défenseur Défensif"
        };

        // Goalie player type
        private readonly string goalieType = "Gardien";

        // Additional type for tall players
        private readonly string powerForwardType = "Attaquant de Puissance";
        private readonly string physicalDefenderType = "Défenseur Physique";

        public MenuDraftReel(GenerateDraft generateDraft)
        {
            InitializeComponent();
            _generateDraft = generateDraft;
            SetupForm();
        }

        private void SetupForm()
        {
            // Increase the form size to accommodate all rows
            this.ClientSize = new Size(800, 700); // Wider and taller form

            // Create the button and set its position in the center
            Button btnGenerate = new Button
            {
                Text = "Générer",
                Size = new Size(100, 30)
            };

            // Dynamically center the button based on form width
            btnGenerate.Location = new Point((this.ClientSize.Width - btnGenerate.Width) / 2, 10);
            this.Controls.Add(btnGenerate);

            // Create the DataGridView with increased height
            dgvDraftReel = new DataGridView
            {
                Name = "dgvDraftReel",
                Location = new Point(10, 50),
                Size = new Size(760, 600), // Adjust height to fit more rows
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false, // Disable column resizing
                AllowUserToResizeRows = false     // Disable row resizing
            };
            this.Controls.Add(dgvDraftReel);

            // Add columns to the DataGridView
            dgvDraftReel.Columns.Add("Rank", "Rang");
            dgvDraftReel.Columns["Rank"].ReadOnly = true; // Make Rank column read-only
            dgvDraftReel.Columns.Add("Name", "Nom");

            // Create the Country column with dropdown
            DataGridViewComboBoxColumn countryColumn = new DataGridViewComboBoxColumn
            {
                Name = "Country",
                HeaderText = "Pays",
                DataPropertyName = "Country"
            };
            countryColumn.Items.AddRange(countries.ToArray());
            dgvDraftReel.Columns.Add(countryColumn);

            // Create the Position column with dropdown
            DataGridViewComboBoxColumn positionColumn = new DataGridViewComboBoxColumn
            {
                Name = "Position",
                HeaderText = "Position",
                DataPropertyName = "Position"
            };
            positionColumn.Items.AddRange(positions.ToArray());
            dgvDraftReel.Columns.Add(positionColumn);

            // Create the Height column with dropdown
            DataGridViewComboBoxColumn heightColumn = new DataGridViewComboBoxColumn
            {
                Name = "Height",
                HeaderText = "Taille",
                DataPropertyName = "Height"
            };
            heightColumn.Items.AddRange(heights.ToArray());
            dgvDraftReel.Columns.Add(heightColumn);

            // Create the PlayerType column with dropdown
            DataGridViewComboBoxColumn playerTypeColumn = new DataGridViewComboBoxColumn
            {
                Name = "PlayerType",
                HeaderText = "Type de joueur",
                DataPropertyName = "PlayerType"
            };
            dgvDraftReel.Columns.Add(playerTypeColumn);

            // Center the text in all cells
            foreach (DataGridViewColumn column in dgvDraftReel.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.Width = 125;
            }
            dgvDraftReel.Columns["Rank"].Width = 55;
            dgvDraftReel.Columns["Name"].Width = 185;

            // Center the text in column headers
            dgvDraftReel.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Add 60 rows to the DataGridView
            for (int i = 1; i <= 60; i++)
            {
                dgvDraftReel.Rows.Add(i, "", null, null, null, null);
            }

            // Additional event handlers
            dgvDraftReel.CellValueChanged += DgvDraftReel_CellValueChanged;

            // Handle form resizing to keep button centered
            this.Resize += (s, e) => btnGenerate.Location = new Point((this.ClientSize.Width - btnGenerate.Width) / 2, 10);

            // Handle the cell click to open dropdown and enter edit mode
            dgvDraftReel.CellClick += (sender, e) =>
            {
                DataGridView dgv = sender as DataGridView;
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Check if it's a combobox column
                    var column = dgv.Columns[e.ColumnIndex];
                    if (column is DataGridViewComboBoxColumn)
                    {
                        // Select the new cell
                        dgv.CurrentCell = dgv[e.ColumnIndex, e.RowIndex];

                        // Enter edit mode
                        dgv.BeginEdit(true);

                        // Open the dropdown using the editing control
                        var editingControl = dgv.EditingControl as ComboBox;
                        if (editingControl != null)
                        {
                            editingControl.DroppedDown = true;
                        }
                    }
                }
            };

            btnGenerate.Click += BtnGenerate_Click;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            List<PlayerSpecification> playerSpecs = new List<PlayerSpecification>();

            // Extract player specifications from the DataGridView
            foreach (DataGridViewRow row in dgvDraftReel.Rows)
            {
                int rank = Convert.ToInt32(row.Cells["Rank"].Value);
                string name = row.Cells["Name"].Value?.ToString() ?? "";
                string country = row.Cells["Country"].Value?.ToString();
                string position = row.Cells["Position"].Value?.ToString();
                string height = row.Cells["Height"].Value?.ToString();
                string playerType = row.Cells["PlayerType"].Value?.ToString();

                // Create a specification object to pass to the generator
                var spec = new PlayerSpecification
                {
                    Rank = rank,
                    Name = name,
                    Country = string.IsNullOrEmpty(country) ? null : country,
                    Position = string.IsNullOrEmpty(position) ? null : position,
                    Height = string.IsNullOrEmpty(height) ? null : height,
                    PlayerType = string.IsNullOrEmpty(playerType) ? null : playerType
                };

                playerSpecs.Add(spec);
            }

            // Generate players with the specifications
            _generateDraft.GeneratePlayersWithSpecs(playerSpecs);

            // Show the GenerateDraft form after generation
            _generateDraft.Show();
            this.Hide();
        }

        // Add a new class to store player specifications
        public class PlayerSpecification
        {
            public int Rank { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string Position { get; set; }
            public string Height { get; set; }
            public string PlayerType { get; set; }
        }

        private void DgvDraftReel_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = (DataGridView)sender;

            // Check if the changed cell is in the Position or Height column
            if (e.ColumnIndex == dgv.Columns["Position"].Index ||
                e.ColumnIndex == dgv.Columns["Height"].Index)
            {
                UpdatePlayerTypeOptions(dgv, e.RowIndex);
            }
        }

        private void UpdatePlayerTypeOptions(DataGridView dgv, int rowIndex)
        {
            // Get the current position and height
            string position = dgv.Rows[rowIndex].Cells["Position"].Value?.ToString();
            string height = dgv.Rows[rowIndex].Cells["Height"].Value?.ToString();

            // Get the player type cell
            DataGridViewComboBoxCell playerTypeCell =
                (DataGridViewComboBoxCell)dgv.Rows[rowIndex].Cells["PlayerType"];

            // Clear existing items
            playerTypeCell.Items.Clear();

            // Skip if position is not set
            if (string.IsNullOrEmpty(position))
                return;

            // Determine if player is tall (6'2" or taller)
            bool isTall = !string.IsNullOrEmpty(height) &&
                          (height == "6'2" || height == "6'3" || height == "6'4" ||
                           height == "6'5" || height == "6'6" || height == "6'7");

            // If a player type was previously selected but is now invalid due to height change,
            // we need to clear the selection
            string currentPlayerType = dgv.Rows[rowIndex].Cells["PlayerType"].Value?.ToString();
            bool needsReset = false;

            // Check if current type is invalid for non-tall players
            if (!isTall &&
                (currentPlayerType == powerForwardType || currentPlayerType == physicalDefenderType))
            {
                needsReset = true;
            }

            // Add options based on position
            if (position == "G")
            {
                // For goalies, automatically set to "Gardien"
                playerTypeCell.Items.Add(goalieType);
                dgv.Rows[rowIndex].Cells["PlayerType"].Value = goalieType;
            }
            else if (position == "D")
            {
                // For defenders
                foreach (string type in defenderTypes)
                {
                    playerTypeCell.Items.Add(type);
                }

                // Add physical defender option for tall players only
                if (isTall)
                {
                    playerTypeCell.Items.Add(physicalDefenderType);
                }

                // Reset if necessary
                if (needsReset)
                {
                    dgv.Rows[rowIndex].Cells["PlayerType"].Value = null;
                }
            }
            else
            {
                // For forwards (AG / AD, AD / AG, C / AG, C / AD)
                foreach (string type in forwardTypes)
                {
                    playerTypeCell.Items.Add(type);
                }

                // Add power forward option for tall players only
                if (isTall)
                {
                    playerTypeCell.Items.Add(powerForwardType);
                }

                // Reset if necessary
                if (needsReset)
                {
                    dgv.Rows[rowIndex].Cells["PlayerType"].Value = null;
                }
            }
        }

        private void MenuDraftReel_Load(object sender, EventArgs e)
        {
            // Form load logic if needed
        }
    }
}
