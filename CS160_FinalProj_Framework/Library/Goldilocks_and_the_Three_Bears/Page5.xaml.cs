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
    public partial class Page5 : Page
    {
        private String[] allText = {"Next, because they were tired, they decided to sit.",                                    
                                    "\"Someone has sat in my chair and broke it!\" said Baby Bear."};

        public Page5()
        {
            InitializeComponent();
            StorybookPage.Textblock.Text = allText.ElementAt(0);
            StorybookPage.textMax = allText.Count() - 1;
            StorybookPage.lines = allText;
        }
    }
}
