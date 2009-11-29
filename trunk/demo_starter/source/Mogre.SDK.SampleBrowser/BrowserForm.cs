using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Mogre.SDK.SampleBrowser
{
    public partial class BrowserForm : Form, IMessageFilter
    {
        private Stream _previewImageStream;

        public BrowserForm()
        {
            SetStyle( ControlStyles.DoubleBuffer, true );
            InitializeComponent();
        }

        protected void _buttonOk_Click( object sender, EventArgs e )
        {
            // TODO: Run demo

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
            // TODO: Setup browser form
        }

        public Stream PreviewImageStream
        {
            get
            {
                return _previewImageStream;
            }
            set
            {
                _previewImageStream = value ?? new FileStream("image_not_available.jpg", FileMode.Open);
                _previewPictureBox.Image = System.Drawing.Image.FromStream(_previewImageStream);
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