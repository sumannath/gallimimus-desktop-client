
namespace Gallimimus
{
    partial class frmMainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainWindow));
            this.lblProgress = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.dtGridApps = new System.Windows.Forms.DataGridView();
            this.cmdClose = new System.Windows.Forms.Button();
            this.lblShowHide = new System.Windows.Forms.LinkLabel();
            this.lblFeedback = new System.Windows.Forms.LinkLabel();
            this.colApplication = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridApps)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(24, 24);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(70, 20);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "Starting...";
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(24, 56);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(737, 29);
            this.pbProgress.TabIndex = 1;
            // 
            // dtGridApps
            // 
            this.dtGridApps.AllowUserToAddRows = false;
            this.dtGridApps.AllowUserToDeleteRows = false;
            this.dtGridApps.AllowUserToResizeColumns = false;
            this.dtGridApps.AllowUserToResizeRows = false;
            this.dtGridApps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtGridApps.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dtGridApps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridApps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colApplication,
            this.Version,
            this.colStatus});
            this.dtGridApps.GridColor = System.Drawing.SystemColors.Control;
            this.dtGridApps.Location = new System.Drawing.Point(24, 147);
            this.dtGridApps.Name = "dtGridApps";
            this.dtGridApps.ReadOnly = true;
            this.dtGridApps.RowHeadersWidth = 51;
            this.dtGridApps.RowTemplate.Height = 29;
            this.dtGridApps.Size = new System.Drawing.Size(737, 152);
            this.dtGridApps.TabIndex = 2;
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(667, 99);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(94, 29);
            this.cmdClose.TabIndex = 3;
            this.cmdClose.Text = "&Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // lblShowHide
            // 
            this.lblShowHide.AutoSize = true;
            this.lblShowHide.Location = new System.Drawing.Point(24, 103);
            this.lblShowHide.Name = "lblShowHide";
            this.lblShowHide.Size = new System.Drawing.Size(95, 20);
            this.lblShowHide.TabIndex = 4;
            this.lblShowHide.TabStop = true;
            this.lblShowHide.Text = "Show Details";
            this.lblShowHide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblShowHide_LinkClicked);
            // 
            // lblFeedback
            // 
            this.lblFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFeedback.AutoSize = true;
            this.lblFeedback.Location = new System.Drawing.Point(535, 103);
            this.lblFeedback.Name = "lblFeedback";
            this.lblFeedback.Size = new System.Drawing.Size(126, 20);
            this.lblFeedback.TabIndex = 5;
            this.lblFeedback.TabStop = true;
            this.lblFeedback.Text = "Provide Feedback";
            this.lblFeedback.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblFeedback_LinkClicked);
            // 
            // colApplication
            // 
            this.colApplication.DataPropertyName = "Name";
            this.colApplication.HeaderText = "Application";
            this.colApplication.MinimumWidth = 6;
            this.colApplication.Name = "colApplication";
            this.colApplication.ReadOnly = true;
            this.colApplication.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colApplication.Width = 300;
            // 
            // Version
            // 
            this.Version.DataPropertyName = "Version";
            this.Version.HeaderText = "Version";
            this.Version.MinimumWidth = 6;
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            this.Version.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Version.Width = 125;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStatus.Width = 250;
            // 
            // frmMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(788, 317);
            this.Controls.Add(this.lblFeedback);
            this.Controls.Add(this.lblShowHide);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.dtGridApps);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.lblProgress);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMainWindow";
            this.Text = "Gallimimus";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridApps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.DataGridView dtGridApps;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.LinkLabel lblShowHide;
        private System.Windows.Forms.LinkLabel lblFeedback;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApplication;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}

