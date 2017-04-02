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
        private int steppi = 0; //left bar steps: 1 = 1hz; 2= 1hz; 3= 100hz; 4= 1Khz; 5= stepsize from gui

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
            long step = 1;
            midiEntry.Frequency = _controlInterface.Frequency;
            midiEntry.Stepsize = _controlInterface.StepSize;


            switch (steppi)
            {
                case 1:
                    step = 1;
                    break;
                case 2:
                    step = 100;
                    break;
                case 3:
                    step = 1000;
                    break;
                case 4:
                    step = 10000;
                    break;
                case 5:
                    step = midiEntry.Stepsize;
                    break;
                case 6:
                    step = midiEntry.Stepsize;
                    break;
                default:
                    step = midiEntry.Stepsize;
                    break;
            }

            if (freq == 1)
            {
                _controlInterface.Frequency = midiEntry.Frequency + step;
            }
            else
            {
                _controlInterface.Frequency = midiEntry.Frequency - step;
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

        public void fineTunigFreq(int pegel)
        {

            var midiEntry = new MidiEntry();

            midiEntry.Frequency = _controlInterface.Frequency;

            if (pegel == 127)
            {
                _controlInterface.Frequency = midiEntry.Frequency - 1;
            }
            else
            {
                _controlInterface.Frequency = midiEntry.Frequency + 1;
            }


        }

        public void bandType(int pegel)
        {
            var midiEntry = new MidiEntry();

            midiEntry.DetectorType = _controlInterface.DetectorType;

            if (pegel == 1)
            {

                switch (midiEntry.DetectorType)
                {
                    case DetectorType.NFM:
                        _controlInterface.DetectorType = DetectorType.AM;
                        break;
                    case DetectorType.AM:
                        _controlInterface.DetectorType = DetectorType.LSB;
                        break;
                    case DetectorType.LSB:
                        _controlInterface.DetectorType = DetectorType.USB;
                        break;
                    case DetectorType.USB:
                        _controlInterface.DetectorType = DetectorType.WFM;
                        break;
                    case DetectorType.WFM:
                        _controlInterface.DetectorType = DetectorType.DSB;
                        break;
                    case DetectorType.DSB:
                        _controlInterface.DetectorType = DetectorType.CW;
                        break;
                    case DetectorType.CW:
                        _controlInterface.DetectorType = DetectorType.RAW;
                        break;
                    case DetectorType.RAW:
                        _controlInterface.DetectorType = DetectorType.NFM;
                        break;
                }

            }
            else
            {
                switch (midiEntry.DetectorType)
                {
                    case DetectorType.NFM:
                        _controlInterface.DetectorType = DetectorType.RAW;
                        break;
                    case DetectorType.RAW:
                        _controlInterface.DetectorType = DetectorType.CW;
                        break;
                    case DetectorType.CW:
                        _controlInterface.DetectorType = DetectorType.DSB;
                        break;
                    case DetectorType.DSB:
                        _controlInterface.DetectorType = DetectorType.WFM;
                        break;
                    case DetectorType.WFM:
                        _controlInterface.DetectorType = DetectorType.USB;
                        break;
                    case DetectorType.USB:
                        _controlInterface.DetectorType = DetectorType.LSB;
                        break;
                    case DetectorType.LSB:
                        _controlInterface.DetectorType = DetectorType.AM;
                        break;
                    case DetectorType.AM:
                        _controlInterface.DetectorType = DetectorType.NFM;
                        break;
                }
            }


        }

        private void startStop(int pegel)
        {

            var midiEntry = new MidiEntry();

            midiEntry.Startstop = _controlInterface.IsPlaying;

            if (pegel == 127)
            {

                if (midiEntry.Startstop == true)
                {
                    _controlInterface.StopRadio();
                }
                else
                {
                    _controlInterface.StartRadio();
                }
            }
        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate (object dummy)
            {

                // Console.WriteLine(e.Message.Data1.ToString()+" - "+ e.Message.Data2.ToString());

                if (e.Message.Data1 == 52)
                {

                    double range = 5;
                    double max_in = 127.0;
                    double min_scal = 1;

                    var scal = min_scal + (range / max_in * e.Message.Data2);


                    steppi = (int)scal;
                }

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
                        fineTunigFreq(e.Message.Data2);
                        break;
                    case 51:
                        bandType(e.Message.Data2);
                        break;
                    case 19:
                        startStop(e.Message.Data2);
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
                        checkBox1.Checked = false;
                    }
                }
            }
            else
            {

                inDevice.StopRecording();
                inDevice.Reset();
                inDevice.Close();

            }
        }
    }
}

