using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogBuckets.Controls
{
    public class GlyphButton : Button
    {
        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GlyphButton));

        public event RoutedEventHandler ColorChanged
        {
            add { AddHandler(ColorChangedEvent, value); }
            remove { RemoveHandler(ColorChangedEvent, value); }
        }


        public static readonly DependencyProperty NormalForegroundProperty =
            DependencyProperty.Register("NormalForeground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.DarkGray, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty NormalBackgroundProperty =
            DependencyProperty.Register("NormalBackground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.White, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty DisabledForegroundProperty =
            DependencyProperty.Register("DisabledForeground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty DisabledBackgroundProperty =
            DependencyProperty.Register("DisabledBackground", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty DisabledBorderColorProperty =
            DependencyProperty.Register("DisabledBorderColor", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(GlyphButton), new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty NormalBorderColorProperty =
            DependencyProperty.Register("NormalBorderColor", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty HoverBorderColorProperty =
            DependencyProperty.Register("HoverBorderColor", typeof(Brush), typeof(GlyphButton), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(OnColorChanged)));

        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register("Glyph", typeof(Path), typeof(GlyphButton),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush NormalForeground
        {
            get { return (Brush)GetValue(NormalForegroundProperty); }
            set { SetValue(NormalForegroundProperty, value); }
        }

        public Brush NormalBackground
        {
            get { return (Brush)GetValue(NormalBackgroundProperty); }
            set { SetValue(NormalBackgroundProperty, value); }
        }

        public Brush HoverForeground
        {
            get { return (Brush)GetValue(HoverForegroundProperty); }
            set { SetValue(HoverForegroundProperty, value); }
        }

        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Brush NormalBorderColor
        {
            get { return (Brush)GetValue(NormalBorderColorProperty); }
            set { SetValue(NormalBorderColorProperty, value); }
        }

        public Brush HoverBorderColor
        {
            get { return (Brush)GetValue(HoverBorderColorProperty); }
            set { SetValue(HoverBorderColorProperty, value); }
        }

        public Brush DisabledBorderColor
        {
            get { return (Brush)GetValue(DisabledBorderColorProperty); }
            set { SetValue(DisabledBorderColorProperty, value); }
        }

        public Path Glyph
        {
            get { return (Path)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }


        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(ColorChangedEvent);
            ((GlyphButton)d).RaiseEvent(args);
        }
    }
}
