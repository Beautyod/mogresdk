#define TEST_SERIALIZER

using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Mogre.SDK.SampleBrowser
{
    // TODO: Category sorting could be useful
    public partial class BrowserForm : Form, IMessageFilter
    {
        private Stream _previewImageStream;
        private readonly ConfigurationSerializer _serializer;

        public BrowserForm()
        {
            SetStyle( ControlStyles.DoubleBuffer, true );
            InitializeComponent();

            _serializer = new ConfigurationSerializer();
        }

        protected void _buttonOk_Click( object sender, EventArgs e )
        {
            if (_samplesListBox.SelectedItem != null)
                if (new TasksForm((ConfigurationSerializer.Sample) _samplesListBox.SelectedItem).ShowDialog() == DialogResult.OK)
                    Close();
        }

        protected void _buttonCancel_Click( object sender, EventArgs e )
        {
            Dispose();
        }

        private void ConfigDialog_Load( object sender, EventArgs e )
        {
            // Register [Enter] and [Esc] keys for Default buttons
            _okButton.NotifyDefault( true );
            Application.AddMessageFilter(this);

            PreviewImageStream = null;
            PopulateSampleList();
        }

        public Stream PreviewImageStream
        {
            get
            {
                return _previewImageStream;
            }
            set
            {
                using (_previewImageStream = value ?? new FileStream("image_not_available.jpg", FileMode.Open))
                {
                    _previewPictureBox.Image = Image.FromStream(_previewImageStream);
                }
            }
        }

        public void PopulateSampleList()
        {
            _samplesListBox.Items.Clear();

            try
            {
                _samplesListBox.Items.AddRange(_serializer.Deserialize("SampleBrowser.xml"));
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "An error occurred while reading the configuration file.\nPlease fix it before running this application again.",
                    "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Dispose();
            }
        }

        private void _samplesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sample = _samplesListBox.SelectedItem as ConfigurationSerializer.Sample;
            if (sample == null) return;

            try
            {
                // Get preview image stream using the selected item
                PreviewImageStream = new FileStream(sample.PreviewImagePath, FileMode.Open);
            }
            catch
            {
                // Reset preview image stream
                PreviewImageStream = null;
            }
        }

        #region IMessageFilter Members

        public bool PreFilterMessage( ref Message msg )
        {
            var keyCode = (Keys)(int)msg.WParam & Keys.KeyCode;
            if ( msg.Msg == WM_KEYDOWN && keyCode == Keys.Return )
            {
                _buttonOk_Click( this, null );
                return true;
            }
            if ( msg.Msg == WM_KEYDOWN && keyCode == Keys.Escape )
            {
                _buttonCancel_Click( this, null );
                return true;
            }
            return false;
        }
        #endregion
    }
}