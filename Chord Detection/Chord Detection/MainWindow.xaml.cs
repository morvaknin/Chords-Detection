using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace Chord_Detection
{
    public enum Level
    {
        Beginner, Experts
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Level _level = Level.Beginner;
        private int i = 0;
        private Process myProcess;
        private string _theLableText = "";
        private string[] AllChordsArray = { "A_m", "A", "B_m", "B", "C", "C_m", "D", "D_m", "E", "E_m", "F", "F_m", "G", "G_m" };
        private string[] OneDayArray = { "C", "G", "A_m", "F" };
        private string[] CurrentChords;
        private int currentChordIndex = 0;
        private int chordsIntervalSeconds = 4;
        private int current_chordsIntervalSeconds = 0;
        private string expectedChord;
        private bool isRecognized;
        public string TheLableText
        {
            get { return _theLableText; }
            set
            {
                _theLableText = value;
                OnPropertyChanged("TheLableText");
            }
        }
        public bool isStartd = false;

        private System.Windows.Threading.DispatcherTimer chordsTimer;

        public MainWindow()
        {
            InitializeComponent();
            chordsTimer = new System.Windows.Threading.DispatcherTimer();
            metroRight.Visibility = Visibility.Visible;
            metroLeft.Visibility = Visibility.Collapsed;
            songs_box.SelectedIndex = 0;
            CurrentChords = AllChordsArray;
            expectedChord = CurrentChords[currentChordIndex];
            chordsTimer.Tick += programTimer_Tick;
            chordsTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                chord_label.Content = expectedChord;
                chord_label.Background = Brushes.White;
                var path = "pics/chords_letters/" + expectedChord + ".png";
                chord_image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                currentChordIndex++;
            });
            StartButtonText.Text = "Start!";
            BeginnerRadioButton.IsChecked = true;
        }

        private void programTimer_Tick(object sender, EventArgs e)
        {
            metroRight.Visibility = metroRight.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            metroLeft.Visibility = metroLeft.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            if (_level == Level.Experts && (current_chordsIntervalSeconds % chordsIntervalSeconds) == 0)
            {
                isRecognized = false;
                moveToNextChord();
            }

            if (((current_chordsIntervalSeconds + 1) % chordsIntervalSeconds == 0) && (!isRecognized))
            {
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    chord_label.Content = expectedChord;
                    chord_label.Background = Brushes.Red;
                });
            }
            current_chordsIntervalSeconds++;
        }

        private void moveToNextChord()
        {
            expectedChord = CurrentChords[currentChordIndex % CurrentChords.Length];
            Dispatcher.BeginInvoke((Action)delegate ()
           {
               chord_label.Content = expectedChord;
               chord_label.Background = Brushes.White;
               var path = "pics/chords_letters/" + expectedChord + ".png";
               chord_image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
               currentChordIndex++;
           });
        }

        private void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            // Create new process start info 
            var myProcessStartInfo = new ProcessStartInfo();
            myProcessStartInfo.FileName = @"C:\Python27\python.exe ";
            myProcessStartInfo.Arguments = @"C:\Chords_master\chords.py";

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.RedirectStandardError = true;
            myProcessStartInfo.CreateNoWindow = true;

            var process = new Process();
            process.StartInfo = myProcessStartInfo;
            process.OutputDataReceived += proc_OutputDataReceived;
            process.ErrorDataReceived += proc_ErrorDataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            
            if (!isStartd)
            {
                StartButtonText.Text = "Stop!";
                isStartd = true;
                chordsTimer.Start();
            }
            else
            {
                StartButtonText.Text = "Start!";
                isStartd = false;
                chordsTimer.Stop();
                currentChordIndex = 0;
                moveToNextChord();
            }

        }

        void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var chordStr = e.Data;
            var expectedChord = CurrentChords[currentChordIndex % CurrentChords.Length];
            var isMached = chordStr == this.expectedChord;
            if (isMached)
            {
                isRecognized = true;
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    chord_label.Background = Brushes.Green;
                    if (_level == Level.Beginner)
                    {
                        moveToNextChord();
                    }
                });
            }
            //Dispatcher.BeginInvoke((Action)delegate ()
            //{
            //    updateLable(chordStr);
            //});
        }

        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //updateLable(e.Data);
        }

        private void updateLable(string str)
        {
            if (str != null)
            {
                //Dispatcher.BeginInvoke((Action)delegate ()
                //{
                //    TheLable.Content = str;
                //});

                //TheLableText = str;
                //OnPropertyChanged("TheLable");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Expert_button_OnClick(object sender, RoutedEventArgs e)
        {
            _level = Level.Experts;
            songs_box.IsHitTestVisible = true;
            BeginnerRadioButton.IsChecked = false;
        }

        private void Beginner_button_OnClick(object sender, RoutedEventArgs e)
        {
            _level = Level.Beginner;
            songs_box.IsHitTestVisible = false;
            ExpertRadioButton.IsChecked = false;

        }

        private void SongChanged(object sender, SelectionChangedEventArgs e)
        {
            if (songs_box.SelectedIndex == 0)
                CurrentChords = AllChordsArray;
            else if (songs_box.SelectedIndex == 1)
                CurrentChords = OneDayArray;
            currentChordIndex = 0;
            moveToNextChord();
        }
    }
}
