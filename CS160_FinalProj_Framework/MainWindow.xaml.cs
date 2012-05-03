using System;
using System.Collections;
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
using DTWGestureRecognition;

namespace CS160_FinalProj_Framework
{
    public partial class MainWindow : Window
    {
        // [WL] For the Kinect sensor.
        public static Microsoft.Samples.Kinect.WpfViewers.KinectSensorChooser sensor;        
        public static KinectAudioSource sensorAudio;
        public bool closing = false;

        // [WL] For DTW Gesture Recognizer.
        public static DtwGestureRecognizer _dtw;
        private ArrayList _video;
        private int _flipFlop;
        private const int Ignore = 2;
        public static String gesture = "UNKNOWN";
        public static int timeout = 0;
        
        // [WL] Cursor details.
        public static Ellipse cursor;        

        // [WL] Skeleton details.
        private static Skeleton[] allSkeletons = new Skeleton[6];
        public static Skeleton skeleton;
        
        // [WL] For gestures.
        public static bool ready = false;
        private static int readyTimer = 0;
        public static double timerMax = 125.0;
        
        // [WL] Miscellaneous variables and page elements.
        public static Frame pageFrame;        
        private static String currentPage = "HomePage";  
        public static String CurrentBook = "";
        public static bool playingMode = true; 

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;            
            cursor = KinectCursor;
            pageFrame = MainFrame;
            ready = true;
        }

        public static bool hoverCheck(Image img)
        {                   
            return (Canvas.GetLeft(MainWindow.cursor) > Canvas.GetLeft(img) && Canvas.GetLeft(MainWindow.cursor) < (Canvas.GetLeft(img) + img.ActualWidth) && Canvas.GetTop(MainWindow.cursor) > Canvas.GetTop(img) && Canvas.GetTop(MainWindow.cursor) < (Canvas.GetTop(img) + img.ActualHeight));                    
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            currentPage = MainFrame.NavigationService.Content.GetType().Name.ToString();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KinectSensorChooser.KinectSensorChanged += new DependencyPropertyChangedEventHandler(KinectSensorChooser_KinectSensorChanged);
        }

        private void KinectSensorChooser_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor oldSensor = (KinectSensor)e.OldValue;
            StopKinect(oldSensor);

            KinectSensor newSensor = (KinectSensor)e.NewValue;
            if (newSensor == null)
            {
                return;
            }

            cursor.Visibility = Visibility.Visible;
            sensorAudio = newSensor.AudioSource; // [WL] For audio recording.

            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.3f,
                Correction = 0.2f,
                Prediction = 0.1f,
                JitterRadius = 0.5f,
                MaxDeviationRadius = 0.2f
            };

            newSensor.ColorStream.Enable();
            newSensor.DepthStream.Enable();
            newSensor.SkeletonStream.Enable(parameters);
            newSensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(newSensor_AllFramesReady);

            _dtw = new DtwGestureRecognizer(12, 0.6, 2, 2, 10);
            _video = new ArrayList();
            newSensor.SkeletonFrameReady += SkeletonExtractSkeletonFrameReady;
            //Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReady;            
            LoadGesturesFromFile("C:\\Users\\whitlai\\Desktop\\CS160\\CS160_FinalProj_Framework\\CS160_FinalProj_Framework\\RecordedGesturesPrototype.txt");  

            try
            {
                newSensor.Start();
            }
            catch (System.IO.IOException)
            {
                KinectSensorChooser.AppConflictOccurred();
            }
        }

        void newSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (!gesture.Contains("UNKNOWN"))
            {
                timeout++;
                if (timeout > 30)
                {
                    gesture = "UNKNOWN";
                }
            }
            else
            {
                timeout = 0;
            }

            if (closing)
            {
                return;
            }

            skeleton = GetFirstSkeleton(e);
            if (skeleton == null)
            {
                return;
            }

            GetCameraPoint(skeleton, e);
            ScalePosition(cursor, skeleton.Joints[JointType.HandRight]);

            float rightHandX = skeleton.Joints[JointType.HandRight].Position.X;
            float rightHandZ = skeleton.Joints[JointType.HandRight].Position.Z;
            /*float rightHandY = skeleton.Joints[JointType.HandRight].Position.Y;            
            float leftHandX = skeleton.Joints[JointType.HandLeft].Position.X;
            float leftHandY = skeleton.Joints[JointType.HandLeft].Position.Y;            
            float headY = skeleton.Joints[JointType.Head].Position.Y;*/

            if (!ready)
            {
                //Console.Write("readyTimer = " + readyTimer);
                readyTimer++;
                if (readyTimer > 100)
                {
                    ready = true;
                    readyTimer = 0;
                }
            }
            else
            {
                // [WL] Gesture recognition stuff (broken up by page).           
                if (currentPage == "HomePage")
                {
                    HomePage.gestureChecks();
                }
                else if (currentPage == "LibraryPage")
                {
                    LibraryPage.gestureChecks();
                }
                else if (currentPage == "InstructionsPage")
                {
                    InstructionsPage.gestureChecks(rightHandX);
                }
                else if (currentPage == "StorybookPage")
                {
                    StorybookPage.gestureChecks(rightHandX, rightHandZ);
                }
            }
        }

        public void LoadGesturesFromFile(string fileLocation)
        {
            int itemCount = 0;
            string line;
            string gestureName = String.Empty;

            // TODO I'm defaulting this to 12 here for now as it meets my current need but I need to cater for variable lengths in the future
            ArrayList frames = new ArrayList();
            double[] items = new double[12];

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(fileLocation);
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("@"))
                {
                    gestureName = line;
                    continue;
                }

                if (line.StartsWith("~"))
                {
                    frames.Add(items);
                    itemCount = 0;
                    items = new double[12];
                    continue;
                }

                if (!line.StartsWith("----"))
                {
                    items[itemCount] = Double.Parse(line);
                }

                itemCount++;

                if (line.StartsWith("----"))
                {
                    MainWindow._dtw.AddOrUpdate(frames, gestureName);
                    frames = new ArrayList();
                    gestureName = String.Empty;
                    itemCount = 0;
                }
            }

            file.Close();
        }

        private static void SkeletonExtractSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            bool receivedData = false;
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    if (allSkeletons == null) //allocate the first time
                    {
                        allSkeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }
                    receivedData = true;
                    skeletonFrame.CopySkeletonDataTo(allSkeletons);
                }
                else
                {
                    // apps processing of skeleton data took too long; it got more than 2 frames behind.
                    // the data is no longer available.
                }
            }
            if (receivedData)
            {
                foreach (Skeleton data in allSkeletons)
                {
                    if (data != null && data.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        string tmpGesture = Skeleton2DDataExtract.ProcessData(data, _dtw);
                        if (!tmpGesture.Contains("UNKNOWN"))
                        {
                            gesture = tmpGesture;
                            Console.WriteLine(gesture);
                        }
                    }
                }
            }
        }

        private void NuiSkeleton2DdataCoordReady(object sender, Skeleton2DdataCoordEventArgs a)
        {
            if (timeout < 30)
            {
                timeout++;
            }
            // We need a sensible number of frames before we start attempting to match gestures against remembered sequences
            if (_video.Count > 30)
            {
                timeout = 0;
                //Debug.WriteLine("Reading and video.Count=" + video.Count);
                gesture = _dtw.Recognize(_video);                
                if (!gesture.Contains("__UNKNOWN"))
                {
                    // There was no match so reset the buffer
                    _video = new ArrayList();
                }
            }

            // Ensures that we remember only the last x frames
            if (_video.Count > 32)
            {
                // Remove the first frame in the buffer
                _video.RemoveAt(0);
            }

            // Decide which skeleton frames to capture. Only do so if the frames actually returned a number. 
            // For some reason my Kinect/PC setup didn't always return a double in range (i.e. infinity) even when standing completely within the frame.
            // TODO Weird. Need to investigate this
            if (!double.IsNaN(a.GetPoint(0).X))
            {
                // Optionally register only 1 frame out of every n
                _flipFlop = (_flipFlop + 1) % Ignore;
                if (_flipFlop == 0)
                {
                    _video.Add(a.GetCoords());
                }
            }

            // Update the debug window with Sequences information
            //dtwTextOutput.Text = _dtw.RetrieveText();
        }

        private void ScalePosition(FrameworkElement element, Joint joint)
        {
            Joint scaledJoint = joint.ScaleTo(1280,800); // NOTE: probably need to adjust this better
            Canvas.SetLeft(element, scaledJoint.Position.X);
            Canvas.SetTop(element, scaledJoint.Position.Y);
        }

        private void CameraPosition(FrameworkElement element, ColorImagePoint point)
        {           
            Canvas.SetLeft(element, point.X - element.Width / 2);
            Canvas.SetTop(element, point.Y - element.Height / 2);
        }

        void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {
            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null ||
                    KinectSensorChooser.Kinect == null)
                {
                    return;
                }

                DepthImagePoint rightDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandRight].Position);
                ColorImagePoint rightColorPoint =
                    depth.MapToColorImagePoint(rightDepthPoint.X, rightDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                CameraPosition(cursor, rightColorPoint);
            }
        }       

        Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }
                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;
            }
        }

        private void StopKinect(KinectSensor s)
        {
            if (s != null)
            {
                if (s.IsRunning)
                {
                    s.ColorStream.Disable();
                    s.DepthStream.Disable();
                    s.SkeletonStream.Disable();
                    s.Stop();
                    if (s.AudioSource != null)
                    {
                        s.AudioSource.Stop();
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            StopKinect(KinectSensorChooser.Kinect);
        }        
    }
}
