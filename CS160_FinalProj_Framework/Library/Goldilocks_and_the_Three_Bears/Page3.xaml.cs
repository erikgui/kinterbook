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
    public partial class Page3 : Page
    {
        private String[] allText = {"In the living room, Goldilocks found three chairs.",
                                    "She tried sitting in Papa Bear's chair first, but it was too high.",
                                    "Next, she tried sitting in Mama Bear's chair, but it was too short.",
                                    "Finally, Goldilocks sat in Baby Bear's chair, and it was just right! But Goldilocks sat for so long that Baby Bear's chair broke.",                                    
                                    "And so, because she was still tired, she decided to look upstairs for someplace to rest.",
                                    "Again, she found three beds in the bedroom upstairs.",
                                    "First she tried Papa Bear's bed, but it was too hard. Then she tried Mama Bear's bed, but it was too soft.",
                                    "Finally, she laid down in Baby Bear's bed, which was just right. In fact, it was so comfy that Godlilocks fell asleep!"};

        public Page3()
        {
            InitializeComponent();
            StorybookPage.Textblock.Text = allText.ElementAt(0);
            StorybookPage.textMax = allText.Count() - 1;
            StorybookPage.lines = allText;
        }
    }
}
