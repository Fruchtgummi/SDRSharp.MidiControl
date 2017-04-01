using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using SDRSharp.Common;
using SDRSharp.Radio;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System.Threading;

namespace SDRSharp.MidiControl
{
    [DesignTimeVisible(true)]
    [Category("SDRSharp")]
    [Description("MidiControl")]
    public partial class MidiView : UserControl
    {

        private const int SysExBufferSize = 128;

        private InputDevice inDevice = null;
        private SynchronizationContext context;


        private ISharpControl _controlInterface;
        public MidiView(ISharpControl control)
        {
            InitializeComponent();
            _controlInterface = control;

        }


        private void lautleise(double pegel)
        {
            var midiEntry = new MidiEntry();

            double range = 34.0;
            double max_in = 127.0;
            double min_scal = 26.0;

            var scal = min_scal + (range / max_in * pegel);

            if (pegel != 0)
            {
                midiEntry.Audiogain = (int)Math.Truncate(scal);
                _controlInterface.AudioGain = midiEntry.Audiogain;
            }
        }

        public void leftVfo(int freq)
        {

            var midiEntry = new MidiEntry();

            midiEntry.Frequency = _controlInterface.Frequency;
            midiEntry.Stepsize = _controlInterface.StepSize;

            if (freq == 1)
            {
                _controlInterface.Frequency = midiEntry.Frequency + midiEntry.Stepsize;
            }
            else
            {
                _controlInterface.Frequency = midiEntry.Frequency - midiEntry.Stepsize;
            }

        }

        public void bandbreite(int pegel)
        {

            var midiEntry = new MidiEntry();

            midiEntry.Bandbreite = _controlInterface.FilterBandwidth;

            if (pegel == 127 && midiEntry.Bandbreite > 10)
            {
                _controlInterface.FilterBandwidth = midiEntry.Bandbreite - 10;
            }
            else if (pegel != 127 && midiEntry.Bandbreite < 32000)
            {
                _controlInterface.FilterBandwidth = midiEntry.Bandbreite + 10;
            }

        }

        public void stepsize(int pegel)
        {

            var midiEntry = new MidiEntry();

            midiEntry.Stepsize = _controlInterface.StepSize;

            if (pegel == 127)
            {

                Console.WriteLine(midiEntry.Stepsize);

            }
            else
            {

            }


        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate (object dummy)
            {

                Console.WriteLine(e.Message.Data1.ToString() + " - " + e.Message.Data2.ToString());

                switch (e.Message.Data1)
                {
                    case 56:
                        lautleise(e.Message.Data2);
                        break;
                    case 48:
                        leftVfo(e.Message.Data2);
                        break;
                    case 49:
                        bandbreite(e.Message.Data2);
                        break;
                    case 50:
                        stepsize(e.Message.Data2);
                        break;
                    default:
                        Console.WriteLine("Kein Funktion für diese Eingabe angelegt!", "Error");
                        break;
                }

            }, null);
        }


        public void OnClosed()
        {
            if (inDevice != null)
            {
                inDevice.Close();
            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {


            if (checkBox1.Checked == true)
            {
                if (InputDevice.DeviceCount == 0)
                {
                    MessageBox.Show("No MIDI input devices available.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    checkBox1.Checked = false;
                }
                else
                {
                    try
                    {
                        context = SynchronizationContext.Current;

                        inDevice = new InputDevice(0);
                        inDevice.ChannelMessageReceived += HandleChannelMessageReceived;

                        inDevice.StartRecording();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    }
                }
            }
            else
            {

                inDevice.StopRecording();
                inDevice.Reset();

            }
        }
    }
}
