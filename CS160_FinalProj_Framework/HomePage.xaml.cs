using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;

namespace CS160_FinalProj_Framework
{
    public partial class HomePage : Page
    {                
        private static Image playButton;
        private static Image recordButton;
        //private static Image settingsButton;

        private static int playButtonTimer = 0;
        private static int recordButtonTimer = 0;

        public HomePage()
        {
            InitializeComponent();           
            playButton = PlayIcon;
            reset();
        }

        private static void reset()
        {
            playButton.Opacity = 1.0;
            playButtonTimer = 0;
        }

        /*
        private static bool onPlayIconCheck()
        {            
            return (Canvas.GetLeft(MainWindow.cursor) > Canvas.GetLeft(playButton) && Canvas.GetLeft(MainWindow.cursor) < (Canvas.GetLeft(playButton) + playButton.ActualWidth) && Canvas.GetTop(MainWindow.cursor) > Canvas.GetTop(playButton) && Canvas.GetTop(MainWindow.cursor) < (Canvas.GetTop(playButton) + playButton.ActualHeight));            
        }
         */

        public static void gestureChecks()
        {
            if (MainWindow.hoverCheck(playButton))
            {
                playButtonTimer++; 
                if (playButtonTimer <= 25)
                {
                    playButton.Opacity = 25.0 / MainWindow.timerMax;
                }
                else
                {
                    playButton.Opacity = (double)(playButtonTimer / MainWindow.timerMax);
                    if (playButtonTimer >= MainWindow.timerMax)
                    {
                        MainWindow.playingMode = true;
                        MainWindow.pageFrame.Navigate(new LibraryPage());
                    }
                }                                                               
            }
            else if (MainWindow.hoverCheck(recordButton))
            {
                recordButtonTimer++;
                if (recordButtonTimer <= 25)
                {
                    recordButton.Opacity = 25.0 / MainWindow.timerMax;
                }
                else
                {
                    recordButton.Opacity = (double)(recordButtonTimer / MainWindow.timerMax);
                    if (recordButtonTimer >= MainWindow.timerMax)
                    {
                        MainWindow.playingMode = false;
                        MainWindow.pageFrame.Navigate(new LibraryPage());
                    }
                }
            }
            else
            {
                reset();
            }
        }

        // temporary, for testing purposes
        private void PlayIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.playingMode = true;
            MainWindow.pageFrame.Navigate(new LibraryPage());
        }

        private void RecordIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.playingMode = false;
            MainWindow.pageFrame.Navigate(new LibraryPage());
        }               
    }
}
