SDRSharp.MidiControl Plugin for SDRSharp
====================

A plugin for SDRSharp. For more informationen & download by [airspy.com](http://airspy.com/download/) It's free! 
You can use the SDRSharp GUI with the Hercules DJControls. 

# Install

Copy SDRSharp.MidiControl.dll and Toolkit dlls in your SDRSharp directory, edit plugins.xml and add on block: 

```<sharpPlugins>``` 

this line

```
    <add key="Midi Control" value="SDRSharp.MidiControl.MidiControlPlugin,SDRSharp.MidiControl" />
```

Test on SDRSharp version 1.0.0.1491

### Features - currently works:

- DeckA is VFO (turn-speed set bandwidth )
- DeckB is Bandwidth
- Volume (AudioGain)
- Fine-Tuning (speed of DeckA set stepsize)
- Radio on/of with Pich-Reset
- Change Band-Type with Right-Pitch

### Next

- table for own configuration to assign knobs, panels and trackbars.
- config-templates for different control hardware.


## Development 
Basic can be found on http://www.andrej-mohar.com/plugin-basics-for-sdr


### Do you need:

## Sanford.MidiToolkit

A toolkit for creating MIDI applications.
Based on Leslie Sanford's project hosted on http://www.codeproject.com/Articles/6228/C-MIDI-Toolkit
and https://github.com/aruss/Sanford.MidiToolkit

more:

- NET. 3.5

### Supported Systems

- Windows x86 (32-bit)


Viel Spaß!
