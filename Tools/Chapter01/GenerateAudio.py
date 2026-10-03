"""Create original deterministic PCM sounds. Requires Python and NumPy; no downloads."""
from pathlib import Path
import wave
import numpy as np

RATE = 44100
OUT = Path(__file__).resolve().parents[2] / "Assets/_Game/Audio/Chapter01"
RNG = np.random.default_rng(4101)


def noise(count, cutoff, highpass=False):
    # Circular spectral filtering keeps ambience seamless at the loop boundary.
    raw = RNG.normal(size=count)
    freq = np.fft.rfftfreq(count, 1 / RATE)
    shape = 1 / np.sqrt(1 + (freq / cutoff) ** 4)
    if highpass:
        shape *= freq / np.sqrt(freq * freq + 180 ** 2)
    result = np.fft.irfft(np.fft.rfft(raw) * shape, n=count)
    return result / max(np.std(result), 1e-8)


def write(name, samples, level=0.6, loop=False):
    samples = np.asarray(samples, dtype=float)
    if not loop:
        fade = min(int(RATE * .008), len(samples) // 2)
        samples[:fade] *= np.linspace(0, 1, fade)
        samples[-fade:] *= np.linspace(1, 0, fade)
    samples *= level / max(np.max(np.abs(samples)), 1e-8)
    with wave.open(str(OUT / (name + ".wav")), "wb") as audio:
        audio.setparams((1, 2, RATE, len(samples), "NONE", "not compressed"))
        audio.writeframes((samples * 32767).astype("<i2").tobytes())


def time(seconds):
    return np.arange(round(RATE * seconds)) / RATE


def generate():
    OUT.mkdir(parents=True, exist_ok=True)
    t = time(12)
    rain = noise(len(t), 5500, True) * (.7 + .08 * np.sin(2 * np.pi * t / 12))
    # Sparse roof droplets over the diffuse rainfall bed.
    for start in RNG.integers(0, len(t), 160):
        drop_t = time(.025)
        drop = np.sin(2 * np.pi * RNG.uniform(1400, 3800) * drop_t) * np.exp(-drop_t * 210)
        indices = (start + np.arange(len(drop))) % len(t)
        rain[indices] += drop * .7
    write("Rain", rain, .72, True)
    wind = noise(len(t), 420) * (.65 + .2 * np.sin(2 * np.pi * t / 6) + .1 * np.sin(2 * np.pi * t / 12))
    write("Wind", wind, .65, True)

    t = time(24)
    music = np.zeros(len(t))
    # A restrained D-minor bowed pad, with a slow D-F-E-D melody.
    for frequency in (146.8324, 174.6141, 220):
        frequency = round(frequency * 24) / 24
        for harmonic in range(1, 7):
            music += np.sin(2 * np.pi * frequency * harmonic * t + .018 * np.sin(2 * np.pi * t / 3)) / harmonic ** 1.8
    music *= .22 * (.85 + .15 * np.cos(2 * np.pi * t / 24))
    for index, frequency in enumerate((293.6648, 349.2282, 329.6276, 293.6648)):
        note_t = time(6)
        envelope = np.sin(np.pi * note_t / 6) ** 2
        note = sum(np.sin(2 * np.pi * frequency * h * note_t) / h ** 2 for h in range(1, 5))
        music[index * len(note_t):(index + 1) * len(note_t)] += note * envelope * .22
    write("SadStrings", music, .5, True)

    for name, closing in (("DoorOpen", False), ("DoorClose", True)):
        t = time(.85)
        creak = np.sin(2 * np.pi * (155 * t + 32 * t * t) + 2 * np.sin(2 * np.pi * 13 * t))
        sound = creak * np.sin(np.pi * t / .85) ** 2 * .17 + noise(len(t), 1800) * np.exp(-t * 13) * .12
        hit_t = np.maximum(t - .65, 0)
        sound += (t >= .65) * np.exp(-hit_t * 38) * (np.sin(2 * np.pi * 95 * hit_t) + noise(len(t), 900) * .3) * (.65 if closing else .2)
        write(name, sound)
    t = time(.65)
    write("Paper", noise(len(t), 7000, True) * np.sin(np.pi * t / .65) ** 2 * (.2 + .8 * np.sin(2 * np.pi * 11 * t) ** 2), .5)
    t = time(.16)
    write("UIConfirm", (np.sin(2 * np.pi * 660 * t) + .35 * np.sin(2 * np.pi * 990 * t)) * np.exp(-t * 35), .35)
    for name, wood in (("WoodStep", True), ("StoneStep", False)):
        t = time(.3)
        impact = np.sin(2 * np.pi * (105 if wood else 180) * t) * np.exp(-t * 35)
        texture = noise(len(t), 1600 if wood else 4200) * np.exp(-t * (32 if wood else 45))
        write(name, .7 * impact + .3 * texture, .7)
    print("Generated 9 original audio clips in", OUT)
    generate_letter()


def generate_letter():
    t = time(.05)
    tick = (np.sin(2 * np.pi * 880 * t) + .25 * np.sin(2 * np.pi * 1760 * t)) * np.exp(-t * 55)
    write("DialogueLetter", tick, .8)


if __name__ == "__main__":
    generate()
