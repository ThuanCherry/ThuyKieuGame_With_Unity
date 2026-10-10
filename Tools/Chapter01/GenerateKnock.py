"""Original deterministic wood knock, standard library only; no downloads."""
from pathlib import Path
import math
import random
import struct
import wave

rate = 44100
rng = random.Random(4102)
samples = []
for i in range(int(rate * .24)):
    t = i / rate
    hit = math.exp(-t * 42) * (math.sin(2 * math.pi * 165 * t) + .3 * math.sin(2 * math.pi * 390 * t))
    hit += rng.uniform(-1, 1) * math.exp(-t * 160) * .28
    samples.append(int(max(-1, min(1, hit * .6)) * 32767))
path = Path(__file__).resolve().parents[2] / 'Assets/_Game/Audio/Chapter01/DoorKnock.wav'
with wave.open(str(path), 'wb') as audio:
    audio.setparams((1, 2, rate, len(samples), 'NONE', 'not compressed'))
    audio.writeframes(struct.pack('<' + 'h' * len(samples), *samples))
