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
    public partial class LibraryPage : Page
    {
        private static Image GoldilocksButton;
        private static Image BackButton;
        private static int GoldilocksButtonTimer = 0;
        private static int BackButtonTimer = 0;

        public LibraryPage()
        {
            InitializeComponent();
            GoldilocksButton = Goldilocks_and_the_Three_Bears;
            BackButton = Back;
            reset();
        }

        private static void reset()
        {
            GoldilocksButton.Opacity = 1.0;
            BackButton.Opacity = 1.0;
            GoldilocksButtonTimer = 0;
            BackButtonTimer = 0;
        }

        private static bool onGoldilocksIconCheck()
        {
            return (Canvas.GetLeft(MainWindow.cursor) > Canvas.GetLeft(GoldilocksButton) && Canvas.GetLeft(MainWindow.cursor) < (Canvas.GetLeft(GoldilocksButton) + GoldilocksButton.ActualWidth) && Canvas.GetTop(MainWindow.cursor) > Canvas.GetTop(GoldilocksButton) && Canvas.GetTop(MainWindow.cursor) < (Canvas.GetTop(GoldilocksButton) + GoldilocksButton.ActualHeight));
        }

        private static bool onBackIconCheck()
        {
            return (Canvas.GetLeft(MainWindow.cursor) > Canvas.GetLeft(BackButton) && Canvas.GetLeft(MainWindow.cursor) < (Canvas.GetLeft(BackButton) + BackButton.ActualWidth) && Canvas.GetTop(MainWindow.cursor) > Canvas.GetTop(BackButton) && Canvas.GetTop(MainWindow.cursor) < (Canvas.GetTop(BackButton) + BackButton.ActualHeight));
        }

        public static void gestureChecks()
        {
            if (onGoldilocksIconCheck())
            {
                GoldilocksButtonTimer++;
                if (GoldilocksButtonTimer <= 25)
                {
                    GoldilocksButton.Opacity = 25.0 / MainWindow.timerMax;
                }
                else
                {
                    GoldilocksButton.Opacity = (double)(GoldilocksButtonTimer / MainWindow.timerMax);
                    if (GoldilocksButtonTimer >= MainWindow.timerMax)
                    {
                        MainWindow.CurrentBook = "Goldilocks_and_the_Three_Bears";
                        MainWindow.pageFrame.Navigate(new InstructionsPage());
                    }
                }
            }
            else if (onBackIconCheck())
            {
                BackButtonTimer++;
                if (BackButtonTimer <= 25)
                {
                    BackButton.Opacity = 25.0 / MainWindow.timerMax;
                }
                else
                {
                    BackButton.Opacity = (double)(BackButtonTimer / MainWindow.timerMax);
                    if (BackButtonTimer >= MainWindow.timerMax)
                    {
                        MainWindow.pageFrame.Navigate(new HomePage());                        
                    }
                }
            }
            else
            {
                reset();
            }
        }

        // temporary, for testing purposes only
        private void Goldilocks_and_the_Three_Bears_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.CurrentBook = "Goldilocks_and_the_Three_Bears";
            MainWindow.pageFrame.Navigate(new InstructionsPage());
        }     
    }
}
