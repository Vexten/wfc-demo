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
    /// Логика взаимодействия для TiledDisplay.xaml
    /// </summary>
    public partial class TiledDisplay : UserControl
    {
        private int tWidth;
        private int tHeight;
        private int tSize;
        private Image[] tiles;

        public static readonly DependencyProperty
            TileWidthProperty = DependencyProperty.Register(
                "TileWidth",
                typeof(int),
                typeof(TiledDisplay),
                new FrameworkPropertyMetadata(10,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(DimentionChanged),
                    new CoerceValueCallback(CoerceDimention)));
        public static readonly DependencyProperty
           TileHeightProperty = DependencyProperty.Register(
                "TileHeight",
                typeof(int),
                typeof(TiledDisplay),
                new FrameworkPropertyMetadata(10,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(DimentionChanged),
                    new CoerceValueCallback(CoerceDimention)));

        protected static object CoerceDimention(DependencyObject d, object value)
        {
            if ((int)value < 10) return 10;
            return value;
        }

        protected static void DimentionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == (int)e.OldValue) return;
            TiledDisplay t = (TiledDisplay)d;
            if (e.Property == TileWidthProperty)
            {
                t.tWidth = (int)e.NewValue;
                t.Display.Width = t.tWidth * t.tSize;
            }
            t.tHeight = (int)e.NewValue;
            t.Display.Height = t.tHeight * t.tSize;
        }

        public int TileWidth
        {
            get { return (int)GetValue(TileWidthProperty); }
            set { SetValue(TileWidthProperty, value); }
        }

        public int TileHeight
        {
            get { return (int)GetValue(TileHeightProperty); }
            set { SetValue(TileHeightProperty, value); }
        }

        public TiledDisplay()
        {
            InitializeComponent();
        }
    }
}
