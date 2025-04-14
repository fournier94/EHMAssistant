
namespace EHMAssistant.ModifyRoster
{
    partial class RosterMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RosterMenu));
            this.tabCtrlRoster = new System.Windows.Forms.TabControl();
            this.tabRoster = new System.Windows.Forms.TabPage();
            this.tabTeamOverview = new System.Windows.Forms.TabPage();
            this.tabTeamsStrength = new System.Windows.Forms.TabPage();
            this.cboTeams = new System.Windows.Forms.ComboBox();
            this.dgvTeamRoster = new System.Windows.Forms.DataGridView();
            this.tabCtrlRoster.SuspendLayout();
            this.tabRoster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeamRoster)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCtrlRoster
            // 
            this.tabCtrlRoster.Controls.Add(this.tabRoster);
            this.tabCtrlRoster.Controls.Add(this.tabTeamOverview);
            this.tabCtrlRoster.Controls.Add(this.tabTeamsStrength);
            this.tabCtrlRoster.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrlRoster.Location = new System.Drawing.Point(12, 20);
            this.tabCtrlRoster.Name = "tabCtrlRoster";
            this.tabCtrlRoster.SelectedIndex = 0;
            this.tabCtrlRoster.Size = new System.Drawing.Size(1560, 823);
            this.tabCtrlRoster.TabIndex = 0;
            // 
            // tabRoster
            // 
            this.tabRoster.Controls.Add(this.dgvTeamRoster);
            this.tabRoster.Location = new System.Drawing.Point(4, 28);
            this.tabRoster.Name = "tabRoster";
            this.tabRoster.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoster.Size = new System.Drawing.Size(1552, 791);
            this.tabRoster.TabIndex = 0;
            this.tabRoster.Text = "Roster d\'équipe";
            this.tabRoster.UseVisualStyleBackColor = true;
            // 
            // tabTeamOverview
            // 
            this.tabTeamOverview.Location = new System.Drawing.Point(4, 28);
            this.tabTeamOverview.Name = "tabTeamOverview";
            this.tabTeamOverview.Size = new System.Drawing.Size(1552, 791);
            this.tabTeamOverview.TabIndex = 2;
            this.tabTeamOverview.Text = "Aperçu de l\'équipe";
            this.tabTeamOverview.UseVisualStyleBackColor = true;
            // 
            // tabTeamsStrength
            // 
            this.tabTeamsStrength.Location = new System.Drawing.Point(4, 28);
            this.tabTeamsStrength.Name = "tabTeamsStrength";
            this.tabTeamsStrength.Padding = new System.Windows.Forms.Padding(3);
            this.tabTeamsStrength.Size = new System.Drawing.Size(1552, 791);
            this.tabTeamsStrength.TabIndex = 1;
            this.tabTeamsStrength.Text = "Force des équipes";
            this.tabTeamsStrength.UseVisualStyleBackColor = true;
            // 
            // cboTeams
            // 
            this.cboTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTeams.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTeams.FormattingEnabled = true;
            this.cboTeams.Location = new System.Drawing.Point(785, 7);
            this.cboTeams.Name = "cboTeams";
            this.cboTeams.Size = new System.Drawing.Size(273, 32);
            this.cboTeams.TabIndex = 1;
            this.cboTeams.SelectedIndexChanged += new System.EventHandler(this.cboTeams_SelectedIndexChanged);
            // 
            // dgvTeamRoster
            // 
            this.dgvTeamRoster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeamRoster.Location = new System.Drawing.Point(6, 6);
            this.dgvTeamRoster.Name = "dgvTeamRoster";
            this.dgvTeamRoster.Size = new System.Drawing.Size(1540, 779);
            this.dgvTeamRoster.TabIndex = 0;
            // 
            // RosterMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.cboTeams);
            this.Controls.Add(this.tabCtrlRoster);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RosterMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modifier un roster";
            this.Load += new System.EventHandler(this.RosterMenu_Load);
            this.tabCtrlRoster.ResumeLayout(false);
            this.tabRoster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeamRoster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrlRoster;
        private System.Windows.Forms.TabPage tabRoster;
        private System.Windows.Forms.TabPage tabTeamsStrength;
        private System.Windows.Forms.ComboBox cboTeams;
        private System.Windows.Forms.TabPage tabTeamOverview;
        private System.Windows.Forms.DataGridView dgvTeamRoster;
    }
}