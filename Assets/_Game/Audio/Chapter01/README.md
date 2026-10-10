# Chapter 1 audio

Eleven original synthesized mono PCM WAV clips, 44.1 kHz / 16 bit.
Created locally for this project; no recordings, downloads, samples or external licenses.
These are designed effects and a synthesized bowed pad, not field recordings or voice acting.

- SadStrings: 24-second restrained D-minor pad and melody.
- Rain / Wind: 12-second cyclic ambience.
- DoorOpen / DoorClose: wooden creak and latch/impact.
- DoorKnock: restrained wooden impact; the story schedules single/double knocks.
- Paper: short parchment rustle.
- UIConfirm: soft confirmation tone.
- DialogueLetter: clear 50 ms tick during progressive dialogue text reveal, with Inspector volume control on Chapter01Audio.
- WoodStep / StoneStep: short surface-specific impacts.

Reproduce with `python Tools/Chapter01/GenerateAudio.py` (NumPy required).
Reproduce DoorKnock with `python Tools/Chapter01/GenerateKnock.py` (standard library only).
Unity does not require Python or NumPy to play the exported files.
With Chapter01_GiaBien open in Edit Mode, use
`ThuyKieu > Chapter 1 > Assign audio` to restore the clip assignments.
Source volumes and 2D/3D settings follow CHAPTER1_ENVIRONMENT_IMPLEMENTATION.
The rework applies softer bedroom rain and quieter dialogue letter ticks. Use
`ThuyKieu > Chapter 1 > Apply story rework` to restore the knock assignment.
There is no recorded crying clip; its cue warns once and leaves the Ink narration
and rain audible. No synthesized sound is presented as recorded human crying.
