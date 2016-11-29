using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
using System.Diagnostics;
using System.IO;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string highScoreUsername = "N/A";
        private int highScore = 0;
        private BitmapImage image;
        private Image img;
        private int score = 0;
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private System.Windows.Threading.DispatcherTimer dispatcherTimerCountdown;

        public MainWindow()
        {
            InitializeComponent();

            Kill_the_Chickens.WindowState = WindowState.Maximized;
            Kill_the_Chickens.WindowStyle = WindowStyle.None;

            string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
             + @"\resources\target.cur";

            Mouse.OverrideCursor = new Cursor(path);

            image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(@"http://www.gourmetsleuth.com/images/default-source/articles/big-white-chicken.jpg?sfvrsn=8");
            image.EndInit();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();

            dispatcherTimerCountdown = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimerCountdown.Tick += new EventHandler(dispatcherTimerCountdown_Tick);
            dispatcherTimerCountdown.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimerCountdown.Start();

            try
            {
                string h = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\HighScore.txt");

                string[] hArray = h.Split(':');

                highScoreUsername = hArray[0];
                highScore = Convert.ToInt16(hArray[1]);
            }
            catch (Exception e)
            {
                highScore = 0;
            }

            highscoreLabel.Content = highScoreUsername + ": " + highScore;

            img = new Image();
            img.Source = image;
            img.MouseLeftButtonDown += new MouseButtonEventHandler(hit);
            img.Margin = new Thickness(0, 0, 0, 0);
            img.Width = 100;
            img.Height = 100;

            canvas.Children.Add(img);
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            scoreLabel.Content = "Score: " + score;

            dispatcherTimer.Stop();
            dispatcherTimerCountdown.Stop();

            timeLeft.Content = 60;
            dispatcherTimer.Start();
            dispatcherTimerCountdown.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            timeLeft.Content = 0;

            dispatcherTimer.Stop();
            dispatcherTimerCountdown.Stop();

            MessageBox.Show("You have an end score of " + score + "!");

            timeLeft.Content = 60;
            dispatcherTimer.Start();
            dispatcherTimerCountdown.Start();

            using (StreamWriter outputFile = File.AppendText((Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\HighScore.txt")))
            {
                if (score > highScore)
                {
                    outputFile.WriteLine(username.Text + ":" + score);

                    highscoreLabel.Content = username.Text + ": " + score;
                }
            }

            score = 0;
            scoreLabel.Content = "Score: " + score;
        }

        private void dispatcherTimerCountdown_Tick(object sender, EventArgs e)
        {
            int timeLeftInSeconds = Convert.ToInt16(timeLeft.Content);

            timeLeft.Content = timeLeftInSeconds - 1;
        }

        private void hit(object sender, RoutedEventArgs e)
        {
            score++;

            double h = (new Random().NextDouble() * 900) - 1;
            double w = (new Random().NextDouble() * 1500) - 1;
            
            img.SetValue(Canvas.LeftProperty, w);
            img.SetValue(Canvas.TopProperty, h);

            scoreLabel.Content = "Score: " + score;
        }
    }
}
