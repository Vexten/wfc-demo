using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wfc_demo.Display
{
    /// <summary>
    /// Логика взаимодействия для ImageSelector.xaml
    /// </summary>
    public partial class ImageSelector : UserControl
    {
        public ImageSelector()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(TextBlock)) return;
            TextBlock t = (TextBlock)e.OriginalSource;
            if (t.Text == WidthHint.Text)
            {
                WidthTB.Focus();
            }
            else
            {
                HeightTB.Focus();
            }
        }
    }
}
