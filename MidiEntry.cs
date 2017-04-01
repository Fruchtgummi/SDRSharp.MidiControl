using System;
using SDRSharp.Radio;

namespace SDRSharp.MidiControl
{
    public class MidiEntry
    {
        private DetectorType _detectorType;
        private long _frequency;
        private int _stepsize;
        private int _audiogain;
        private int _bandbreite;


        public MidiEntry() { }


        public MidiEntry(MidiEntry midiEntry)
        {
            _frequency = midiEntry._frequency;
            _stepsize = midiEntry._stepsize;
            _audiogain = midiEntry._audiogain;
            _bandbreite = midiEntry._bandbreite;
            _detectorType = midiEntry._detectorType;

        }

        public DetectorType DetectorType
        {
            get { return _detectorType; }
            set { _detectorType = value; }
        }

        public int Bandbreite
        {
            get { return _bandbreite; }
            set { _bandbreite = value; }
        }

        public long Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public int Stepsize
        {
            get { return _stepsize; }
            set { _stepsize = value; }
        }

        public int Audiogain
        {
            get { return _audiogain; }
            set { _audiogain = value; }
        }

    }
}
