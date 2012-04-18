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
    public partial class Page2 : Page
    {
        private String[] allText = {"Inside the house, Goldilocks saw a table with three bowls of porridge.",
                                    "Goldilocks decided to try Papa Bear's big blue bowl first, but it was too hot.",
                                    "She then decided to try Mama Bear's medium red bowl, but it was too cold.",
                                    "Finally, Goldilocks tried Baby Bear's little yellow bowl, and it just was right!"};                           

        public Page2()
        {
            InitializeComponent();
            StorybookPage.Textblock.Text = allText.ElementAt(0);
            StorybookPage.textMax = allText.Count() - 1;
            StorybookPage.lines = allText;
        }
    }
}
