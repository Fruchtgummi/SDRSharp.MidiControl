using System.Windows.Forms;
using SDRSharp.Common;

namespace SDRSharp.MidiControl
{
    public class MidiControlPlugin : ISharpPlugin
    {

        private const string _displayName = "Midicontrol";
        private ISharpControl _control;
        private MidiView _guiControl;

        public string DisplayName
        {
            get { return _displayName; }
        }

        public bool HasGui
        {
            get { return true; }
        }

        public UserControl Gui
        {
            get { return _guiControl; }
        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
            _guiControl = new MidiView(_control);
           
        }


        public void Close()
        {
            _guiControl.OnClosed();
        }

    }
}
