using System.ComponentModel;
using System.Windows.Forms;

namespace Mogre.SDK.SampleBrowser
{
    partial class BrowserForm
    {
        // ReSharper disable InconsistentNaming
        const int WM_KEYDOWN = 0x100;
        // ReSharper restore InconsistentNaming

        private readonly Container _components;
        private PictureBox _logoPictureBox;
        private GroupBox _videoOptionsGroup;
        private ListBox _samplesListBox;
        private Button _cancelButton;
        private Button _okButton;
        private Panel _backgroundPanel;
        private PictureBox _previewPictureBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            this._videoOptionsGroup = new System.Windows.Forms.GroupBox();
            this._previewPictureBox = new System.Windows.Forms.PictureBox();
            this._samplesListBox = new System.Windows.Forms.ListBox();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._backgroundPanel = new System.Windows.Forms.Panel();
            this._loadSamplesWorker = new System.ComponentModel.BackgroundWorker();
            this._progressIndicator = new System.Windows.Forms.PictureBox();
            this._logoPictureBox = new System.Windows.Forms.PictureBox();
            this._videoOptionsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._previewPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._progressIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _videoOptionsGroup
            // 
            this._videoOptionsGroup.Controls.Add(this._previewPictureBox);
            this._videoOptionsGroup.Controls.Add(this._samplesListBox);
            this._videoOptionsGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._videoOptionsGroup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._videoOptionsGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(75)))));
            this._videoOptionsGroup.Location = new System.Drawing.Point(12, 183);
            this._videoOptionsGroup.Name = "_videoOptionsGroup";
            this._videoOptionsGroup.Size = new System.Drawing.Size(420, 159);
            this._videoOptionsGroup.TabIndex = 6;
            this._videoOptionsGroup.TabStop = false;
            this._videoOptionsGroup.Text = "Samples";
            // 
            // _previewPictureBox
            // 
            this._previewPictureBox.Location = new System.Drawing.Point(216, 22);
            this._previewPictureBox.Name = "_previewPictureBox";
            this._previewPictureBox.Size = new System.Drawing.Size(198, 124);
            this._previewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._previewPictureBox.TabIndex = 1;
            this._previewPictureBox.TabStop = false;
            // 
            // _samplesListBox
            // 
            this._samplesListBox.ItemHeight = 15;
            this._samplesListBox.Location = new System.Drawing.Point(7, 22);
            this._samplesListBox.Name = "_samplesListBox";
            this._samplesListBox.Size = new System.Drawing.Size(203, 124);
            this._samplesListBox.TabIndex = 0;
            this._samplesListBox.SelectedIndexChanged += new System.EventHandler(this._samplesListBox_SelectedIndexChanged);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._cancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._cancelButton.Location = new System.Drawing.Point(357, 348);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 10;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _okButton
            // 
            this._okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Enabled = false;
            this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._okButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._okButton.Location = new System.Drawing.Point(276, 348);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 11;
            this._okButton.Text = "&OK";
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _backgroundPanel
            // 
            this._backgroundPanel.BackColor = System.Drawing.Color.White;
            this._backgroundPanel.Location = new System.Drawing.Point(-2, 3);
            this._backgroundPanel.Name = "_backgroundPanel";
            this._backgroundPanel.Size = new System.Drawing.Size(446, 174);
            this._backgroundPanel.TabIndex = 12;
            // 
            // _loadSamplesWorker
            // 
            this._loadSamplesWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._loadSamplesWorker_DoWork);
            this._loadSamplesWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this._loadSamplesWorker_RunWorkerCompleted);
            // 
            // _progressIndicator
            // 
            this._progressIndicator.Image = global::Mogre.SDK.SampleBrowser.Properties.Resources.loading_samples;
            this._progressIndicator.Location = new System.Drawing.Point(12, 355);
            this._progressIndicator.Name = "_progressIndicator";
            this._progressIndicator.Size = new System.Drawing.Size(16, 16);
            this._progressIndicator.TabIndex = 13;
            this._progressIndicator.TabStop = false;
            // 
            // _logoPictureBox
            // 
            this._logoPictureBox.BackColor = System.Drawing.Color.White;
            this._logoPictureBox.Image = global::Mogre.SDK.SampleBrowser.Properties.Resources.mogre_banner;
            this._logoPictureBox.Location = new System.Drawing.Point(0, 3);
            this._logoPictureBox.Name = "_logoPictureBox";
            this._logoPictureBox.Size = new System.Drawing.Size(442, 174);
            this._logoPictureBox.TabIndex = 3;
            this._logoPictureBox.TabStop = false;
            // 
            // BrowserForm
            // 
            this.ClientSize = new System.Drawing.Size(442, 380);
            this.ControlBox = false;
            this.Controls.Add(this._progressIndicator);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._videoOptionsGroup);
            this.Controls.Add(this._logoPictureBox);
            this.Controls.Add(this._backgroundPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BrowserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MOGRE Sample Browser";
            this.Load += new System.EventHandler(this.ConfigDialog_Load);
            this._videoOptionsGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._previewPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._progressIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        private BackgroundWorker _loadSamplesWorker;
        private PictureBox _progressIndicator;
    }
}