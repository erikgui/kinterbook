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
    public partial class InstructionsPage : Page
    {
        private static float oldRightHandX;

        public InstructionsPage()
        {
            InitializeComponent();
        }      

        /*public static void gestureChecks()
        {
            Console.WriteLine("gesturedcheck " + MainWindow.gesture);
            if (MainWindow.gesture.Contains("Right hand swipe left"))
            {
                MainWindow.pageFrame.Navigate(new StorybookPage());
            }            
        }*/

        // [WL] Hardcoded R-hand swipe in L-direction b/c the DTW Gesture Recognizer doesn't like me. :(
        public static void gestureChecks(float rightHandX)
        {
            //Console.WriteLine("rightHandX = "+rightHandX);
            if (oldRightHandX - rightHandX > 0.07)
            {
                MainWindow.ready = false;
                MainWindow.pageFrame.Navigate(new StorybookPage());
            }
            oldRightHandX = rightHandX;
        }

        // temporary, for testing purposes only
        private void InstructionalImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.pageFrame.Navigate(new StorybookPage());
        }
    }
}
