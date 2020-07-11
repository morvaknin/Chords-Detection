# Chords-Detection
Self Learning Guitar Playing App

#About the app:
The purpse of the app is to let unexpirenced users learn how to play the guitar by themself.
The app recognize chords and check if the user played the chored correctly and gives him a feedback.
There are 2 levels: 'Begginer mode' and 'Expert mode'
*Begginer:
In the begginer level the user needs to follow the chords showing on the screen.
The chord won't change until the user play the chord correctly, then the chord will turn green, and continue to the next chord.
*Expert:
In the expert level the user will choose a song he wants to play, the chords will change automatically according to the song tempo.
The user will have to hit the chords correctly in live time. The app will return a feedback, green if the user played it right and red if they didn't,
the chords will keep changing regardless to the user success or failure.

#How it works:
*Workspace:
The C# based interface runs a python script in another thread which listens to the user input and analyzes the input to figure which chord the user was playing.
the output of the python progrem redirects to the C# interface where only close enugh comparison (in the treshhold limit) includes as a right answer.
The user will get the feedback from the app after the comparison in real time.

#The Algorithem:
We recorded samples of all the chords and labled them by the chord name. Every chord represented in the frequency space,
after FFT (Fast Fourier transform). Every input from the user will be analyzed by FFT and then compered the chords recorded and labled as the samples.
the algorithem returns an indication to how much the input is similar the samples and then we check if the input chord is close enugh to
the chord the user supposed to play according to the trshhold we determined after trial and error.