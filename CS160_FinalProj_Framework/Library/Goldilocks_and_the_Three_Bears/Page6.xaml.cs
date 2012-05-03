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

namespace CS160_FinalProj_Framework.Library.Goldilocks_and_the_Three_Bears
{
    public partial class Page6 : Page
    {
        private String[] allText = {"Finally, they decided to try to sleep, so they went upstairs.",                                    
                                    "\"Someone has been sleeping in my bed!\" said Papa Bear.",
                                    "\"Someone has been sleeping in my bed, too!\" said Mama Bear.",
                                    "\"Someone has been sleeping in my bed, too!\" said Baby Bear, \"And she's still there!\"",
                                    "Hearing Baby Bear's voice, Goldilocks awoke with a start.",
                                    "Glancing around and seeing three bears, Goldilocks jumped out of bed and ran away as fast as she could.",
                                    "And she never went near the house of the three bears ever again!",
                                    "The End"};

        public Page6()
        {
            InitializeComponent();
            StorybookPage.Textblock.Text = allText.ElementAt(0);
            StorybookPage.textMax = allText.Count() - 1;
            StorybookPage.lines = allText;
        }
    }
}
