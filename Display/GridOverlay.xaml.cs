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
    /// Логика взаимодействия для GridOverlay.xaml
    /// </summary>
    public partial class GridOverlay : UserControl
    {
        public static readonly DependencyProperty
            LineIntervalProperty,
            LineColorProperty;

        private int lineInterval;

        static GridOverlay()
        {
            LineIntervalProperty = DependencyProperty.Register(
                "LineInterval",
                typeof(int),
                typeof(GridOverlay),
                new FrameworkPropertyMetadata(
                    10,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(LineIntervalChanged),
                    new CoerceValueCallback(IntervalCoerce)));
            LineColorProperty = DependencyProperty.Register(
                "LineColor",
                typeof(Color),
                typeof(GridOverlay),
                new FrameworkPropertyMetadata(
                    Colors.Black,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(GridColorChanged),
                    new CoerceValueCallback(GridColorAddAlpha)));
        }

        private static object GridColorAddAlpha(DependencyObject d, object baseValue)
        {
            Color c = (Color)baseValue;
            c.A = 200;
            return c;
        }

        private static void GridColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((Color)e.OldValue == (Color)e.NewValue) return;
            (d as GridOverlay).UpdateGridColor();
        }

        private static object IntervalCoerce(DependencyObject d, object baseValue)
        {
            if ((int)baseValue < 2) return 2;
            return baseValue;
        }

        private static void LineIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == (int)e.OldValue) return;
            (d as GridOverlay).GenerateGrid();
        }

        public int LineInterval
        {
            get { return (int)GetValue(LineIntervalProperty); }
            set { SetValue(LineIntervalProperty, value); }
        }

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set { SetValue(LineColorProperty, value); }
        }

        public GridOverlay()
        {
            InitializeComponent();
            Loaded += delegate { GenerateGrid(); };
            SizeChanged += delegate { GenerateGrid(); };
        }

        private void DrawLineBetween(Point p1, Point p2)
        {
            var l = new Line();
            l.X1 = p1.X; l.X2 = p2.X;
            l.Y1 = p1.Y; l.Y2 = p2.Y;
            l.StrokeThickness = 1;
            l.Visibility = Visibility.Visible;
            OverlayCanvas.Children.Add(l);
        }

        private void UpdateGridColor()
        {
            SolidColorBrush b = new SolidColorBrush(LineColor);
            foreach (Line l in OverlayCanvas.Children)
            {
                l.Stroke = b;
            }
        }

        private void GenerateGrid()
        {
            OverlayCanvas.Children.Clear();
            SolidColorBrush brush = new SolidColorBrush(LineColor);
            double w = ActualWidth < 1 ? Width : ActualWidth;
            double h = ActualHeight < 1 ? Height : ActualHeight;
            for (int x = LineInterval; x < w; x += LineInterval)
            {
                DrawLineBetween(new Point(x, 0), new Point(x, h));
            }
            for (int y = LineInterval; y < h; y += LineInterval)
            {
                DrawLineBetween(new Point(0, y), new Point(w, y));
            }
            UpdateGridColor();
        }
    }
}
