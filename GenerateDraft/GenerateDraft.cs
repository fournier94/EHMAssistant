using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace EHMAssistant
{
    public partial class GenerateDraft : Form
    {
        #region Scripts
        private BaseAttributesGenerator _baseAttributesGenerator;
        private PlayerTargetAttributes _playerTargetAttributes;
        private PositionStrengthGenerator _strengthGen;
        #endregion

        #region Player list
        private List<Player> players;
        #endregion

        #region UI
        private Button b_GenerateDraft;
        private Button btnOpenDraftReel;
        private TextBox txtDraftYear;
        private TabControl mainTabControl;

        private TabPage overviewTabPage;

        private TabPage finalStatsTabPage;
        private DataGridView finalStatsGridView;

        private TabPage draftListTabPage;
        private DataGridView playersGridView;
        private Panel playerListPanel;
        private RichTextBox draftListBox;
        private Button copyDraftListButton;
        #endregion

        #region Variables | Sorting columns 
        private bool sortedOnce = false; // Used to reset sorting after generating a new draft
        private bool ascendingSort = true;  // Track sort direction
        private string currentSortColumn = "";  // Track which column we're sorting by
        #endregion
        
        #region Constructor
        public GenerateDraft()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;  // This centers the form
            this.MinimumSize = new Size(800, 600);  // Keep your minimum size

            // Initialize objects
            players = new List<Player>();
            _baseAttributesGenerator = new BaseAttributesGenerator();
            _playerTargetAttributes = new PlayerTargetAttributes();
            _strengthGen = new PositionStrengthGenerator();

            // Initialize UI
            InitializeTabControl();
            InitializePlayerListView();
            InitializeFinalStatsGridView();
            InitializeDraftListView();
        }
        #endregion

        #region OnClick Generate draft
        private void b_GenerateDraft_Click(object sender, EventArgs e)
        {
            // Validate draft year input
            if (!Regex.IsMatch(txtDraftYear.Text, @"^\d{4}$"))
            {
                MessageBox.Show("Vous devez entrer l'année du repêchage à 4 chiffres.", "Erreur de saisie",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sortedOnce = false;
            GeneratePlayers();  // Generate new players
            RefreshPlayerList();  // Refresh UI after generation
            RefreshHeaderAlignment();
        }

        private void RefreshHeaderAlignment()
        {
            foreach (DataGridViewColumn column in finalStatsGridView.Columns)
            {
                int headerWidth = column.Width;
                int leftPadding;

                // Special handling for the "Nom" header
                if (column.HeaderText == "Nom")
                {
                    // Calculate padding specifically for "Nom"
                    leftPadding = 30;
                }
                else if (column.HeaderText == "CON" || column.HeaderText == "POT")
                {
                    // Calculate padding specifically for "CON" and "POT"
                    leftPadding = 0;
                }
                else
                {
                    // Default calculation for other headers
                    leftPadding = (int)((headerWidth - column.HeaderCell.Style.Font.Size * column.HeaderText.Length) / 2);
                }

                // Apply the left padding to the header text (if needed)
                column.HeaderCell.Style.Padding = new Padding(leftPadding, 0, 0, 0);
            }
        }
        #endregion

        #region Tab Control
        private void InitializeTabControl()
        {
            // Create container panel for button + tab control
            Panel tabControlContainer = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Create a panel for the buttons
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.LightGray
            };

            // Create the label for the input field with bold text and size 10
            Label lblDraftYear = new Label
            {
                Text = "Année de repêchage",
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            // Create the input field to the left of Draft Reel button
            txtDraftYear = new TextBox
            {
                Size = new Size(150, 25),
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Position the text box vertically centered
            txtDraftYear.Location = new Point(20, ((buttonPanel.Height - txtDraftYear.Height) / 2) + 5);

            // Position the label above the text box, slightly to the right and lower than before
            lblDraftYear.Location = new Point(txtDraftYear.Left - 2, txtDraftYear.Top - lblDraftYear.Height);

            // Create the Draft Reel button - positioned to the right of the text box
            btnOpenDraftReel = new Button
            {
                Text = "Draft réel",
                Size = new Size(180, 50),
                BackColor = Color.White
            };
            btnOpenDraftReel.Click += (sender, e) => OpenDraftReel();
            // Position to the right of the text box with some spacing
            btnOpenDraftReel.Location = new Point(txtDraftYear.Right + 10, (buttonPanel.Height - btnOpenDraftReel.Height) / 2);

            // Create the Generate Draft button - centered
            b_GenerateDraft = new Button
            {
                Text = "Générer un nouveau repêchage",
                Size = new Size(250, 50),
                BackColor = Color.White,
                Anchor = AnchorStyles.None
            };
            b_GenerateDraft.Click += b_GenerateDraft_Click;

            // Center the generate button
            b_GenerateDraft.Location = new Point(
                (buttonPanel.Width - b_GenerateDraft.Width) / 2,
                (buttonPanel.Height - b_GenerateDraft.Height) / 2
            );

            // Create the Update Players button - positioned on far right
            Button btnUpdatePlayers = new Button
            {
                Text = "Sauvegarder dans le fichier de jeu",
                Size = new Size(180, 50),
                BackColor = Color.White,
                Anchor = AnchorStyles.Right // This will anchor it to the right of the form
            };
            btnUpdatePlayers.Click += btnUpdatePlayers_Click;
            // Position on far right with some padding
            btnUpdatePlayers.Location = new Point(
                buttonPanel.Width - btnUpdatePlayers.Width - 20, // 20 pixels from right edge
                (buttonPanel.Height - btnUpdatePlayers.Height) / 2
            );

            // Add controls to the panel
            buttonPanel.Controls.Add(lblDraftYear);
            buttonPanel.Controls.Add(txtDraftYear);
            buttonPanel.Controls.Add(btnOpenDraftReel);
            buttonPanel.Controls.Add(b_GenerateDraft);
            buttonPanel.Controls.Add(btnUpdatePlayers);

            // Initialize TabControl
            mainTabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(12, 4),
                Visible = true // Ensure it is visible
            };

            // Add tabs
            overviewTabPage = new TabPage("Vue d'ensemble");
            finalStatsTabPage = new TabPage("Stats finales des joueurs");
            draftListTabPage = new TabPage("Liste de repêchage");
            mainTabControl.TabPages.Add(overviewTabPage);
            mainTabControl.TabPages.Add(finalStatsTabPage);
            mainTabControl.TabPages.Add(draftListTabPage);

            // Add to container
            tabControlContainer.Controls.Add(mainTabControl);
            tabControlContainer.Controls.Add(buttonPanel);

            // Ensure TabControl is not hidden
            buttonPanel.BringToFront();
            mainTabControl.BringToFront();

            // Add to form
            this.Controls.Add(tabControlContainer);

            // Handle form resize events to keep the buttons positioned correctly
            this.Resize += (sender, e) =>
            {
                // Recalculate positions for the buttons on resize
                int buttonPanelWidth = buttonPanel.Width;

                // Center the Generate Draft button
                b_GenerateDraft.Location = new Point(
                    (buttonPanelWidth - b_GenerateDraft.Width) / 2,
                    (buttonPanel.Height - b_GenerateDraft.Height) / 2
                );

                // Position the Update Players button on the far right
                btnUpdatePlayers.Location = new Point(
                    buttonPanel.Width - btnUpdatePlayers.Width - 20,
                    (buttonPanel.Height - btnUpdatePlayers.Height) / 2
                );

                // Make sure the text box and label stay correctly positioned
                txtDraftYear.Location = new Point(20, (buttonPanel.Height - txtDraftYear.Height) / 2);
                lblDraftYear.Location = new Point(txtDraftYear.Left + 5, txtDraftYear.Top - lblDraftYear.Height - 1);

                // Position the Draft Reel button to the right of the text box
                btnOpenDraftReel.Location = new Point(txtDraftYear.Right + 10, (buttonPanel.Height - btnOpenDraftReel.Height) / 2);
            };

            // Force UI refresh
            mainTabControl.Invalidate();
            mainTabControl.Update();
        }

        // Add a handler method for the Update Players button
        private void btnUpdatePlayers_Click(object sender, EventArgs e)
        {
            // Only proceed if we have generated players
            if (players == null || players.Count == 0)
            {
                MessageBox.Show("Veuillez d'abord générer des joueurs.", "Aucun joueur disponible",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open file dialog to select EHM file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "EHM Files (*.ehm)|*.ehm|All Files (*.*)|*.*";
                openFileDialog.Title = "Sélectionnez un fichier EHM";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;

                        // Create processor instance and process the file
                        ModifyPlayers processor = new ModifyPlayers(players);
                        int eligibleCount = processor.ProcessEHMFile(filePath);

                        if (eligibleCount == 0)
                        {
                            MessageBox.Show("Aucun joueur éligible trouvé dans le fichier.",
                                "Aucun joueur trouvé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Directly ask for confirmation without showing the number of eligible players
                        DialogResult result = MessageBox.Show(
                            "Voulez-vous mettre à jour les joueurs éligibles?",
                            "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            processor.UpdatePlayerStats();
                            MessageBox.Show("Mise à jour des joueurs terminée avec succès!",
                                "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur: {ex.Message}", "Erreur",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Method to open the MenuDraftReel form
        private void OpenDraftReel()
        {
            // Validate draft year input
            if (!Regex.IsMatch(txtDraftYear.Text, @"^\d{4}$"))
            {
                MessageBox.Show("Vous devez entrer l'année du repêchage à 4 chiffres.", "Erreur de saisie",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MenuDraftReel draftReelForm = new MenuDraftReel(this);

            // Set the window to appear in the center of the screen
            draftReelForm.StartPosition = FormStartPosition.CenterScreen;

            // Hide the main form and show the draft reel
            this.Hide();
            draftReelForm.FormClosed += (s, e) => this.Show(); // Restore main form when closed
            draftReelForm.Show();
        }
        #endregion

        #region Tab Player list overview
        private void InitializePlayerListView()
        {
            // Initialize the Panel
            playerListPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // Initialize the DataGridView (your existing DataGridView initialization code)
            playersGridView = new DataGridView();
            playersGridView.Dock = DockStyle.Fill;
            playersGridView.AutoGenerateColumns = false;  // This prevents showing all properties
            playersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            playersGridView.AllowUserToAddRows = false;
            playersGridView.AllowUserToDeleteRows = false;
            playersGridView.ReadOnly = true;
            playersGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            playersGridView.MultiSelect = false;
            playersGridView.RowHeadersVisible = false;
            playersGridView.BackgroundColor = Color.White;
            playersGridView.BorderStyle = BorderStyle.None;
            playersGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Center-align the header text
            playersGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            playersGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Optional: Adjust header font

            // Center-align the cell text
            playersGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            playersGridView.ColumnHeaderMouseClick += PlayersGridView_ColumnHeaderMouseClick;

            // Define only the requested columns
            playersGridView.Columns.AddRange(new DataGridViewColumn[]
            {
        new DataGridViewTextBoxColumn
        {
            Name = "Rank",
            DataPropertyName = "Rank",
            HeaderText = "Rang",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Name",
            DataPropertyName = "Name",
            HeaderText = "Nom",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Country",
            DataPropertyName = "PlayerCountry",
            HeaderText = "Pays",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Position",
            DataPropertyName = "PlayerPosition",
            HeaderText = "Position",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Height",
            DataPropertyName = "Height",
            HeaderText = "Taille",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Type",
            DataPropertyName = "PlayerType",
            HeaderText = "Type du joueur",
            Width = 100,
            CellTemplate = new DataGridViewTextBoxCell()
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Strength",
            DataPropertyName = "PositionStrength",
            HeaderText = "Force du joueur",
            Width = 120
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Offense",
            DataPropertyName = "Offense",
            HeaderText = "Offense",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Defense",
            DataPropertyName = "Defense",
            HeaderText = "Defense",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        },
        new DataGridViewTextBoxColumn
        {
            Name = "Overall",
            DataPropertyName = "Overall",
            HeaderText = "Overall",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        }
            });

            // Add the DataGridView to the Panel
            playerListPanel.Controls.Add(playersGridView);

            // Add the Panel to the players TabPage instead of the form
            overviewTabPage.Controls.Add(playerListPanel);

            // Optional: Set minimum form size to prevent too much squishing
            this.MinimumSize = new Size(800, 600);

            // Bind the data
            BindPlayerData();
        }

        private void BindPlayerData()
        {
            var sortedPlayers = players.OrderBy(p => p.Rank).ToList();

            // Create a list of anonymous objects to bind to the grid, replacing the enum with the display string
            var displayPlayers = sortedPlayers.Select(player => new
            {
                player.Rank,
                player.Name,
                PlayerCountry = GetCountryString(player.PlayerCountry),
                PlayerPosition = GetPositionConcatenatedString(player.PlayerPosition, player),
                player.Height,
                PlayerType = GetPlayerTypeString(player.PlayerType),
                PositionStrength = GetStrengthString(player.PositionStrength),
                player.Offense,
                player.Defense,
                player.Overall
            }).ToList();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = displayPlayers;
            playersGridView.DataSource = bindingSource;
        }

        private void PlayersGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var clickedColumn = playersGridView.Columns[e.ColumnIndex];

            // Handle sorting for Rank, Country, Name, Height, and Position
            if (clickedColumn.Name == "Rank" || clickedColumn.Name == "Country" || clickedColumn.Name == "Name" ||
                clickedColumn.Name == "Height" || clickedColumn.Name == "Position" || clickedColumn.Name == "Type"
                || clickedColumn.Name == "Strength" || clickedColumn.Name == "Offense" || clickedColumn.Name == "Defense"
                || clickedColumn.Name == "Overall")
            {
                // If clicking the same column, toggle direction. If different column, default to ascending
                if (clickedColumn.Name == currentSortColumn && sortedOnce)
                {
                    ascendingSort = !ascendingSort;
                }
                else
                {
                    sortedOnce = true;
                    currentSortColumn = clickedColumn.Name;
                    ascendingSort = true;
                }

                var bindingSource = (BindingSource)playersGridView.DataSource;
                var data = (IEnumerable<dynamic>)bindingSource.DataSource;

                // Sort based on the column
                List<dynamic> sortedData;
                if (currentSortColumn == "Rank")
                {
                    sortedData = ascendingSort
                        ? data.OrderBy(x => x.Rank).ToList()
                        : data.OrderByDescending(x => x.Rank).ToList();
                }
                else if (currentSortColumn == "Country")
                {
                    sortedData = ascendingSort
                        ? data.OrderBy(x => x.PlayerCountry).ToList()
                        : data.OrderByDescending(x => x.PlayerCountry).ToList();
                }
                else if (currentSortColumn == "Name")
                {
                    sortedData = ascendingSort
                        ? data.OrderBy(x => x.Name).ToList()
                        : data.OrderByDescending(x => x.Name).ToList();
                }
                else if (currentSortColumn == "Height")
                {
                    // Convert height to total inches before sorting
                    sortedData = ascendingSort
                        ? data.OrderBy(x => ConvertHeightToInches(x.Height)).ToList()
                        : data.OrderByDescending(x => ConvertHeightToInches(x.Height)).ToList();
                }
                else if (currentSortColumn == "Position")
                {
                    // Sort positions using the defined order
                    sortedData = ascendingSort
                        ? data.OrderBy(x => GetPositionSortValue(x.PlayerPosition)).ToList()
                        : data.OrderByDescending(x => GetPositionSortValue(x.PlayerPosition)).ToList();
                }
                else if (currentSortColumn == "Type")
                {
                    // Sort positions using the defined order
                    sortedData = ascendingSort
                        ? data.OrderBy(x => GetTypeSortValue(x.PlayerType)).ToList()
                        : data.OrderByDescending(x => GetTypeSortValue(x.PlayerType)).ToList();
                }
                else if (currentSortColumn == "Strength")
                {
                    // Sort positions using the defined order
                    sortedData = ascendingSort
                        ? data.OrderBy(x => GetPositionStrengthSortValue(x.PositionStrength)).ToList()
                        : data.OrderByDescending(x => GetPositionStrengthSortValue(x.PositionStrength)).ToList();
                }
                else if (currentSortColumn == "Offense")
                {
                    sortedData = ascendingSort
                        ? data.OrderByDescending(x => x.Offense).ToList()
                        : data.OrderBy(x => x.Offense).ToList();
                }
                else if (currentSortColumn == "Defense")
                {
                    sortedData = ascendingSort
                        ? data.OrderByDescending(x => x.Defense).ToList()
                        : data.OrderBy(x => x.Defense).ToList();
                }
                else if (currentSortColumn == "Overall")
                {
                    sortedData = ascendingSort
                        ? data.OrderByDescending(x => x.Overall).ToList()
                        : data.OrderBy(x => x.Overall).ToList();
                }
                else
                {
                    sortedData = data.ToList();
                }

                // Update the binding source
                bindingSource.DataSource = sortedData;

                // Clear all sort glyphs
                foreach (DataGridViewColumn column in playersGridView.Columns)
                {
                    column.HeaderCell.SortGlyphDirection = SortOrder.None;
                }

                // Set the sort glyph for the current column
                clickedColumn.HeaderCell.SortGlyphDirection =
                    ascendingSort ? SortOrder.Ascending : SortOrder.Descending;
            }
        }

        // Helper function to get sorting order for positions
        private int GetPositionSortValue(string position)
        {
            if (position == "C / AD") return 1;
            if (position == "C / AG") return 2;
            if (position == "AD / AG") return 3;
            if (position == "AG / AD") return 4;
            if (position == "D") return 5;
            if (position == "G") return 6;
            return 7;
        }

        // Helper function to convert height string to total inches
        private int ConvertHeightToInches(string height)
        {
            // Height format is expected to be like "5'9\""
            var match = Regex.Match(height, @"(\d+)'(\d+)""");
            if (match.Success)
            {
                int feet = int.Parse(match.Groups[1].Value);
                int inches = int.Parse(match.Groups[2].Value);
                return (feet * 12) + inches; // Convert feet to inches and add remaining inches
            }

            return 0; // Default to 0 if parsing fails
        }

        // Helper function to get sorting order for types
        private int GetTypeSortValue(string type)
        {
            if (type == "Marqueur naturel") return 1;
            if (type == "Fabricant de Jeu") return 2;
            if (type == "Attaquant Offensif") return 3;
            if (type == "Attaquant de Puissance") return 4;
            if (type == "Attaquant Polyvalent") return 5;
            if (type == "Joueur de Caractère") return 6;
            if (type == "Défenseur Offensif") return 7;
            if (type == "Défenseur Défensif") return 8;
            if (type == "Défenseur Physique") return 9;
            if (type == "Gardien") return 10;
            return 11;
        }

        // Helper function to get sorting order for types
        private int GetPositionStrengthSortValue(string strenght)
        {
            if (strenght == "Générationnel") return 1;
            if (strenght == "Élite") return 2;
            if (strenght == "Première ligne") return 3;
            if (strenght == "Première paire") return 4;
            if (strenght == "Deuxième ligne") return 5;
            if (strenght == "Deuxième paire") return 6;
            if (strenght == "Troisième ligne") return 7;
            if (strenght == "Troisième paire") return 8;
            if (strenght == "Quatrième ligne") return 9;
            if (strenght == "AHL") return 10;
            if (strenght == "Gardien partant") return 11;
            if (strenght == "Gardien auxiliaire") return 12;
            return 13;
        }
        #endregion

        #region Tab stats finales
        private void InitializeFinalStatsGridView()
        {
            // Create panel for the final stats grid
            Panel finalStatsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // Initialize the DataGridView for player attributes
            finalStatsGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false, // Allow cell selection
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowTemplate = { Height = 25 }, // Adjust row height
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing,
                ColumnHeadersHeight = 35 // Increase header height
            };

            // Set default styles (these will be inherited by new columns)
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                Font = new Font("Arial", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                WrapMode = DataGridViewTriState.True
            };

            finalStatsGridView.ColumnHeadersDefaultCellStyle = headerStyle;
            finalStatsGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            finalStatsGridView.ColumnHeaderMouseClick += FinalStatsGridView_ColumnHeaderMouseClick;

            // Define column mappings
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
    {
        { "Rang", "Rank" },
        { "Nom", "Name" },
        { "FI", "Fighting" },
        { "SH", "Shooting" },
        { "PL", "Playmaking" },
        { "ST", "Stickhandling" },
        { "CH", "Checking" },
        { "PO", "Positioning" },
        { "HIT", "Hitting" },
        { "SK", "Skating" },
        { "EN", "Endurance" },
        { "PE", "Penalty" },
        { "FA", "Faceoffs" },
        { "LE", "Leadership" },
        { "SR", "AttributeStrength" },
        { "POT", "Potential" },
        { "CON", "Constance" },
        { "Greed", "Greed" },
        { "OFF", "Offense" },
        { "DEF", "Defense" },
        { "OA", "Overall" }
    };

            // Add columns and apply styles individually
            foreach (var mapping in columnMappings)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
                {
                    Name = mapping.Key,
                    HeaderText = mapping.Key,
                    DataPropertyName = mapping.Value,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };

                // Explicitly set the header cell style for each column
                column.HeaderCell.Style = new DataGridViewCellStyle
                {
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    WrapMode = DataGridViewTriState.True
                };

                if (mapping.Key == "Rang")
                {
                    column.Width = 50; // Set a fixed width for "Rang"
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // Disable AutoSize
                }
                if (mapping.Key == "POT")
                {
                    column.Width = 60; // Set a fixed width for "POT"
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // Disable AutoSize
                }
                if (mapping.Key == "CON")
                {
                    column.Width = 60; // Set a fixed width for "CON"
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // Disable AutoSize
                }
                if (mapping.Key == "Greed")
                {
                    column.Width = 60; // Set a fixed width for "Greed"
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // Disable AutoSize
                }

                finalStatsGridView.Columns.Add(column);
            }

            foreach (DataGridViewColumn column in finalStatsGridView.Columns)
            {
                if (column.Name == "OFF" || column.Name == "DEF" || column.Name == "OA" || column.Name == "Greed")
                {
                    column.ReadOnly = true;

                    // Optionally, you can style read-only cells differently
                    column.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }

            // Force a refresh of the headers
            finalStatsGridView.EnableHeadersVisualStyles = false;

            // Populate the grid with player final stats
            PopulateFinalStatsGrid();

            // Add the grid to the panel and the panel to the tab
            finalStatsPanel.Controls.Add(finalStatsGridView);
            finalStatsTabPage.Controls.Add(finalStatsPanel);
            finalStatsGridView.AllowUserToResizeColumns = false;
            finalStatsGridView.AllowUserToResizeRows = false;
            finalStatsGridView.CellValueChanged += FinalStatsGridView_CellValueChanged;
            finalStatsGridView.EditingControlShowing += FinalStatsGridView_EditingControlShowing;
            finalStatsGridView.CellValidating += FinalStatsGridView_CellValidating;
            finalStatsGridView.CellEndEdit += FinalStatsGridView_CellEndEdit;
        }

        private void PopulateFinalStatsGrid()
        {
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = players;
            finalStatsGridView.DataSource = bindingSource;
        }

        private void FinalStatsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure a valid cell was changed
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Get the Player object for the current row
            Player player = players[e.RowIndex];

            // Get the column name to determine which property to update
            string columnName = finalStatsGridView.Columns[e.ColumnIndex].Name;

            // Get the new value from the cell
            object newValue = finalStatsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            // Try to convert the value to the appropriate type if needed
            // For numeric values, you may need to parse them
            if (newValue != null)
            {
                try
                {
                    // Update the corresponding property based on the column name
                    switch (columnName)
                    {
                        case "Rang":
                            player.Rank = Convert.ToInt32(newValue);
                            break;
                        case "Nom":
                            player.Name = newValue.ToString();
                            break;
                        case "FI":
                            player.Fighting = Convert.ToInt32(newValue);
                            break;
                        case "SH":
                            player.Shooting = Convert.ToInt32(newValue);
                            break;
                        case "PL":
                            player.Playmaking = Convert.ToInt32(newValue);
                            break;
                        case "ST":
                            player.Stickhandling = Convert.ToInt32(newValue);
                            break;
                        case "CH":
                            player.Checking = Convert.ToInt32(newValue);
                            break;
                        case "PO":
                            player.Positioning = Convert.ToInt32(newValue);
                            break;
                        case "HIT":
                            player.Hitting = Convert.ToInt32(newValue);
                            break;
                        case "SK":
                            player.Skating = Convert.ToInt32(newValue);
                            break;
                        case "EN":
                            player.Endurance = Convert.ToInt32(newValue);
                            break;
                        case "PE":
                            player.Penalty = Convert.ToInt32(newValue);
                            break;
                        case "FA":
                            player.Faceoffs = Convert.ToInt32(newValue);
                            break;
                        case "LE":
                            player.Leadership = Convert.ToInt32(newValue);
                            break;
                        case "SR":
                            player.AttributeStrength = Convert.ToInt32(newValue);
                            break;
                        case "POT":
                            player.Potential = Convert.ToInt32(newValue);
                            break;
                        case "CON":
                            player.Constance = Convert.ToInt32(newValue);
                            break;
                    }

                    // Refresh the display to show updated calculated values
                    UpdateCalculatedValues(e.RowIndex);

                    // Refresh the row to update any derived values
                    finalStatsGridView.InvalidateRow(e.RowIndex);
                }
                catch (FormatException)
                {
                    // Handle invalid format (e.g., entering text in a numeric field)
                    MessageBox.Show("Veuillez entrer une valeur numérique valide.", "Erreur de saisie",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Revert the change
                    finalStatsGridView.CellValueChanged -= FinalStatsGridView_CellValueChanged;
                    finalStatsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = GetOriginalValue(player, columnName);
                    finalStatsGridView.CellValueChanged += FinalStatsGridView_CellValueChanged;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    MessageBox.Show($"Erreur: {ex.Message}", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Revert the change
                    finalStatsGridView.CellValueChanged -= FinalStatsGridView_CellValueChanged;
                    finalStatsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = GetOriginalValue(player, columnName);
                    finalStatsGridView.CellValueChanged += FinalStatsGridView_CellValueChanged;
                }
            }
        }

        // Helper method to get the original value from a player object based on column name
        private object GetOriginalValue(Player player, string columnName)
        {
            switch (columnName)
            {
                case "Rang": return player.Rank;
                case "Nom": return player.Name;
                case "FI": return player.Fighting;
                case "SH": return player.Shooting;
                case "PL": return player.Playmaking;
                case "ST": return player.Stickhandling;
                case "CH": return player.Checking;
                case "PO": return player.Positioning;
                case "HIT": return player.Hitting;
                case "SK": return player.Skating;
                case "EN": return player.Endurance;
                case "PE": return player.Penalty;
                case "FA": return player.Faceoffs;
                case "LE": return player.Leadership;
                case "SR": return player.AttributeStrength;
                case "POT": return player.Potential;
                case "CON": return player.Constance;
                case "Greed": return player.Greed;
                case "OFF": return player.Offense;
                case "DEF": return player.Defense;
                case "OA": return player.Overall;
                default: return null;
            }
        }

        private void UpdateCalculatedValues(int rowIndex)
        {
            // Temporarily remove the event handler to prevent infinite loops
            finalStatsGridView.CellValueChanged -= FinalStatsGridView_CellValueChanged;

            // Get the player
            Player player = players[rowIndex];

            // Update the calculated fields in the grid
            foreach (DataGridViewColumn column in finalStatsGridView.Columns)
            {
                if (column.Name == "OFF")
                    finalStatsGridView.Rows[rowIndex].Cells[column.Index].Value = player.Offense;
                else if (column.Name == "DEF")
                    finalStatsGridView.Rows[rowIndex].Cells[column.Index].Value = player.Defense;
                else if (column.Name == "OA")
                    finalStatsGridView.Rows[rowIndex].Cells[column.Index].Value = player.Overall;
            }

            // Reattach the event handler
            finalStatsGridView.CellValueChanged += FinalStatsGridView_CellValueChanged;
        }

        private void FinalStatsGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var clickedColumn = finalStatsGridView.Columns[e.ColumnIndex];
            var columnName = clickedColumn.Name;

            // Define which columns can be sorted
            var sortableColumns = new[] { "Rang", "Nom", "FI", "SH", "PL",
        "ST", "CH", "PO", "HIT", "SK",
        "EN", "PE", "FA", "LE", "SR",
        "POT", "CON", "OFF", "DEF", "OA" };

            if (sortableColumns.Contains(columnName))
            {
                // If clicking the same column, toggle direction. If different column, default to descending
                if (columnName == currentSortColumn && sortedOnce)
                {
                    ascendingSort = !ascendingSort;
                }
                else
                {
                    sortedOnce = true;
                    currentSortColumn = columnName;
                    ascendingSort = false;  // Start with descending sort (highest values first)
                }

                var bindingSource = (BindingSource)finalStatsGridView.DataSource;
                var data = (IEnumerable<dynamic>)bindingSource.DataSource;
                List<dynamic> sortedData;

                // Sort based on the column
                switch (currentSortColumn)
                {
                    case "Rang":
                        sortedData = !ascendingSort
                            ? data.OrderBy(x => x.Name).ToList()
                            : data.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "Nom":
                        sortedData = !ascendingSort
                            ? data.OrderBy(x => x.Name).ToList()
                            : data.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "FI":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Fighting).ToList()
                            : data.OrderByDescending(x => x.Fighting).ToList();
                        break;
                    case "SH":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Shooting).ToList()
                            : data.OrderByDescending(x => x.Shooting).ToList();
                        break;
                    case "PL":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Playmaking).ToList()
                            : data.OrderByDescending(x => x.Playmaking).ToList();
                        break;
                    case "ST":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Stickhandling).ToList()
                            : data.OrderByDescending(x => x.Stickhandling).ToList();
                        break;
                    case "CH":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Checking).ToList()
                            : data.OrderByDescending(x => x.Checking).ToList();
                        break;
                    case "PO":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Positioning).ToList()
                            : data.OrderByDescending(x => x.Positioning).ToList();
                        break;
                    case "HIT":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Hitting).ToList()
                            : data.OrderByDescending(x => x.Hitting).ToList();
                        break;
                    case "SK":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Skating).ToList()
                            : data.OrderByDescending(x => x.Skating).ToList();
                        break;
                    case "EN":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Endurance).ToList()
                            : data.OrderByDescending(x => x.Endurance).ToList();
                        break;
                    case "PE":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Penalty).ToList()
                            : data.OrderByDescending(x => x.Penalty).ToList();
                        break;
                    case "FA":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Faceoffs).ToList()
                            : data.OrderByDescending(x => x.Faceoffs).ToList();
                        break;
                    case "LE":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Leadership).ToList()
                            : data.OrderByDescending(x => x.Leadership).ToList();
                        break;
                    case "SR":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.AttributeStrength).ToList()
                            : data.OrderByDescending(x => x.AttributeStrength).ToList();
                        break;
                    case "POT":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Potential).ToList()
                            : data.OrderByDescending(x => x.Potential).ToList();
                        break;
                    case "CON":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Constance).ToList()
                            : data.OrderByDescending(x => x.Constance).ToList();
                        break;
                    case "OFF":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Offense).ToList()
                            : data.OrderByDescending(x => x.Offense).ToList();
                        break;
                    case "DEF":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Defense).ToList()
                            : data.OrderByDescending(x => x.Defense).ToList();
                        break;
                    case "OA":
                        sortedData = ascendingSort
                            ? data.OrderBy(x => x.Overall).ToList()
                            : data.OrderByDescending(x => x.Overall).ToList();
                        break;
                    default:
                        sortedData = data.ToList();
                        break;
                }

                // Update the binding source
                bindingSource.DataSource = sortedData;

                // Clear all sort glyphs
                foreach (DataGridViewColumn column in finalStatsGridView.Columns)
                {
                    column.HeaderCell.SortGlyphDirection = SortOrder.None;
                }

                // Set the sort glyph for the current column
                clickedColumn.HeaderCell.SortGlyphDirection =
                    ascendingSort ? SortOrder.Ascending : SortOrder.Descending;
            }
        }

        private void FinalStatsGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            string columnName = finalStatsGridView.Columns[finalStatsGridView.CurrentCell.ColumnIndex].Name;

            // Important: use DataGridViewTextBoxEditingControl for better control
            if (e.Control is TextBox textBox)
            {
                // Important: First remove existing handlers
                textBox.KeyPress -= NumericOnly_KeyPress;
                textBox.KeyPress -= NameOnly_KeyPress;
                textBox.TextChanged -= TextBox_TextChanged;

                // Store the column name as a Tag to use in the TextChanged handler
                textBox.Tag = columnName;

                if (columnName == "Nom")
                {
                    // For "Nom" field block numeric input
                    textBox.KeyPress += NameOnly_KeyPress;
                }
                else if (columnName != "Greed" && columnName != "OFF" && columnName != "DEF" && columnName != "OA")
                {
                    // For numeric fields block non-numeric input
                    textBox.KeyPress += NumericOnly_KeyPress;
                }

                // Add TextChanged handler to catch paste operations and other input methods
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        // Handle TextChanged to catch paste operations
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string columnName = textBox.Tag.ToString();
                string text = textBox.Text;
                int selectionStart = textBox.SelectionStart;

                if (columnName == "Nom")
                {
                    // Remove any digits from the "Nom" field
                    string filteredText = new string(text.Where(c => !char.IsDigit(c)).ToArray());

                    if (filteredText != text)
                    {
                        textBox.Text = filteredText;
                        textBox.SelectionStart = Math.Min(selectionStart, filteredText.Length);
                    }
                }
                else if (columnName != "Greed" && columnName != "OFF" && columnName != "DEF" && columnName != "OA")
                {
                    // Remove any non-digits from numeric fields
                    string filteredText = new string(text.Where(char.IsDigit).ToArray());

                    if (filteredText != text)
                    {
                        textBox.Text = filteredText;
                        textBox.SelectionStart = Math.Min(selectionStart, filteredText.Length);
                    }
                }
            }
        }

        private void FinalStatsGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = finalStatsGridView.Columns[e.ColumnIndex].Name;
            string value = e.FormattedValue.ToString();

            if (columnName == "Nom" && value.Any(char.IsDigit))
            {
                // Block digits in "Nom" field at validation level
                e.Cancel = true;
                finalStatsGridView.Rows[e.RowIndex].ErrorText = "Le nom ne peut pas contenir de chiffres.";
            }
            else if (columnName != "Nom" && columnName != "Greed" &&
                     columnName != "OFF" && columnName != "DEF" &&
                     columnName != "OA" && value.Any(c => !char.IsDigit(c)))
            {
                // Block non-digits in numeric fields at validation level
                e.Cancel = true;
                finalStatsGridView.Rows[e.RowIndex].ErrorText = "Ce champ n'accepte que des chiffres.";
            }
        }

        private void FinalStatsGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear error text when editing ends
            finalStatsGridView.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Block non-numeric input (allow digits, backspace, and control chars)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void NameOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Block numeric input
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Tab Copy draft to forum
        private void InitializeDraftListView()
        {
            // Create a container panel for the entire draft list view
            Panel containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };

            // Create a panel for the button that will be at the top
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60  // Give enough height for the button and padding
            };

            // Initialize the copy button
            copyDraftListButton = new Button
            {
                Text = "Copier la liste de repêchage",
                Size = new Size(250, 40),
                Anchor = AnchorStyles.None  // This allows centering
            };

            // Center the button in its panel
            copyDraftListButton.Location = new Point(
                (buttonPanel.ClientSize.Width - copyDraftListButton.Width) / 2,
                (buttonPanel.ClientSize.Height - copyDraftListButton.Height) / 2
            );

            copyDraftListButton.Click += CopyDraftListButton_Click;
            buttonPanel.Controls.Add(copyDraftListButton);

            // Initialize the RichTextBox for the draft list
            draftListBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                Font = new Font("Consolas", 10),
                BorderStyle = BorderStyle.None
            };

            // Create a panel to contain the RichTextBox
            Panel draftListPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // Add controls to their respective containers
            draftListPanel.Controls.Add(draftListBox);
            containerPanel.Controls.Add(draftListPanel);
            containerPanel.Controls.Add(buttonPanel);  // Add buttonPanel first so it appears at the top

            // Add the container panel to the tab page
            draftListTabPage.Controls.Add(containerPanel);

            // Initial update of the draft list
            UpdateDraftList();

            // Handle button positioning when the form resizes
            buttonPanel.Resize += (sender, e) =>
            {
                copyDraftListButton.Location = new Point(
                    (buttonPanel.ClientSize.Width - copyDraftListButton.Width) / 2,
                    (buttonPanel.ClientSize.Height - copyDraftListButton.Height) / 2
                );
            };
        }

        private void CopyDraftListButton_Click(object sender, EventArgs e)
        {
            // Copy the content of the draft list (RichTextBox) to the clipboard
            Clipboard.SetText(draftListBox.Text);
        }

        private void UpdateDraftList()
        {
            if (players == null || players.Count == 0)
                return;

            StringBuilder sb = new StringBuilder();

            // Sort players by rank
            var sortedPlayers = players.OrderBy(p => p.Rank).ToList();

            foreach (var player in sortedPlayers)
            {
                // Format each player's information including PlayerType
                string playerInfo = string.Format("{0,3}. {1} {2} {3} - {4} - {5} ----- {6}\n",
                player.Rank,
                player.Name,
                GetCountryLogoString(player.PlayerCountry),
                GetPositionConcatenatedString(player.PlayerPosition, player),
                player.Height,
                GetPlayerTypeString(player.PlayerType),
                _strengthGen.GetStrengthOddsTranslated(player.PlayerPosition, player.Rank, GetStrengthString));

                sb.Append(playerInfo);
            }

            // Update the RichTextBox
            if (draftListBox.InvokeRequired)
            {
                draftListBox.Invoke(new Action(() => draftListBox.Text = sb.ToString()));
            }
            else
            {
                draftListBox.Text = sb.ToString();
            }
        }
        #endregion

        #region Refresh views
        // Call this after generating players to refresh the view
        private void RefreshPlayerList()
        {
            if (playersGridView.InvokeRequired)
                playersGridView.Invoke(new Action(BindPlayerData));
            else
                BindPlayerData();

            UpdateDraftList();

            if (finalStatsGridView != null)
                PopulateFinalStatsGrid();
        }
        #endregion
        
        #region Strings
        private string GetPlayerTypeString(PlayerTypeGenerator.PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerTypeGenerator.PlayerType.Sniper:
                    return "Marqueur naturel";
                case PlayerTypeGenerator.PlayerType.FabricantDeJeu:
                    return "Fabricant de Jeu";
                case PlayerTypeGenerator.PlayerType.AttaquantOffensif:
                    return "Attaquant Offensif";
                case PlayerTypeGenerator.PlayerType.AttaquantDePuissance:
                    return "Attaquant de Puissance";
                case PlayerTypeGenerator.PlayerType.AttaquantPolyvalent:
                    return "Attaquant Polyvalent";
                case PlayerTypeGenerator.PlayerType.JoueurDeCaractere:
                    return "Joueur de Caractère";
                case PlayerTypeGenerator.PlayerType.DefenseurOffensif:
                    return "Défenseur Offensif";
                case PlayerTypeGenerator.PlayerType.DefenseurDefensif:
                    return "Défenseur Défensif";
                case PlayerTypeGenerator.PlayerType.DefenseurPhysique:
                    return "Défenseur Physique";
                case PlayerTypeGenerator.PlayerType.Gardien:
                    return "Gardien";
                default:
                    return playerType.ToString();
            }
        }

        private string GetStrengthString(PositionStrengthGenerator.PositionStrength positionStrength)
        {
            switch (positionStrength)
            {
                case PositionStrengthGenerator.PositionStrength.Generational:
                    return "Générationnel";
                case PositionStrengthGenerator.PositionStrength.Elite:
                    return "Élite";
                case PositionStrengthGenerator.PositionStrength.FirstLine:
                    return "Première ligne";
                case PositionStrengthGenerator.PositionStrength.SecondLine:
                    return "Deuxième ligne";
                case PositionStrengthGenerator.PositionStrength.ThirdLine:
                    return "Troisième ligne";
                case PositionStrengthGenerator.PositionStrength.FourthLine:
                    return "Quatrième ligne";
                case PositionStrengthGenerator.PositionStrength.FirstPair:
                    return "Première paire";
                case PositionStrengthGenerator.PositionStrength.SecondPair:
                    return "Deuxième paire";
                case PositionStrengthGenerator.PositionStrength.ThirdPair:
                    return "Troisième paire";
                case PositionStrengthGenerator.PositionStrength.Starter:
                    return "Gardien partant";
                case PositionStrengthGenerator.PositionStrength.Backup:
                    return "Gardien auxiliaire";
                case PositionStrengthGenerator.PositionStrength.AHL:
                    return "AHL";
                default:
                    return positionStrength.ToString();
            }
        }

        private string GetPositionString(PositionGenerator.Position playerPosition)
        {
            switch (playerPosition)
            {
                case PositionGenerator.Position.Center:
                    return "Centre";
                case PositionGenerator.Position.Winger:
                    return "Ailier";
                case PositionGenerator.Position.Defenseman:
                    return "Défenseur";
                case PositionGenerator.Position.Goaltender:
                    return "Gardien";
                default:
                    return playerPosition.ToString();
            }
        }

        private string GetPositionConcatenatedString(PositionGenerator.Position playerPosition, Player player)
        {
            switch (playerPosition)
            {
                case PositionGenerator.Position.Center:
                    return player.Handedness == "Right" ? "C / AD" : "C / AG";
                case PositionGenerator.Position.Winger:
                    return player.Handedness == "Right" ? "AD / AG" : "AG / AD";
                case PositionGenerator.Position.Defenseman:
                    return "D";
                case PositionGenerator.Position.Goaltender:
                    return "G";
                default:
                    return playerPosition.ToString();
            }
        }

        private string GetCountryString(CountryGenerator.Country playerCountry)
        {
            switch (playerCountry)
            {
                case CountryGenerator.Country.Canada:
                    return "Canada";
                case CountryGenerator.Country.UnitedStates:
                    return "USA";
                case CountryGenerator.Country.Sweden:
                    return "Suède";
                case CountryGenerator.Country.Russia:
                    return "Russie";
                case CountryGenerator.Country.Finland:
                    return "Finlande";
                case CountryGenerator.Country.CzechRepublic:
                    return "République tchèque";
                case CountryGenerator.Country.Slovakia:
                    return "Slovaquie";
                case CountryGenerator.Country.Switzerland:
                    return "Suisse";
                case CountryGenerator.Country.Germany:
                    return "Allemagne";
                case CountryGenerator.Country.Denmark:
                    return "Danemark";
                case CountryGenerator.Country.Latvia:
                    return "Lettonie";
                case CountryGenerator.Country.Belarus:
                    return "Belarus";
                case CountryGenerator.Country.Slovenia:
                    return "Slovénie";
                default:
                    return playerCountry.ToString();
            }
        }

        private string GetCountryLogoString(CountryGenerator.Country playerCountry)
        {
            switch (playerCountry)
            {
                case CountryGenerator.Country.Canada:
                    return "CAN";
                case CountryGenerator.Country.UnitedStates:
                    return "USA";
                case CountryGenerator.Country.Sweden:
                    return "SWE";
                case CountryGenerator.Country.Russia:
                    return "RUS";
                case CountryGenerator.Country.Finland:
                    return "FIN";
                case CountryGenerator.Country.CzechRepublic:
                    return "CZE";
                case CountryGenerator.Country.Slovakia:
                    return "SVK";
                case CountryGenerator.Country.Switzerland:
                    return "SUI";
                case CountryGenerator.Country.Germany:
                    return "GER";
                case CountryGenerator.Country.Denmark:
                    return "DEN";
                case CountryGenerator.Country.Latvia:
                    return "LAT";
                case CountryGenerator.Country.Belarus:
                    return "BLR";
                case CountryGenerator.Country.Slovenia:
                    return "SLK";
                default:
                    return playerCountry.ToString();
            }
        }
        #endregion

        #region Generate Players
        private void GeneratePlayers()
        {
            // Reset name generator static collections
            typeof(NameGenerator)
                .GetField("usedFirstNames", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new HashSet<string>());

            typeof(NameGenerator)
                .GetField("usedLastNames", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new HashSet<string>());

            typeof(NameGenerator)
                .GetMethod("InitializeNameLists", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.Invoke(null, null);

            // Clear existing players
            if (players.Count > 0)
                players.Clear();

            // Create generators with total number of players
            var posGen = new PositionGenerator(60);
            var countryGen = new CountryGenerator(60);
            var typeGen = new PlayerTypeGenerator(60);
            var heightGen = new HeightGenerator();

            // Generate players
            for (int i = 1; i <= 60; i++)
            {
                Player player = new Player(i, posGen, countryGen, typeGen, _strengthGen, heightGen, this);
                heightGen.SetMinHeightForElitePlayers(player); // Gen and Elite players can't be under 6 feets
                _baseAttributesGenerator.SetPlayerBaseAttributes(player);

                var (targetOff, targetDef) = _playerTargetAttributes.GetTargets(player.PlayerType, player.PositionStrength);
                player.TargetOffense = targetOff;
                player.TargetDefense = targetDef;

                if (player.PlayerType != PlayerTypeGenerator.PlayerType.Gardien)
                {
                    DistributeOffensiveAttributes(player);
                    DistributeDefensiveAttributes(player);
                    DistributeOtherAttributes(player);
                }
                DistributeStartingAttributes(player);

                players.Add(player);
            }
        }
        #endregion

        #region Generate real draft
        public void GeneratePlayersWithSpecs(List<MenuDraftReel.PlayerSpecification> playerSpecs)
        {
            // Reset name generator static collections (same as in GeneratePlayers method)
            typeof(NameGenerator)
                .GetField("usedFirstNames", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new HashSet<string>());

            typeof(NameGenerator)
                .GetField("usedLastNames", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new HashSet<string>());

            typeof(NameGenerator)
                .GetMethod("InitializeNameLists", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.Invoke(null, null);

            // Clear existing players
            if (players.Count > 0)
                players.Clear();

            // Create generators with total number of players
            var posGen = new PositionGenerator(60);
            var countryGen = new CountryGenerator(60);
            var typeGen = new PlayerTypeGenerator(60);
            var heightGen = new HeightGenerator();

            // Generate players with specifications
            foreach (var spec in playerSpecs)
            {
                // Create a player with either specified or random values
                Player player = CreatePlayerWithSpec(spec, posGen, countryGen, typeGen, _strengthGen, heightGen);

                // Set minimum height for elite players if necessary
                heightGen.SetMinHeightForElitePlayers(player);

                // Set all the attributes
                _baseAttributesGenerator.SetPlayerBaseAttributes(player);

                var (targetOff, targetDef) = _playerTargetAttributes.GetTargets(player.PlayerType, player.PositionStrength);
                player.TargetOffense = targetOff;
                player.TargetDefense = targetDef;

                if (player.PlayerType != PlayerTypeGenerator.PlayerType.Gardien)
                {
                    DistributeOffensiveAttributes(player);
                    DistributeDefensiveAttributes(player);
                    DistributeOtherAttributes(player);
                }
                DistributeStartingAttributes(player);
                players.Add(player);
            }

            // Refresh the UI
            RefreshPlayerList();
        }

        // Helper method to create a player with specified values
        private Player CreatePlayerWithSpec(
            MenuDraftReel.PlayerSpecification spec,
            PositionGenerator posGen,
            CountryGenerator countryGen,
            PlayerTypeGenerator typeGen,
            PositionStrengthGenerator strengthGen,
            HeightGenerator heightGen)
        {
            // Create a basic player with default random values
            Player player = new Player(spec.Rank, posGen, countryGen, typeGen, strengthGen, heightGen, this);

            // Override with specified values if provided
            if (!string.IsNullOrEmpty(spec.Name))
            {
                player.Name = spec.Name;
            }

            if (!string.IsNullOrEmpty(spec.Country))
            {
                // Create a mapping from display names to enum values
                switch (spec.Country)
                {
                    case "Canada":
                        player.PlayerCountry = CountryGenerator.Country.Canada;
                        break;
                    case "USA":
                        player.PlayerCountry = CountryGenerator.Country.UnitedStates;
                        break;
                    case "Suède":
                        player.PlayerCountry = CountryGenerator.Country.Sweden;
                        break;
                    case "Russie":
                        player.PlayerCountry = CountryGenerator.Country.Russia;
                        break;
                    case "Finlande":
                        player.PlayerCountry = CountryGenerator.Country.Finland;
                        break;
                    case "République tchèque":
                        player.PlayerCountry = CountryGenerator.Country.CzechRepublic;
                        break;
                    case "Slovaquie":
                        player.PlayerCountry = CountryGenerator.Country.Slovakia;
                        break;
                    case "Suisse":
                        player.PlayerCountry = CountryGenerator.Country.Switzerland;
                        break;
                    case "Allemagne":
                        player.PlayerCountry = CountryGenerator.Country.Germany;
                        break;
                    case "Danemark":
                        player.PlayerCountry = CountryGenerator.Country.Denmark;
                        break;
                    case "Lettonie":
                        player.PlayerCountry = CountryGenerator.Country.Latvia;
                        break;
                    case "Belarus":
                        player.PlayerCountry = CountryGenerator.Country.Belarus;
                        break;
                    case "Slovénie":
                        player.PlayerCountry = CountryGenerator.Country.Slovenia;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(spec.Position))
            {
                // Map position string to enum
                switch (spec.Position)
                {
                    case "AG / AD":
                        player.PlayerPosition = PositionGenerator.Position.Winger;
                        break;
                    case "AD / AG":
                        player.PlayerPosition = PositionGenerator.Position.Winger;
                        break;
                    case "C / AG":
                        player.PlayerPosition = PositionGenerator.Position.Center;
                        break;
                    case "C / AD":
                        player.PlayerPosition = PositionGenerator.Position.Center;
                        break;
                    case "D":
                        player.PlayerPosition = PositionGenerator.Position.Defenseman;
                        break;
                    case "G":
                        player.PlayerPosition = PositionGenerator.Position.Goaltender;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(spec.Height))
            {
                player.Height = spec.Height;
            }

            if (!string.IsNullOrEmpty(spec.PlayerType))
            {
                // Map player type string to enum
                switch (spec.PlayerType)
                {
                    case "Marqueur naturel":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.Sniper;
                        break;
                    case "Fabricant de Jeu":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.FabricantDeJeu;
                        break;
                    case "Attaquant Offensif":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.AttaquantOffensif;
                        break;
                    case "Attaquant Polyvalent":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.AttaquantPolyvalent;
                        break;
                    case "Joueur de Caractère":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.JoueurDeCaractere;
                        break;
                    case "Attaquant de Puissance":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.AttaquantDePuissance;
                        break;
                    case "Défenseur Offensif":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.DefenseurOffensif;
                        break;
                    case "Défenseur Défensif":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.DefenseurDefensif;
                        break;
                    case "Défenseur Physique":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.DefenseurPhysique;
                        break;
                    case "Gardien":
                        player.PlayerType = PlayerTypeGenerator.PlayerType.Gardien;
                        break;
                }
            }

            return player;
        }
        #endregion

        #region Distribute offensive attributes
        private void DistributeOffensiveAttributes(Player player)
        {
            // Condition 1: Attaquant Offensif
            if (player.PlayerType == PlayerTypeGenerator.PlayerType.AttaquantOffensif || player.TargetOffense == 99)
            {
                player.Shooting = player.TargetOffense;
                player.Playmaking = player.TargetOffense;
                player.Stickhandling = player.TargetOffense;
                return;
            }

            int targetTotalPoints = player.TargetOffense * 3;
            int currentTotalPoints = player.Shooting + player.Playmaking + player.Stickhandling;
            int remainingPoints = targetTotalPoints - currentTotalPoints;

            do
            {
                if (remainingPoints == 0)
                    return;
                else if (remainingPoints < 0)
                {
                    int pointsToSubtract = remainingPoints / 3;

                    player.Shooting -= pointsToSubtract;
                    player.Playmaking -= pointsToSubtract;
                    player.Stickhandling -= pointsToSubtract;
                    return;
                }

                int firstValue = remainingPoints / 3;
                int shootingBoost;

                using (var generator = new SecureRandomGenerator())
                    shootingBoost = generator.GetRandomValue(0, firstValue + 1);
                Console.WriteLine("firstValue : " + firstValue);
                Console.WriteLine("shootingBoost : " + shootingBoost);
                // int shootingBoost = GetSecureRandomValue(0, firstValue + 1);

                remainingPoints -= shootingBoost;

                if (player.Shooting + shootingBoost > 99)
                {
                    int shootingBeforeReduction = player.Shooting + shootingBoost;
                    int pointsToBeDistributed = shootingBeforeReduction - 99;
                    int pointsToAddToSingleAttribute = pointsToBeDistributed / 2;
                    player.Shooting = 99;

                    if (player.Playmaking + pointsToAddToSingleAttribute > 99)
                    {
                        int excessPlaymaking = player.Playmaking + pointsToAddToSingleAttribute - 99;
                        player.Playmaking = 99;

                        if (player.Stickhandling + excessPlaymaking > 99)
                        {
                            player.Stickhandling = 99;
                            break;
                        }
                        else
                        {
                            player.Stickhandling += excessPlaymaking;
                            break;
                        }
                    }
                    else
                    {
                        player.Playmaking += pointsToAddToSingleAttribute;
                        remainingPoints -= pointsToAddToSingleAttribute;

                        if (player.Stickhandling + pointsToAddToSingleAttribute > 99)
                        {
                            int excessStickhandling = player.Stickhandling + pointsToAddToSingleAttribute - 99;
                            player.Stickhandling = 99;

                            if (player.Playmaking + excessStickhandling > 99)
                            {
                                player.Playmaking = 99;
                                break;
                            }
                            else
                            {
                                player.Playmaking += excessStickhandling;
                                break;
                            }
                        }
                        else
                        {
                            player.Stickhandling += remainingPoints;
                            break;
                        }
                    }
                }
                else
                {
                    player.Shooting += shootingBoost;
                }

                int playmakingBoost;

                // Handle Playmaking
                using (var generator = new SecureRandomGenerator())
                    playmakingBoost = generator.GetRandomValue(0, firstValue + 1);

                // int playmakingBoost = GetSecureRandomValue(0, firstValue + 1);
                Console.WriteLine("playmakingBoost : " + playmakingBoost);
                remainingPoints -= playmakingBoost;

                if (player.Playmaking + playmakingBoost > 99)
                {
                    int playmakingBeforeReduction = player.Playmaking + playmakingBoost;
                    int pointsToBeDistributed = playmakingBeforeReduction - 99;
                    int pointsToAddToSingleAttribute = pointsToBeDistributed / 2;
                    player.Playmaking = 99;

                    if (player.Stickhandling + pointsToAddToSingleAttribute > 99)
                    {
                        int excessStickhandling = player.Stickhandling + pointsToAddToSingleAttribute - 99;
                        player.Stickhandling = 99;

                        if (player.Shooting + excessStickhandling > 99)
                        {
                            player.Shooting = 99;
                            break;
                        }
                        else
                        {
                            player.Shooting += excessStickhandling;
                            break;
                        }
                    }
                    else
                    {
                        player.Stickhandling += pointsToAddToSingleAttribute;
                        remainingPoints -= pointsToAddToSingleAttribute;

                        if (player.Shooting + pointsToAddToSingleAttribute > 99)
                        {
                            int excessShooting = player.Shooting + pointsToAddToSingleAttribute - 99;
                            player.Shooting = 99;

                            if (player.Stickhandling + excessShooting > 99)
                            {
                                player.Stickhandling = 99;
                                break;
                            }
                            else
                            {
                                player.Stickhandling += excessShooting;
                                break;
                            }
                        }
                        else
                        {
                            player.Shooting += remainingPoints;
                            break;
                        }
                    }
                }
                else
                {
                    player.Playmaking += playmakingBoost;
                }

                int stickhandlingBoost;

                // Handle Stickhandling
                using (var generator = new SecureRandomGenerator())
                    stickhandlingBoost = generator.GetRandomValue(0, firstValue + 1);

                // int stickhandlingBoost = GetSecureRandomValue(0, firstValue + 1);
                Console.WriteLine("random stickhandlingBoost : " + stickhandlingBoost);

                remainingPoints -= stickhandlingBoost;
                Console.WriteLine("remainingPoints : " + remainingPoints);
                if (player.Stickhandling + stickhandlingBoost > 99)
                {
                    int stickhandlingBeforeReduction = player.Stickhandling + stickhandlingBoost;
                    int pointsToBeDistributed = stickhandlingBeforeReduction - 99;
                    int pointsToAddToSingleAttribute = pointsToBeDistributed / 2;
                    player.Stickhandling = 99;

                    if (player.Shooting + pointsToAddToSingleAttribute > 99)
                    {
                        int excessShooting = player.Shooting + pointsToAddToSingleAttribute - 99;
                        player.Shooting = 99;

                        if (player.Playmaking + excessShooting > 99)
                        {
                            player.Playmaking = 99;
                            break;
                        }
                        else
                        {
                            player.Playmaking += excessShooting;
                            break;
                        }
                    }
                    else
                    {
                        player.Shooting += pointsToAddToSingleAttribute;
                        remainingPoints -= pointsToAddToSingleAttribute;

                        if (player.Playmaking + pointsToAddToSingleAttribute > 99)
                        {
                            int excessPlaymaking = player.Playmaking + pointsToAddToSingleAttribute - 99;
                            player.Playmaking = 99;

                            if (player.Shooting + excessPlaymaking > 99)
                            {
                                player.Shooting = 99;
                                break;
                            }
                            else
                            {
                                player.Shooting += excessPlaymaking;
                                break;
                            }
                        }
                        else
                        {
                            player.Playmaking += remainingPoints;
                            break;
                        }
                    }
                }
                else
                {
                    player.Stickhandling += stickhandlingBoost;
                }

                targetTotalPoints = player.TargetOffense * 3;
                currentTotalPoints = player.Shooting + player.Playmaking + player.Stickhandling;
                remainingPoints = targetTotalPoints - currentTotalPoints;

            } while (remainingPoints >= 3);

            if (remainingPoints > 0)
                DistributeRemainingOffensePts(player, remainingPoints); // Distribute the last couple of points left

            DistributeOffenseOverflows(player); // Distribute overflows of attributes that are over 99

            // Make sure Shooting is the best offensive attributes for snipers
            if (player.PlayerType == PlayerTypeGenerator.PlayerType.Sniper)
            {
                int[] offensiveAttributes = { player.Shooting, player.Playmaking, player.Stickhandling };
                int maxIndex = Array.IndexOf(offensiveAttributes, offensiveAttributes.Max());

                if (maxIndex != 0)
                {
                    int temp = player.Shooting;
                    switch (maxIndex)
                    {
                        case 1:
                            player.Shooting = player.Playmaking;
                            player.Playmaking = temp;
                            break;
                        case 2:
                            player.Shooting = player.Stickhandling;
                            player.Stickhandling = temp;
                            break;
                    }
                }
            }

            // // Make sure Playmaking is the best offensive attributes for playmakers
            if (player.PlayerType == PlayerTypeGenerator.PlayerType.FabricantDeJeu)
            {
                int[] offensiveAttributes = { player.Shooting, player.Playmaking, player.Stickhandling };
                int maxIndex = Array.IndexOf(offensiveAttributes, offensiveAttributes.Max());

                if (maxIndex != 1)
                {
                    int temp = player.Playmaking;
                    switch (maxIndex)
                    {
                        case 0:
                            player.Playmaking = player.Shooting;
                            player.Shooting = temp;
                            break;
                        case 2:
                            player.Playmaking = player.Stickhandling;
                            player.Stickhandling = temp;
                            break;
                    }
                }
            }
        }

        private void DistributeRemainingOffensePts(Player player, int remainingPoints)
        {
            if (remainingPoints <= 0) return;

            // Create array of offensive attributes with their current values
            var attributes = new[]
            {
        new { Name = "Shooting", Value = player.Shooting },
        new { Name = "Playmaking", Value = player.Playmaking },
        new { Name = "Stickhandling", Value = player.Stickhandling }
    };

            // Sort by value to get lowest attributes first
            var sortedAttributes = attributes.OrderBy(a => a.Value).ToArray();

            // Add 1 point to the lowest attribute if possible
            if (sortedAttributes[0].Value < 99)
            {
                switch (sortedAttributes[0].Name)
                {
                    case "Shooting":
                        player.Shooting++;
                        break;
                    case "Playmaking":
                        player.Playmaking++;
                        break;
                    case "Stickhandling":
                        player.Stickhandling++;
                        break;
                }
                remainingPoints--;
            }

            // If we still have a point, add it to the second lowest attribute if possible
            if (remainingPoints > 0 && sortedAttributes[1].Value < 99)
            {
                switch (sortedAttributes[1].Name)
                {
                    case "Shooting":
                        player.Shooting++;
                        break;
                    case "Playmaking":
                        player.Playmaking++;
                        break;
                    case "Stickhandling":
                        player.Stickhandling++;
                        break;
                }
            }
        }

        private void DistributeOffenseOverflows(Player player)
        {
            // Continue redistributing until all stats are 99 or below
            while (player.Shooting > 99 || player.Playmaking > 99 || player.Stickhandling > 99)
            {
                if (player.Shooting > 99)
                {
                    int overflow = player.Shooting - 99;
                    player.Shooting = 99;

                    // Distribute overflow equally to the other two stats
                    player.Playmaking += overflow / 2;
                    player.Stickhandling += overflow - (overflow / 2); // Handle any remainder
                }

                if (player.Playmaking > 99)
                {
                    int overflow = player.Playmaking - 99;
                    player.Playmaking = 99;

                    // Distribute overflow equally to the other two stats
                    player.Shooting += overflow / 2;
                    player.Stickhandling += overflow - (overflow / 2);
                }

                if (player.Stickhandling > 99)
                {
                    int overflow = player.Stickhandling - 99;
                    player.Stickhandling = 99;

                    // Distribute overflow equally to the other two stats
                    player.Shooting += overflow / 2;
                    player.Playmaking += overflow - (overflow / 2);
                }
            }
        }
        #endregion

        #region Distribute defensive attributes
        private void DistributeDefensiveAttributes(Player player)
        {
            int maxHit;

            if (player.Height == "5'9\"" || player.Height == "5'10\"" || player.Height == "5'11\"")
                maxHit = 67;
            else if (player.Height == "6'0\"" || player.Height == "6'1\"")
                maxHit = 86;
            else
                maxHit = 99;

            int targetTotalPoints = player.TargetDefense * 3;
            int currentTotalPoints = player.Checking + player.Positioning + player.Hitting;
            int remainingPoints = targetTotalPoints - currentTotalPoints;

            do
            {
                if (remainingPoints == 0)
                    return;
                else if (remainingPoints < 0)
                {
                    int pointsToSubtract = remainingPoints / 3;

                    player.Checking -= pointsToSubtract;
                    player.Positioning -= pointsToSubtract;
                    player.Hitting -= pointsToSubtract;
                    return;
                }

                int firstValue = remainingPoints / 3;
                int checkingBoost;

                using (var generator = new SecureRandomGenerator())
                    checkingBoost = generator.GetRandomValue(0, firstValue + 1);
                Console.WriteLine("firstValue : " + firstValue);
                Console.WriteLine("checkingBoost : " + checkingBoost);
                // int CheckingBoost = GetSecureRandomValue(0, firstValue + 1);

                remainingPoints -= checkingBoost;
                Console.WriteLine("remainingPoints : " + remainingPoints);
                if (player.Checking + checkingBoost > 99)
                {
                    int checkingBeforeReduction = player.Checking + checkingBoost;
                    int pointsToBeDistributed = checkingBeforeReduction - 99;
                    int pointsToAddToSingleAttribute = pointsToBeDistributed / 2;
                    player.Checking = 99;

                    if (player.Positioning + pointsToAddToSingleAttribute > 99)
                    {
                        int excessPositioning = player.Positioning + pointsToAddToSingleAttribute - 99;
                        player.Positioning = 99;

                        if (player.Hitting + excessPositioning > 99)
                        {
                            player.Hitting = 99;
                            break;
                        }
                        else
                        {
                            player.Hitting += excessPositioning;
                            break;
                        }
                    }
                    else
                    {
                        player.Positioning += pointsToAddToSingleAttribute;
                        remainingPoints -= pointsToAddToSingleAttribute;

                        if (player.Hitting + pointsToAddToSingleAttribute > 99)
                        {
                            int excessHitting = player.Hitting + pointsToAddToSingleAttribute - 99;
                            player.Hitting = 99;

                            if (player.Positioning + excessHitting > 99)
                            {
                                player.Positioning = 99;
                                break;
                            }
                            else
                            {
                                player.Positioning += excessHitting;
                                break;
                            }
                        }
                        else
                        {
                            player.Hitting += remainingPoints;
                            break;
                        }
                    }
                }
                else
                {
                    player.Checking += checkingBoost;
                }

                int PositioningBoost;

                // Handle Positioning
                using (var generator = new SecureRandomGenerator())
                    PositioningBoost = generator.GetRandomValue(0, firstValue + 1);

                // int PositioningBoost = GetSecureRandomValue(0, firstValue + 1);
                Console.WriteLine("PositioningBoost : " + PositioningBoost);
                remainingPoints -= PositioningBoost;
                Console.WriteLine("remainingPoints : " + remainingPoints);
                if (player.Positioning + PositioningBoost > 99)
                {
                    int PositioningBeforeReduction = player.Positioning + PositioningBoost;
                    int pointsToBeDistributed = PositioningBeforeReduction - 99;
                    int pointsToAddToSingleAttribute = pointsToBeDistributed / 2;
                    player.Positioning = 99;

                    if (player.Hitting + pointsToAddToSingleAttribute > 99)
                    {
                        int excessHitting = player.Hitting + pointsToAddToSingleAttribute - 99;
                        player.Hitting = 99;

                        if (player.Checking + excessHitting > 99)
                        {
                            player.Checking = 99;
                            break;
                        }
                        else
                        {
                            player.Checking += excessHitting;
                            break;
                        }
                    }
                    else
                    {
                        player.Hitting += pointsToAddToSingleAttribute;
                        remainingPoints -= pointsToAddToSingleAttribute;

                        if (player.Checking + pointsToAddToSingleAttribute > 99)
                        {
                            int excessChecking = player.Checking + pointsToAddToSingleAttribute - 99;
                            player.Checking = 99;

                            if (player.Hitting + excessChecking > 99)
                            {
                                player.Hitting = 99;
                                break;
                            }
                            else
                            {
                                player.Hitting += excessChecking;
                                break;
                            }
                        }
                        else
                        {
                            player.Checking += remainingPoints;
                            break;
                        }
                    }
                }
                else
                {
                    player.Positioning += PositioningBoost;
                }

                int HittingBoost;

                // Handle Hitting
                using (var generator = new SecureRandomGenerator())
                    HittingBoost = generator.GetRandomValue(0, firstValue + 1);

                // int HittingBoost = GetSecureRandomValue(0, firstValue + 1);
                Console.WriteLine("HittingBoost : " + HittingBoost);

                remainingPoints -= HittingBoost;
                Console.WriteLine("remainingPoints : " + remainingPoints);
                if (player.Hitting + HittingBoost > 99)
                {
                    int HittingBeforeReduction = player.Hitting + HittingBoost;
                    int pointsToBeDistributed = HittingBeforeReduction - 99;
                    int pointsToAddToSingleAttribute = pointsToBeDistributed / 2;
                    player.Hitting = 99;

                    if (player.Checking + pointsToAddToSingleAttribute > 99)
                    {
                        int excessChecking = player.Checking + pointsToAddToSingleAttribute - 99;
                        player.Checking = 99;

                        if (player.Positioning + excessChecking > 99)
                        {
                            player.Positioning = 99;
                            break;
                        }
                        else
                        {
                            player.Positioning += excessChecking;
                            break;
                        }
                    }
                    else
                    {
                        player.Checking += pointsToAddToSingleAttribute;
                        remainingPoints -= pointsToAddToSingleAttribute;

                        if (player.Positioning + pointsToAddToSingleAttribute > 99)
                        {
                            int excessPositioning = player.Positioning + pointsToAddToSingleAttribute - 99;
                            player.Positioning = 99;

                            if (player.Checking + excessPositioning > 99)
                            {
                                player.Checking = 99;
                                break;
                            }
                            else
                            {
                                player.Checking += excessPositioning;
                                break;
                            }
                        }
                        else
                        {
                            player.Positioning += remainingPoints;
                            break;
                        }
                    }
                }
                else
                {
                    player.Hitting += HittingBoost;
                }

                targetTotalPoints = player.TargetDefense * 3;
                currentTotalPoints = player.Checking + player.Positioning + player.Hitting;
                remainingPoints = targetTotalPoints - currentTotalPoints;

            } while (remainingPoints >= 3);

            if (remainingPoints > 0)
                DistributeRemainingDefensePts(player, remainingPoints, maxHit);

            // Distribute overflows of defensive attributes that are over 99 to the other def attributes
            DistributeDefenseOverflows(player, maxHit);

            if (player.PlayerType == PlayerTypeGenerator.PlayerType.AttaquantDePuissance || player.PlayerType == PlayerTypeGenerator.PlayerType.DefenseurPhysique)
            {
                // Ensure Hitting is the best defensive attributes for powerforwards and physical defensemen
                int[] defensiveAttributes = { player.Checking, player.Positioning, player.Hitting };
                int maxIndex = Array.IndexOf(defensiveAttributes, defensiveAttributes.Max());

                if (maxIndex != 2)
                {
                    int temp = player.Hitting;
                    switch (maxIndex)
                    {
                        case 0:
                            player.Hitting = player.Checking;
                            player.Checking = temp;
                            break;
                        case 1:
                            player.Hitting = player.Positioning;
                            player.Positioning = temp;
                            break;
                    }
                }
            }
            else if (player.PlayerType == PlayerTypeGenerator.PlayerType.AttaquantPolyvalent || player.PlayerType == PlayerTypeGenerator.PlayerType.DefenseurDefensif)
            {
                // Ensure Positioning is the best defensive attributes for AttaquantPolyvalent and DefenseurDefensif
                int[] defensiveAttributes = { player.Checking, player.Positioning, player.Hitting };
                int maxIndex = Array.IndexOf(defensiveAttributes, defensiveAttributes.Max());

                if (maxIndex != 1)
                {
                    int temp = player.Positioning;
                    switch (maxIndex)
                    {
                        case 0:
                            player.Positioning = player.Checking;
                            player.Checking = temp;
                            break;
                        case 2:
                            player.Positioning = player.Hitting;
                            player.Hitting = temp;
                            break;
                    }
                }
            }
        }

        private void DistributeRemainingDefensePts(Player player, int remainingPoints, int maxHit)
        {
            if (remainingPoints <= 0) return;

            // Create array of defensive attributes with their current values and max values
            var attributes = new[]
            {
        new { Name = "Checking", Value = player.Checking, MaxValue = 99 },
        new { Name = "Positioning", Value = player.Positioning, MaxValue = 99 },
        new { Name = "Hitting", Value = player.Hitting, MaxValue = maxHit }
    };

            // Sort by value to get lowest attributes first
            var sortedAttributes = attributes.OrderBy(a => a.Value).ToArray();

            // Add 1 point to the lowest attribute if possible
            if (sortedAttributes[0].Value < sortedAttributes[0].MaxValue)
            {
                switch (sortedAttributes[0].Name)
                {
                    case "Checking":
                        player.Checking++;
                        break;
                    case "Positioning":
                        player.Positioning++;
                        break;
                    case "Hitting":
                        player.Hitting++;
                        break;
                }
                remainingPoints--;
            }

            // If we still have a point, add it to the second lowest attribute if possible
            if (remainingPoints > 0 && sortedAttributes[1].Value < sortedAttributes[1].MaxValue)
            {
                switch (sortedAttributes[1].Name)
                {
                    case "Checking":
                        player.Checking++;
                        break;
                    case "Positioning":
                        player.Positioning++;
                        break;
                    case "Hitting":
                        player.Hitting++;
                        break;
                }
            }
        }

        private void DistributeDefenseOverflows(Player player, int maxHit)
        {
            // Continue redistributing until all stats are 99 or below
            while (player.Checking > 99 || player.Positioning > 99 || player.Hitting > maxHit)
            {
                if (player.Checking > 99)
                {
                    int overflow = player.Checking - 99;
                    player.Checking = 99;

                    // Distribute overflow equally to the other two stats
                    player.Positioning += overflow / 2;
                    player.Hitting += overflow - (overflow / 2); // Handle any remainder
                }

                if (player.Positioning > 99)
                {
                    int overflow = player.Positioning - 99;
                    player.Positioning = 99;

                    // Distribute overflow equally to the other two stats
                    player.Checking += overflow / 2;
                    player.Hitting += overflow - (overflow / 2);
                }

                if (player.Hitting > maxHit)
                {
                    int overflow = player.Hitting - maxHit;
                    player.Hitting = maxHit;

                    // Distribute overflow equally to the other two stats
                    player.Checking += overflow / 2;
                    player.Positioning += overflow - (overflow / 2);
                }
            }
        }
        #endregion

        #region Distribute physical attributes
        private void DistributeOtherAttributes(Player player)
        {
            int skatingBoost = RollSkatingBoost(player); // Its possible to roll a value that goes up to 11
            int enduranceBoost = RollEnduranceBoost(player); // Its possible to roll a value that goes up to 11

            // If value is over 11, set it to 9
            // We do this on purpose to create a bias which in turns makes it more likely to get a better boost
            if (skatingBoost >= 10)
                skatingBoost = 9;
            if (enduranceBoost >= 10)
                enduranceBoost = 9;

            player.Skating += skatingBoost;
            player.Endurance += enduranceBoost;

            RollFighting(player);
            RollPenalty(player);
            RollFaceoffs(player);
            RollStrength(player);
            AdjustPhysicalAccordingToHeight(player);
        }

        private int RollSkatingBoost(Player player)
        {
            int firstSkatingBoost;
            int secondSkatingBoost;

            using (var generator = new SecureRandomGenerator())
                firstSkatingBoost = generator.GetRandomValue(0, 5);

            using (var generator = new SecureRandomGenerator())
                secondSkatingBoost = generator.GetRandomValue(0, 5);

            int totalSkatingBoost = firstSkatingBoost + secondSkatingBoost;

            return totalSkatingBoost;
        }

        private int RollEnduranceBoost(Player player)
        {
            int firstEnduranceBoost;
            int secondEnduranceBoost;

            using (var generator = new SecureRandomGenerator())
                firstEnduranceBoost = generator.GetRandomValue(0, 5);

            using (var generator = new SecureRandomGenerator())
                secondEnduranceBoost = generator.GetRandomValue(0, 5);

            int totalEnduranceBoost = firstEnduranceBoost + secondEnduranceBoost;

            return totalEnduranceBoost;
        }

        private void RollFighting(Player player)
        {
            int maxRoll = 99 - player.Fighting;
            int fightingBoost;

            using (var generator = new SecureRandomGenerator())
                fightingBoost = generator.GetRandomValue(0, maxRoll);

            player.Fighting += fightingBoost;
        }

        private void RollPenalty(Player player)
        {
            int maxRoll = 99 - player.Penalty;
            int penaltyBoost;

            using (var generator = new SecureRandomGenerator())
                penaltyBoost = generator.GetRandomValue(0, maxRoll);

            player.Penalty += penaltyBoost;
        }

        private void RollFaceoffs(Player player)
        {
            int maxRoll = 0;
            int faceoffBoost;

            if (player.PlayerPosition != PositionGenerator.Position.Center)
                maxRoll = 5;
            else if (player.PositionStrength == PositionStrengthGenerator.PositionStrength.Generational ||
                player.PositionStrength == PositionStrengthGenerator.PositionStrength.Elite)
            {
                player.Faceoffs = 85;
                maxRoll = 6;
            }
            else if (player.PositionStrength == PositionStrengthGenerator.PositionStrength.FirstLine)
            {
                player.Faceoffs = 81;
                maxRoll = 6;
            }
            else if (player.PositionStrength == PositionStrengthGenerator.PositionStrength.SecondLine)
            {
                player.Faceoffs = 79;
                maxRoll = 5;
            }
            else if (player.PositionStrength == PositionStrengthGenerator.PositionStrength.ThirdLine)
            {
                player.Faceoffs = 75;
                maxRoll = 5;
            }
            else if (player.PositionStrength == PositionStrengthGenerator.PositionStrength.FourthLine)
            {
                player.Faceoffs = 72;
                maxRoll = 5;
            }

            using (var generator = new SecureRandomGenerator())
                faceoffBoost = generator.GetRandomValue(0, maxRoll);
            player.Faceoffs += faceoffBoost;
        }

        private void RollStrength(Player player)
        {
            int maxRoll = 0;
            int strengthBoost;

            if (player.Height == "5'9\"" || player.Height == "5'10\"" || player.Height == "5'11\"")
            {
                // Give a small skating boost to small players to compensate for lower strengths values
                if (player.Skating <= 99 && player.Skating >= 96)
                    player.Skating = 99;
                else if (player.Skating < 96)
                    player.Skating += 3;

                maxRoll = 0;
            }
            else
                maxRoll = 6;

            using (var generator = new SecureRandomGenerator())
                strengthBoost = generator.GetRandomValue(0, maxRoll);

            player.AttributeStrength += strengthBoost;
        }

        private void AdjustPhysicalAccordingToHeight(Player player)
        {
            if (player.Height == "5'9\"" || player.Height == "5'10\"")
            {
                player.Skating += 3;
                player.AttributeStrength -= 3;
            }
            if (player.Height == "5'11\"")
            {
                player.Skating += 2;
                player.AttributeStrength -= 2;
            }
            if (player.Height == "6'2\"" || player.Height == "6'3\"")
            {
                player.Skating -= 1;
                player.AttributeStrength += 1;
            }
            if (player.Height == "6'4\"" || player.Height == "6'5\"")
            {
                player.Skating -= 3;
                player.AttributeStrength += 3;
            }
            if (player.Height == "6'6\"" || player.Height == "6'7\"" || player.Height == "6'8\"")
            {
                player.Skating -= 4;
                player.AttributeStrength += 4;
            }
        }
        #endregion

        #region Distribute starting attributes
        private void DistributeStartingAttributes(Player player)
        {
            player.StartingPenalty = player.Penalty;
            player.StartingAttributeStrength = player.AttributeStrength;
            player.StartingFighting = 25;
            SetStartingLeadership(player);

            if (player.Rank <= 3)
            {
                player.StartingShooting = 56;
                player.StartingPlaymaking = 56;
                player.StartingStickhandling = 56;
                player.StartingChecking = 56;
                player.StartingPositioning = 56;
                player.StartingHitting = 56;
                player.StartingSkating = 56;
                player.StartingEndurance = 56;
                player.StartingFaceoffs = 52;
            }
            else if (player.Rank <= 10)
            {
                player.StartingShooting = 50;
                player.StartingPlaymaking = 50;
                player.StartingStickhandling = 50;
                player.StartingChecking = 50;
                player.StartingPositioning = 50;
                player.StartingHitting = 50;
                player.StartingSkating = 50;
                player.StartingEndurance = 50;
                player.StartingFaceoffs = 45;
            }
            else if (player.Rank <= 30)
            {
                player.StartingShooting = 43;
                player.StartingPlaymaking = 43;
                player.StartingStickhandling = 43;
                player.StartingChecking = 43;
                player.StartingPositioning = 43;
                player.StartingHitting = 43;
                player.StartingSkating = 43;
                player.StartingEndurance = 43;
                player.StartingFaceoffs = 40;
            }
            else
            {
                player.StartingShooting = 38;
                player.StartingPlaymaking = 38;
                player.StartingStickhandling = 38;
                player.StartingChecking = 38;
                player.StartingPositioning = 38;
                player.StartingHitting = 38;
                player.StartingSkating = 38;
                player.StartingEndurance = 38;
                player.StartingFaceoffs = 35;
            }

            // Add a random value between 1 and 5 to each attribute
            using (var generator = new SecureRandomGenerator())
            {
                player.StartingShooting += generator.GetRandomValue(1, 8);
                player.StartingPlaymaking += generator.GetRandomValue(1, 8);
                player.StartingStickhandling += generator.GetRandomValue(1, 8);
                player.StartingChecking += generator.GetRandomValue(1, 8);
                player.StartingPositioning += generator.GetRandomValue(1, 8);
                player.StartingHitting += generator.GetRandomValue(1, 8);
                player.StartingSkating += generator.GetRandomValue(1, 8);
                player.StartingEndurance += generator.GetRandomValue(1, 8);
                player.StartingFaceoffs += generator.GetRandomValue(1, 8);
            }
        }

        private void SetStartingLeadership(Player player)
        {
            if (player.Rank <= 3)
                player.StartingLeadership = 63;
            else if (player.Rank <= 10)
                player.StartingLeadership = 60;
            else if (player.Rank <= 20)
                player.StartingLeadership = 58;
            else if (player.Rank <= 30)
                player.StartingLeadership = 55;
            else
                player.StartingLeadership = 53;

            // Add a random value between 1 and 5 to StartingLeadership
            using (var generator = new SecureRandomGenerator())
                player.StartingLeadership += generator.GetRandomValue(1, 5);
        }

        private void AddRandomValueToStartingAttribute(Player player)
        {
            if (player.Rank <= 3)
                player.StartingLeadership = 63;
            else if (player.Rank <= 10)
                player.StartingLeadership = 60;
            else if (player.Rank <= 20)
                player.StartingLeadership = 58;
            else if (player.Rank <= 30)
                player.StartingLeadership = 55;
            else
                player.StartingLeadership = 53;

            // Add a random value between 1 and 5 to StartingLeadership
            using (var generator = new SecureRandomGenerator())
                player.StartingLeadership += generator.GetRandomValue(1, 5);
        }
        #endregion
    }
}