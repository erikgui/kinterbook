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
    public partial class Page1 : Page
    {
        private String[] allText = {"There once lived a little girl who was named Goldilocks, because her hair shone like gold.",
                                    "One day, Goldilocks got lost in the woods and came upon a neat little house.",
                                    "This house belonged to a family of three bears-- Papa Bear, Mama Bear, and Baby Bear. But Goldilocks did not know that.",
                                    "Goldilocks went up to the door and knocked.",
                                    "Help Goldilocks by knocking three times! *KNOCK*",
                                    "No one answered the door. And so, because she was very tired and hungry, Goldilocks opened the door."};                

        public Page1()
        {
            InitializeComponent();
            StorybookPage.Textblock.Text = allText.ElementAt(0);
            StorybookPage.textMax = allText.Count() - 1;
            StorybookPage.lines = allText;
        }
    }
}
