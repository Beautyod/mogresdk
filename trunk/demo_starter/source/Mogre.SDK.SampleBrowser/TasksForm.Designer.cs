namespace Mogre.SDK.SampleBrowser
{
    partial class TasksForm
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
            this._questionLabel = new System.Windows.Forms.Label();
            this._tasksBox = new System.Windows.Forms.GroupBox();
            this._wishLabel = new System.Windows.Forms.Label();
            this._runRadioButton = new System.Windows.Forms.RadioButton();
            this._explainRadioButton = new System.Windows.Forms.RadioButton();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._informationsBox = new System.Windows.Forms.GroupBox();
            this._categoryContentLabel = new System.Windows.Forms.Label();
            this._categoryLabel = new System.Windows.Forms.Label();
            this._descriptionTextBox = new System.Windows.Forms.TextBox();
            this._descriptionLabel = new System.Windows.Forms.Label();
            this._nameContentLabel = new System.Windows.Forms.Label();
            this._nameLabel = new System.Windows.Forms.Label();
            this._tasksBox.SuspendLayout();
            this._informationsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _questionLabel
            // 
            this._questionLabel.AutoSize = true;
            this._questionLabel.Location = new System.Drawing.Point(5, 5);
            this._questionLabel.Name = "_questionLabel";
            this._questionLabel.Size = new System.Drawing.Size(164, 13);
            this._questionLabel.TabIndex = 0;
            this._questionLabel.Text = "What do you want to do next?";
            // 
            // _tasksBox
            // 
            this._tasksBox.Controls.Add(this._wishLabel);
            this._tasksBox.Controls.Add(this._runRadioButton);
            this._tasksBox.Controls.Add(this._explainRadioButton);
            this._tasksBox.Location = new System.Drawing.Point(8, 127);
            this._tasksBox.Name = "_tasksBox";
            this._tasksBox.Size = new System.Drawing.Size(356, 83);
            this._tasksBox.TabIndex = 1;
            this._tasksBox.TabStop = false;
            this._tasksBox.Text = "Tasks";
            // 
            // _wishLabel
            // 
            this._wishLabel.AutoSize = true;
            this._wishLabel.Location = new System.Drawing.Point(6, 18);
            this._wishLabel.Name = "_wishLabel";
            this._wishLabel.Size = new System.Drawing.Size(51, 13);
            this._wishLabel.TabIndex = 2;
            this._wishLabel.Text = "I want ...";
            // 
            // _runRadioButton
            // 
            this._runRadioButton.AutoSize = true;
            this._runRadioButton.Location = new System.Drawing.Point(9, 57);
            this._runRadioButton.Name = "_runRadioButton";
            this._runRadioButton.Size = new System.Drawing.Size(128, 17);
            this._runRadioButton.TabIndex = 1;
            this._runRadioButton.TabStop = true;
            this._runRadioButton.Text = "... to run the sample";
            this._runRadioButton.UseVisualStyleBackColor = true;
            this._runRadioButton.CheckedChanged += new System.EventHandler(this._radioButton_CheckedChanged);
            // 
            // _explainRadioButton
            // 
            this._explainRadioButton.AutoSize = true;
            this._explainRadioButton.Location = new System.Drawing.Point(9, 34);
            this._explainRadioButton.Name = "_explainRadioButton";
            this._explainRadioButton.Size = new System.Drawing.Size(180, 17);
            this._explainRadioButton.TabIndex = 0;
            this._explainRadioButton.TabStop = true;
            this._explainRadioButton.Text = "... to get the sample explained";
            this._explainRadioButton.UseVisualStyleBackColor = true;
            this._explainRadioButton.CheckedChanged += new System.EventHandler(this._radioButton_CheckedChanged);
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(208, 216);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 2;
            this._okButton.Text = "&OK";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(289, 216);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _informationsBox
            // 
            this._informationsBox.Controls.Add(this._categoryContentLabel);
            this._informationsBox.Controls.Add(this._categoryLabel);
            this._informationsBox.Controls.Add(this._descriptionTextBox);
            this._informationsBox.Controls.Add(this._descriptionLabel);
            this._informationsBox.Controls.Add(this._nameContentLabel);
            this._informationsBox.Controls.Add(this._nameLabel);
            this._informationsBox.Location = new System.Drawing.Point(8, 21);
            this._informationsBox.Name = "_informationsBox";
            this._informationsBox.Size = new System.Drawing.Size(356, 100);
            this._informationsBox.TabIndex = 4;
            this._informationsBox.TabStop = false;
            this._informationsBox.Text = "Informations";
            // 
            // _categoryContentLabel
            // 
            this._categoryContentLabel.AutoSize = true;
            this._categoryContentLabel.Location = new System.Drawing.Point(81, 31);
            this._categoryContentLabel.Name = "_categoryContentLabel";
            this._categoryContentLabel.Size = new System.Drawing.Size(26, 13);
            this._categoryContentLabel.TabIndex = 10;
            this._categoryContentLabel.Text = "N/A";
            // 
            // _categoryLabel
            // 
            this._categoryLabel.AutoSize = true;
            this._categoryLabel.Location = new System.Drawing.Point(6, 31);
            this._categoryLabel.Name = "_categoryLabel";
            this._categoryLabel.Size = new System.Drawing.Size(56, 13);
            this._categoryLabel.TabIndex = 9;
            this._categoryLabel.Text = "Category:";
            // 
            // _descriptionTextBox
            // 
            this._descriptionTextBox.BackColor = System.Drawing.SystemColors.Control;
            this._descriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._descriptionTextBox.Location = new System.Drawing.Point(81, 44);
            this._descriptionTextBox.Multiline = true;
            this._descriptionTextBox.Name = "_descriptionTextBox";
            this._descriptionTextBox.ReadOnly = true;
            this._descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._descriptionTextBox.Size = new System.Drawing.Size(265, 45);
            this._descriptionTextBox.TabIndex = 8;
            this._descriptionTextBox.Text = "N/A";
            // 
            // _descriptionLabel
            // 
            this._descriptionLabel.AutoSize = true;
            this._descriptionLabel.Location = new System.Drawing.Point(6, 44);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new System.Drawing.Size(69, 13);
            this._descriptionLabel.TabIndex = 7;
            this._descriptionLabel.Text = "Description:";
            // 
            // _nameContentLabel
            // 
            this._nameContentLabel.AutoSize = true;
            this._nameContentLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._nameContentLabel.Location = new System.Drawing.Point(81, 18);
            this._nameContentLabel.Name = "_nameContentLabel";
            this._nameContentLabel.Size = new System.Drawing.Size(29, 13);
            this._nameContentLabel.TabIndex = 6;
            this._nameContentLabel.Text = "N/A";
            // 
            // _nameLabel
            // 
            this._nameLabel.AutoSize = true;
            this._nameLabel.Location = new System.Drawing.Point(6, 18);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(39, 13);
            this._nameLabel.TabIndex = 5;
            this._nameLabel.Text = "Name:";
            // 
            // TasksForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(376, 248);
            this.Controls.Add(this._informationsBox);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._tasksBox);
            this.Controls.Add(this._questionLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TasksForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Selected sample";
            this._tasksBox.ResumeLayout(false);
            this._tasksBox.PerformLayout();
            this._informationsBox.ResumeLayout(false);
            this._informationsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _questionLabel;
        private System.Windows.Forms.GroupBox _tasksBox;
        private System.Windows.Forms.RadioButton _runRadioButton;
        private System.Windows.Forms.RadioButton _explainRadioButton;
        private System.Windows.Forms.Label _wishLabel;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.GroupBox _informationsBox;
        private System.Windows.Forms.TextBox _descriptionTextBox;
        private System.Windows.Forms.Label _descriptionLabel;
        private System.Windows.Forms.Label _nameContentLabel;
        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.Label _categoryContentLabel;
        private System.Windows.Forms.Label _categoryLabel;
    }
}