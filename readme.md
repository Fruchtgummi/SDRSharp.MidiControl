SDRSharp.MidiControl Plugin for SDRSharp
====================

A plugin for SDRSharp. For more informationen & download by [airspy.com](http://airspy.com/download/) It's free! 
You can use the SDRSharp GUI with the Hercules DJControls. 

# Install

Copy SDRSharp.MidiControl.dll and Toolkit in your SDRSharp directory and edit plugins.xml and add on block: 

```<sharpPlugins>``` 

```
    <add key="Midi Control" value="SDRSharp.MidiControl.MidiControlPlugin,SDRSharp.MidiControl" />
```


### Features - currently works:

- DeckA is VFO
- DeckB is Bandwidth
- Volume (AudioGain)

### Next

- StepSize
- Start/stop SDR
- Select RadioType


## Development 
Basic can be found on http://www.andrej-mohar.com/plugin-basics-for-sdr


### Do you need:

## Sanford.MidiToolkit

A toolkit for creating MIDI applications.
Based on Leslie Sanford's project hosted on http://www.codeproject.com/Articles/6228/C-MIDI-Toolkit
and https://github.com/aruss/Sanford.MidiToolkit

more:

- NET. 3.5
- SRDLL.dll http://pe0fko.nl/CFGSR/ 

### Supported Systems

- Windows x86 (32-bit)


Viel Spaß!
