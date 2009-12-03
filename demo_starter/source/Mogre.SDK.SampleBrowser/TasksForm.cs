using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Mogre.SDK.SampleBrowser
{
    public partial class TasksForm : Form
    {
        private readonly ConfigurationSerializer.Sample _sample;
        private readonly int _origin;

        public TasksForm(ConfigurationSerializer.Sample sample)
        {
            _sample = sample;

            InitializeComponent();

            _origin = _explainRadioButton.Left; // _runRadioButton could also be used here ...

            FillInInformations();
        }

        private void FillInInformations()
        {
            _nameContentLabel.Text = _sample.Name;
            _categoryContentLabel.Text = _sample.Category;
            _descriptionTextBox.Text = _sample.Description;
        }

        private void _radioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            var radioButton = sender is RadioButton ? (RadioButton) sender : null;
            if (radioButton == null) return;

            if (radioButton.Checked)
            {
                for (double i = 0; i < 1; i = i + 0.03d)
                {
                    var eased = Easing.EaseInOut(i, EasingType.Quartic) * 10 + _origin;
                    radioButton.Left = (int)eased;

                    Application.DoEvents();
                    Thread.Sleep(5);
                }

                for (double i = 0; i < 1; i = i + 0.03d)
                {
                    var eased = (_origin + 10) - Easing.EaseInOut(i, EasingType.Quartic) * 10;
                    radioButton.Left = (int)eased;

                    Application.DoEvents();
                    Thread.Sleep(5);
                }
            }
        }

        private void _okButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_runRadioButton.Checked)
                    Process.Start(_sample.ExecutablePath);
                else
                    Process.Start(_sample.TutorialLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "There was an error processing your command. The error returned was:\n\n\"" + ex.Message + "\".",
                    "Error processing command", MessageBoxButtons.OK, MessageBoxIcon.Error);

                DialogResult = DialogResult.Abort;
            }
        }
    }
}
