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

namespace CS160_FinalProj_Framework
{    
    public partial class StorybookPage : Page
    {        
        public static String path = "pack://application:,,,/Library/" + MainWindow.CurrentBook + "/";

        // [WL] For gestures (hardcoding method).
        private static float oldRightHandX;
        private static float oldRightHandZ;
        private static int knockCount = 0;

        // [WL] To handle changing pages.
        private static int currentPage = 0; // [WL] Current page of storybook we're on; 0 means cover. 
        public static int pageMax = 0; // [WL] The total number of pages our storybook has.    
        
        // [WL] To handle changing lines within a page.
        private static int currentText = 0; // [WL] The current text within the current page.
        public static int textMax = 0; // [WL] Total number of lines within the current page. 
        public static String[] lines;
        private static String instructions = "";

        // [WL] The three menu icons.
        private static Image SettingsIcon;
        private static Image PlayIcon;
        private static Image ExitIcon;

        // [WL] Other important page components.
        private static MediaElement AudioPlayer;
        private static int playAudioIconTimer = 0;
        public static TextBlock Textblock;
        private static Frame Book;

        public StorybookPage()
        {
            InitializeComponent();
            //SettingsIcon = Storybook_SettingsIcon;
            PlayIcon = Storybook_PlayIcon;
            //ExitIcon = Storybook_ExitIcon;
            AudioPlayer = Audio;
            Textblock = Storybook_Text;
            Book = PageFrame;            
            init();
        }

        public static String cleanTitle(String s)
        {
            return s.Replace("_", " ");
        }

        // [WL] Setup for the landing page (book cover + title).
        public void init()
        {
            Book.Source = new Uri(path + "CoverPage.xaml", UriKind.RelativeOrAbsolute);
            Textblock.Text = cleanTitle(MainWindow.CurrentBook);                        
        }

        private static void reset()
        {
            PlayIcon.Opacity = 1.0;
            playAudioIconTimer = 0;
            knockCount = 0;
            Textblock.Foreground = new SolidColorBrush(Colors.Black);
            Textblock.FontWeight = FontWeights.Normal;            
        }

        private static bool onPlayIconCheck()
        {
            return (Canvas.GetLeft(MainWindow.cursor) > Canvas.GetLeft(PlayIcon) && Canvas.GetLeft(MainWindow.cursor) < (Canvas.GetLeft(PlayIcon) + PlayIcon.ActualWidth) && Canvas.GetTop(MainWindow.cursor) > Canvas.GetTop(PlayIcon) && Canvas.GetTop(MainWindow.cursor) < (Canvas.GetTop(PlayIcon) + PlayIcon.ActualHeight));                        
        }

        // [WL] Hardcoded, at least for temporary use.
        private static bool rightHandSwipeLeft(float rightHandX)
        {
            //Console.WriteLine("swipe distance = " + Math.Abs(oldRightHandX - rightHandX));
            return ((oldRightHandX - rightHandX) > 0.07);            
        }

        private static bool knockingMotion(float rightHandZ)
        {
            //Console.WriteLine("old = " + oldRightHandZ + " new = " + rightHandZ + " knock distance = " + (oldRightHandZ - rightHandZ));
            if (Math.Abs(oldRightHandZ - rightHandZ) > 0.01)
            {                
                knockCount++;
            }
            Console.WriteLine("knockCount = " + knockCount);
            return knockCount >= 3;
        }

        public static void gestureChecks(float rightHandX, float rightHandZ)
        {
            if (MainWindow.playingMode && onPlayIconCheck())
            {
                playAudioIconTimer++;
                if (playAudioIconTimer <= 25)
                {
                    PlayIcon.Opacity = 25.0 / MainWindow.timerMax;
                }
                else
                {
                    PlayIcon.Opacity = (double)(playAudioIconTimer / MainWindow.timerMax);
                    if (playAudioIconTimer >= MainWindow.timerMax)
                    {                        
                        // play corresponding audio file for the page    
                        // temporarily just playing a fixed audio
                        AudioPlayer.Source = new Uri("C:\\Users\\whitlai\\Desktop\\CS160\\CS160_FinalProj_Framework\\CS160_FinalProj_Framework\\recording.wav", UriKind.RelativeOrAbsolute);
                        AudioPlayer.LoadedBehavior = MediaState.Play;
                        AudioPlayer.UnloadedBehavior = MediaState.Close;
                    }
                }
            }
            else if (!MainWindow.playingMode)
            {
                // recording stuff goes here
            }
            else if (!instructions.Equals(""))
            {
                // may need to move the following three lines before the switch/case statement
                Console.WriteLine("Currently on an instruction.");
                MainWindow.ready = false;
                instructions = instructions.Replace("*", "");
                switch (instructions)
                {
                    case "KNOCK":
                        if (knockingMotion(rightHandZ))
                        {
                            Console.WriteLine("Done knocking.");
                            instructions = "";
                            reset();
                            StorybookPage.Textblock.Text = "Good job! Swipe to continue.";
                        }
                        break;
                    default:
                        instructions = "";
                        break;
                }
            }
            else
            {
                reset();
                if (rightHandSwipeLeft(rightHandX))
                {
                    if (currentText < textMax)
                    {
                        currentText++;
                        if (lines[currentText].Contains('*'))
                        {
                            int pos = lines[currentText].IndexOf('*');
                            instructions = lines[currentText].Substring(pos);
                            lines[currentText] = lines[currentText].Replace(instructions, "");
                            Textblock.Foreground = new SolidColorBrush(Colors.Red);
                            Textblock.FontWeight = FontWeights.Bold;
                        }
                        else
                        {
                            instructions = "";
                            MainWindow.ready = false;
                        }
                        Textblock.Text = lines[currentText];
                    }
                    else
                    {
                        currentText = 0;
                        if (currentPage + 1 <= pageMax)
                        {
                            currentPage++;
                            MainWindow.ready = false;
                            Book.Source = new Uri(path + "Page" + currentPage + ".xaml", UriKind.RelativeOrAbsolute);
                        }
                    }
                }
                oldRightHandX = rightHandX;
                oldRightHandZ = rightHandZ;
                /*switch (MainWindow.gesture)
                {
                    case "Right hand swipe left":
                        // change to next page
                        if (currentPage + 1 < pageMax)
                        {
                            currentPage++;
                            Book.Source = new Uri(path + "Page" + currentPage + ".xaml", UriKind.RelativeOrAbsolute);
                        }                        
                        break;
                    case "Right hand swipe right":
                        if (currentPage - 1 > 0)
                        {
                            currentPage--;
                            if (currentPage > 0)
                            {
                                Book.Source = new Uri(path + "Page" + currentPage + ".xaml", UriKind.RelativeOrAbsolute);
                            }
                            else if (currentPage == 0)
                            {
                                Book.Source = new Uri(path + "CoverPage.xaml", UriKind.RelativeOrAbsolute);
                            }
                        }                        
                        //MainWindow.pageFrame.Navigate(path + "Page" + currentPage);
                        break;
                    case "Knock left":
                        // do something
                        break;
                    case "Knock right":
                        // do something
                        break;
                    default:
                        break;
                }*/
            }
        }        

        // temporary, for testing purposes
        private void Storybook_PlayIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Playing audio...");
            // temporary- testing audio playback (replace with proper .wav file path)
            Audio.Source = new Uri("http://www.wav-sounds.com/movie/harrypotter.wav", UriKind.Absolute);
            Audio.LoadedBehavior = MediaState.Play;
            Audio.UnloadedBehavior = MediaState.Close;
        }  
    }
}
