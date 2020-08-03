# Chords-Detection
Self Learning Guitar Playing App

## About the app:
The purpose of the app is to let inexperienced users learn how to play the guitar by themself.
The app recognizes chords and checks if the user played the chored correctly and provide him with feedback.
2 levels exist: 'Beginner mode' and 'Expert mode'
- Beginner: At the beginner level the user needs to follow the chords shown on the screen.
The chord won't change until the user plays the chord correctly, then the chord will turn green, and continue to the next chord.
- Expert: At the expert level the user will choose a song he wants to play, the chords will change automatically according to the song tempo.
The user will have to hit the chords correctly in live time. The app will return feedback, green if the user played it right and red if they didn't,
the chords will keep changing regardless of the user’s success or failure.

## How it works:
Workspace: The C# based interface runs a python script in another thread which listens to the user input and analyzes the input to figure which chord the user was playing.
The output of the python (workes with Python 2.7) program redirects to the C# interface where only close enough comparison (in the threshold limit) includes the right answer.
The user will get the feedback from the app after the comparison in real-time.

## The Algorithm:
We recorded samples of all the chords and labeled them by the chord name. Every chord represented in the frequency space,
after FFT (Fast Fourier transform). Every input from the user will be analyzed by FFT and then compared to the chords recorded and labeled as the samples.
the algorithm returns an indication to how much the input is similar to the samples and then we check if the input chord is close enough to
the chord the user supposed to play according to the threshold we determined after trial and error.
