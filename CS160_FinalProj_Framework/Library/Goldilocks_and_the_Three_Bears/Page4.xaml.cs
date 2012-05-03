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
    public partial class Page4 : Page
    {
        private String[] allText = {"Now while Goldilocks was sleeping, the family of bears came home.",
                                    "Being hungry, they went to eat their porridge first.",
                                    "\"Someone has eaten my porridge!\" said Baby Bear."};

        public Page4()
        {
            InitializeComponent();
            StorybookPage.Textblock.Text = allText.ElementAt(0);
            StorybookPage.textMax = allText.Count() - 1;
            StorybookPage.lines = allText;
        }
    }
}
