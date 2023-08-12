using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        private static readonly DependencyPropertyKey TileArrayProperty;
        private static readonly DependencyPropertyKey TileSizeProperty;
        private static readonly DependencyPropertyKey ContrastColorProperty;

        private BitmapImage tileset;

        static ImageSelector()
        {
            TileArrayProperty = DependencyProperty.RegisterReadOnly(
                "TileArray",
                typeof(Image[]),
                typeof(ImageSelector),
                new PropertyMetadata(null));
            TileSizeProperty = DependencyProperty.RegisterReadOnly(
                "TileSize",
                typeof(int),
                typeof(ImageSelector),
                new PropertyMetadata(10));
            ContrastColorProperty = DependencyProperty.RegisterReadOnly(
                "ContrastColor",
                typeof(Color),
                typeof(ImageSelector),
                new PropertyMetadata(Colors.Black)
                );
        }

        public Image[] TileArray => (Image[])GetValue(TileArrayProperty.DependencyProperty);
        public int TileSize => (int)GetValue(TileSizeProperty.DependencyProperty);
        public Color ContrastColor => (Color)GetValue(ContrastColorProperty.DependencyProperty);

        public ImageSelector()
        {
            InitializeComponent();
        }

        private void SetTileSize(int size)
        {
            SetValue(TileSizeProperty, size);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                TextBlock t = (TextBlock)e.OriginalSource;
                if (t.Text == SizeHint.Text)
                {
                    SizeTB.Focus();
                }
            }
        }

        private bool LoadImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Tileset|*.png;*.bmp;*.jpg;*.jpeg";
            bool? res = ofd.ShowDialog();
            if (res != true) return false;
            Stream img = File.OpenRead(ofd.FileName);
            tileset = new BitmapImage();
            tileset.BeginInit();
            tileset.StreamSource = img;
            tileset.CacheOption = BitmapCacheOption.OnLoad;
            tileset.EndInit();
            tileset.Freeze();
            ImageDisplay.Source = tileset;
            ImageDisplay.Width = tileset.PixelWidth;
            ImageDisplay.Height = tileset.PixelHeight;
            return true;
        }

        private int GetAverageImageLuminocity()
        {
            int totalR = 0;
            int totalG = 0;
            int totalB = 0;
            var b = new System.Drawing.Bitmap(tileset.StreamSource);
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    System.Drawing.Color c = b.GetPixel(x, y);
                    totalR += c.R;
                    totalG += c.G;
                    totalB += c.B;
                }
            }
            int pixels = b.Width * b.Height;
            return (totalR + totalG + totalB) / (pixels * 3);
        }

        private Color GetContrastColor(byte averageLuminocity)
        {
            byte inverseLuminocity = (byte)(255 - averageLuminocity);
            bool light = inverseLuminocity > 127;
            inverseLuminocity /= 2;
            if (light) { inverseLuminocity += 127; }
            return Color.FromRgb(inverseLuminocity, inverseLuminocity, inverseLuminocity);
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoadImage())
            {
                int lum = GetAverageImageLuminocity();
                SetValue(ContrastColorProperty, GetContrastColor((byte)lum));
            }
        }

        private void SizeTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetTileSize(Convert.ToInt32((sender as TextBox).Text));
                Keyboard.ClearFocus();
            }
            if (e.Key < Key.D0 || e.Key > Key.D9) e.Handled = true;
        }
    }
}
