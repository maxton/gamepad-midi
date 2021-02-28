# GamepadMidi

Use your Rock Band 3 Keytar or Drum Kit as a MIDI controller - wirelessly!
Connect a controller, start the program, select the controller
and the output MIDI device and start shredding.

I suggest installing [LoopBe1](https://www.nerds.de/en/download.html) if you want to use this as a midi controller for e.g. a DAW.

## Keytar
Use the left and right D-pad controlls to switch octaves.

No touchpad support, yet. I can't find touchpad data in any of the Xinput structs.

## Drum kit
The drums are output on MIDI channel 10. Currently the MIDI notes are fixed and assume you have the Pro Cymbals attached.

## Screenshot
![screenshot](https://i.imgur.com/eJGkYzU.png)


## Build Status / Download

Windows (.NET 4.7 exe): [![Windows](https://ci.appveyor.com/api/projects/status/3acux00e2hynqud3?svg=true)](https://ci.appveyor.com/project/maxton/keytar/build/artifacts)
