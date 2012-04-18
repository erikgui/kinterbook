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
    public partial class CoverPage : Page
    {
        public CoverPage()
        {
            InitializeComponent();            
            StorybookPage.pageMax = Convert.ToInt32(PageCount.Text);
            StorybookPage.textMax = 0;
            StorybookPage.lines = null;
        }
    }
}
