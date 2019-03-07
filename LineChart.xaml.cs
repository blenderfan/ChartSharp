using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChartSharp
{
    /* Class: LineChart
    
       A WPF Chart Control, that can render line data in different styles. It has far less features than similar software, but is (maybe) easy to setup and use. 
    */
    [ContentProperty("Items")]
    public partial class LineChart : UserControl
    {

        #region Constructor

        /* Constructor: LineChart
        
           Initializes all properties to their default values, manages event handlers and initializes the components of the control.
        */
        public LineChart()
        {
            Title = null;
            XLabel = null;
            YLabel = null;
            MinX = 0;
            MaxX = 1;
            MinY = 0;
            MaxY = 1;

            ChartStyle = new ChartStyle();

            Loaded += new RoutedEventHandler(OnLoaded);

            InitializeComponent();
        }

        #endregion

        #region Properties

        #region UIElements

        private TextBlock TitleBlock { get; set; } = new TextBlock();
        private TextBlock XLabelBlock { get; set; } = new TextBlock();
        private TextBlock YLabelBlock { get; set; } = new TextBlock();
        private Line XAxisLine { get; set; } = new Line();
        private Line YAxisLine { get; set; } = new Line();

        private List<Line> Markings { get; set; } = new List<Line>();
        private List<TextBlock> Numbers { get; set; } = new List<TextBlock>();
        private List<Polygon> Arrows { get; set; } = new List<Polygon>();
        private List<Line> Grid { get; set; } = new List<Line>();
        private Rectangle GridBackground { get; set; } = new Rectangle();

        private List<LineData> LineSeries { get; set; } = new List<LineData>();
        private List<LineData> ViewSeries { get; set; } = new List<LineData>();

        private List<UIElement> Elements { get; set; } = new List<UIElement>();

        #endregion

        #region InternalProperties

        private double MarginYTop { get; set; }
        private double MarginYBottom { get; set; }
        private double MarginXLeft { get; set; }
        private double MarginXRight { get; set; }
        private double CenterX { get; set; }
        private double CenterY { get; set; }

        /* Variable: Epsilon
        
           Epsilon is used to compare different doubles with each other. In this case, this is necessary to test if two
           points are equal.

           Default Value:

           0.0001
        */
        public static double Epsilon = 0.0001f;

        /* Variable: NrStdMarkings
        
           Determines how many intervals are automatically generated, if the markings of a <AxisStyle> are null.

           Default Value:

           10
        */
        public static double NrStdMarkings = 10;

        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(LineChart), new FrameworkPropertyMetadata(default(IEnumerable), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(LineChart), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty XLabelProperty = DependencyProperty.Register("XLabel", typeof(string), typeof(LineChart), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty YLabelProperty = DependencyProperty.Register("YLabel", typeof(string), typeof(LineChart), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MinXProperty = DependencyProperty.Register("MinX", typeof(double), typeof(LineChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MinYProperty = DependencyProperty.Register("MinY", typeof(double), typeof(LineChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MaxXProperty = DependencyProperty.Register("MaxX", typeof(double), typeof(LineChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MaxYProperty = DependencyProperty.Register("MaxY", typeof(double), typeof(LineChart), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ChartStyleProperty = DependencyProperty.Register("ChartStyle", typeof(ChartStyle), typeof(LineChart), new FrameworkPropertyMetadata(default(ChartStyle), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /* Property: ChartStyle
        
           Defines the style of the chart. Includes color, style of the X- and Y-Axis and style of a background grid.

           Default Value:

           Constructor of <ChartSharp.ChartStyle>
        */
        public ChartStyle ChartStyle
        {
            get { return (ChartStyle)GetValue(ChartStyleProperty); }
            set { SetValue(ChartStyleProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /* Property: MaxX
        
           The maximal value in the X-Direction. If MaxX <= MinX, an ArgumentException occurs.

           Default Value:

           1
        */
        public double MaxX
        {
            get { return (double)GetValue(MaxXProperty); }
            set { SetValue(MaxXProperty, value); }
        }

        /* Property: MaxY
        
           The maximal value in the Y-Direction. If MaxY <= MinY, an ArgumentException occurs.

           Default Value:

           1
        */
        public double MaxY
        {
            get { return (double)GetValue(MaxYProperty); }
            set { SetValue(MaxYProperty, value); }
        }

        /* Property: MinX
        
           The minimal value in the X-Direction. If MinX >= MaxX, an ArgumentException occurs.

           Default Value:

           0
        */
        public double MinX
        {
            get { return (double)GetValue(MinXProperty); }
            set { SetValue(MinXProperty, value); }
        }

        /* Property: MinY
        
           The minimal value in the Y-Direction. If MinY >= MaxY, an ArgumentException occurs.

           Default Value:

           0
        */
        public double MinY
        {
            get { return (double)GetValue(MinYProperty); }
            set { SetValue(MinYProperty, value); }
        }

        /* Property: Title
         
           The title of the <LineChart>. It is displayed above the chart and is centered. If null, no text will be displayed.

           Default Value:

           null
        */
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /* Property: XLabel
        
           The label of the X-Axis. It is only displayed if XLabel is not null and <ChartStyle.XAxisStyle> is not null.

           Default Value:

           null
        */
        public string XLabel
        {
            get { return (string)GetValue(XLabelProperty); }
            set { SetValue(XLabelProperty, value); }
        }

        /* Property: YLabel
        
           The label of the Y-Axis. It is only displayed if YLabel is not null and <ChartStyle.YAxisStyle> is not null.

           Default Value:

           null
        */
        public string YLabel
        {
            get { return (string)GetValue(YLabelProperty); }
            set { SetValue(YLabelProperty, value); }
        }

        public ItemCollection Items { get { return itemsControl.Items; } }

        #endregion

        #endregion

        #region Helper Functions

        #region PointCollection

        private bool PointInBounds(Point point, double minX, double maxX, double minY, double maxY)
        {
            return (point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY);          
        }

        private Point? FindIntersectionPoint(Point start, Point end, double? line, bool xAxis)
        {
            if (!(line is null))
            {
                Point intersection;

                Vector vec = end - start;
                vec.Normalize();

                if (xAxis)
                {
                    double t = (line.Value - start.X) / vec.X;
                    intersection = start + t * vec;
                }
                else
                {
                    double t = (line.Value - start.Y) / vec.Y;
                    intersection = start + t * vec;
                }
                return intersection;
            }
            return null;
        }

        private void SortPointCollection(PointCollection collection)
        {
            var points = collection.ToArray();
            var sortedPoints = points.OrderBy(point => point.X).ThenBy(point => point.Y).ToArray();
            collection.Clear();
            foreach (Point point in sortedPoints)
                collection.Add(point);
        }

        private bool PointsAreEqual(Point first, Point second)
        {
            return first.X < second.X + Epsilon && first.X > second.X - Epsilon && first.Y < second.Y + Epsilon && first.Y > second.Y - Epsilon;
        }

        private PointCollection GetRangeFromCollection(PointCollection collection, int start, int end)
        {
            PointCollection range = new PointCollection();

            for(int i = start; i < end; i++)
                range.Add(collection[i]);

            return range;
        }

        private List<int> ClampPointCollection(PointCollection collection)
        {
            List<int> splittingPoints = new List<int>();
            var points = collection.ToArray();
            collection.Clear();
            Point lastPoint = new Point(double.NaN, double.NaN);
            if (points.Count() > 0)
            {
                bool outOfBounds = true;
                var firstPoint = points[0];
                if (PointInBounds(firstPoint, MinX, MaxX, MinY, MaxY))
                    outOfBounds = false;

                int counter = 0;
                foreach (Point point in points)
                {
                    if (!PointInBounds(point, MinX, MaxX, MinY, MaxY))
                    {
                        if (!outOfBounds)
                        {
                            Point intersection;
                            double? yLimit = null;
                            double? xLimit = null;
                            if (point.Y < MinY)
                                yLimit = MinY;
                            else if (point.Y > MaxY) yLimit = MaxY;

                            if (point.X < MinX)
                                xLimit = MinX;
                            else if (point.X > MaxX) xLimit = MaxX;

                            Point? intersectionX = FindIntersectionPoint(lastPoint, point, xLimit, true);
                            Point? intersectionY = FindIntersectionPoint(lastPoint, point, yLimit, false);

                            if (!(intersectionX is null) && !(intersectionY is null))
                            {
                                if (PointInBounds(intersectionX.Value, MinX, MaxX, MinY, MaxY))
                                    intersection = intersectionX.Value;
                                else intersection = intersectionY.Value;
                            }
                            else if (!(intersectionX is null)) intersection = intersectionX.Value;
                            else intersection = intersectionY.Value;


                            if (!PointsAreEqual(lastPoint, intersection))
                            {
                                collection.Add(intersection);
                                splittingPoints.Add(counter);
                                counter++;
                            }
                        }
                        outOfBounds = true;
                    }
                    else
                    {
                        if (outOfBounds)
                        {
                            Point intersection;
                            double? yLimit = null;
                            double? xLimit = null;
                            if (lastPoint.Y < MinY)
                                yLimit = MinY;
                            else if (lastPoint.Y > MaxY) yLimit = MaxY;

                            if (lastPoint.X < MinX)
                                xLimit = MinX;
                            else if (lastPoint.X > MaxX) xLimit = MaxX;

                            Point? intersectionX = FindIntersectionPoint(lastPoint, point, xLimit, true);
                            Point? intersectionY = FindIntersectionPoint(lastPoint, point, yLimit, false);

                            if (!(intersectionX is null) && !(intersectionY is null))
                            {
                                if (PointInBounds(intersectionX.Value, MinX, MaxX, MinY, MaxY))
                                    intersection = intersectionX.Value;
                                else intersection = intersectionY.Value;
                            }
                            else if (!(intersectionX is null)) intersection = intersectionX.Value;
                            else intersection = intersectionY.Value;

                            if (!PointsAreEqual(point, intersection))
                            {
                                collection.Add(intersection);
                                splittingPoints.Add(counter);
                                counter++;
                            }
                        }
                        outOfBounds = false;
                        collection.Add(point);
                        counter++;
                    }


                    lastPoint = point;

                }
            }
            return splittingPoints;
        }

        private void NormalizePointCollection(PointCollection collection, double sizeX, double sizeY)
        {
            double factorX = sizeX / (MaxX - MinX);
            double factorY = sizeY / (MaxY - MinY);

            var points = collection.ToArray();
            collection.Clear();

            foreach(Point point in points)
                collection.Add(new Point((point.X - MinX) * factorX, sizeY - (point.Y - MinY) * factorY));
        }

        #endregion

        #endregion

        #region Load

        private void AdjustViewSeries()
        {
            ViewSeries.Clear();
            foreach (var series in LineSeries)
            {
                var originalColl = series.Points;
                if (!(originalColl is null))
                {
                    var viewColl = new PointCollection();
                    foreach (var point in originalColl)
                        viewColl.Add(new Point(point.X, point.Y));
                    SortPointCollection(viewColl);
                    var intersectionPoints = ClampPointCollection(viewColl);

                    int lastPoint = 0;

                    bool outOfBounds = false;
                    List<PointCollection> collectionsToRemove = new List<PointCollection>();
                    foreach (int idx in intersectionPoints)
                    {
                        PointCollection newCollection = new PointCollection();
                        var point = viewColl[idx];
                        if (PointsAreEqual(point, new Point(point.X, MaxY)) || PointsAreEqual(point, new Point(point.X, MinY)))
                        {
                            if (!outOfBounds)
                            {
                                newCollection = GetRangeFromCollection(viewColl, lastPoint, idx + 1);
                                outOfBounds = true;
                            }
                            else
                            {
                                lastPoint = idx;
                                outOfBounds = false;
                            }

                        }

                        if (newCollection.Count > 0)
                        {
                            collectionsToRemove.Add(newCollection);
                            LineData viewSeries = new LineData
                            {
                                DrawStyle = series.DrawStyle,
                                Points = newCollection,
                                Outline = series.Outline,
                                OutlineThickness = series.OutlineThickness,
                                Fill = series.Fill,
                                Thickness = series.Thickness,
                            };


                            ViewSeries.Add(viewSeries);
                        }
                    }

                    foreach (var pointSeries in collectionsToRemove)
                    {
                        foreach (var point in pointSeries)
                            viewColl.Remove(point);
                    }

                    if (viewColl.Count > 0)
                    {
                        LineData rest = new LineData
                        {
                            DrawStyle = series.DrawStyle,
                            Points = viewColl,
                            Outline = series.Outline,
                            OutlineThickness = series.OutlineThickness,
                            Fill = series.Fill,
                            Thickness = series.Thickness
                        };
                        ViewSeries.Add(rest);
                    }

                }
            }
        }

        private void LoadGraph()
        {
            ((INotifyCollectionChanged)Items).CollectionChanged += OnItemCollectionChanged;
            OnItemCollectionChanged(Items, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            AdjustViewSeries();
        }

        #endregion

        #region Render


        private void UpdateTextblockLayout(TextBlock block, Color textColor, string binding, double fontSize, double rotation)
        {
            Binding textBinding = new Binding(binding)
            {
                Source = this
            };
            block.SetBinding(TextBlock.TextProperty, textBinding);
            block.FontSize = fontSize;
            block.Foreground =  new SolidColorBrush(textColor);

            block.RenderTransform = new RotateTransform(rotation);
            block.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            block.Arrange(new Rect(block.DesiredSize));
        }

        private void UpdateTextLayouts()
        {

            if (!(ChartStyle is null))
            {
                var xStyle = ChartStyle.XAxisStyle;
                var yStyle = ChartStyle.YAxisStyle;

                if(!(Title is null)) UpdateTextblockLayout(TitleBlock, ChartStyle.TitleColor, "Title", ChartStyle.TitleFontSize, 0.0f);
                if(!(YLabel is null) && !(xStyle is null)) UpdateTextblockLayout(YLabelBlock, yStyle.Color, "YLabel", yStyle.FontSize, 270.0f);
                if(!(XLabel is null) && !(yStyle is null)) UpdateTextblockLayout(XLabelBlock, xStyle.Color, "XLabel", xStyle.FontSize, 0.0f);

                MarginYTop += TitleBlock.ActualHeight;
                MarginXLeft += YLabelBlock.ActualHeight;
                MarginYBottom += XLabelBlock.ActualHeight;

                double graphSizeX = ActualWidth - ChartStyle.PaddingX * 2 - MarginXLeft - MarginXRight;
                double graphSizeY = ActualHeight - ChartStyle.PaddingY * 2 - MarginYTop - MarginYBottom;


                if (!(Title is null))
                    TitleBlock.Margin = new Thickness(CenterX - TitleBlock.ActualWidth / 2.0f, 0, 0, 0);

                if (!(YLabel is null) && !(xStyle is null))
                    YLabelBlock.Margin = new Thickness(0, MarginYTop + ChartStyle.PaddingY + graphSizeY / 2.0f + YLabelBlock.ActualWidth / 2.0f, 0, 0);

                if (!(XLabel is null) && !(yStyle is null))
                    XLabelBlock.Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX + graphSizeX / 2.0f - XLabelBlock.ActualWidth / 2.0f, 2 * CenterY - XLabelBlock.ActualHeight, 0, 0);

                Elements.Add(TitleBlock);
                Elements.Add(XLabelBlock);
                Elements.Add(YLabelBlock);
            }
        }

        private void UpdateAxisLayout(AxisStyle style, bool isXAxis)
        {
            double graphSizeX, graphSizeY;

            graphSizeX = ActualWidth - MarginXLeft - MarginXRight - 2 * ChartStyle.PaddingX;
            graphSizeY = ActualHeight - MarginYTop - MarginYBottom - 2 * ChartStyle.PaddingY;

            if (!(style is null) && style.IsVisible)
            {
                Line axis;
                if (isXAxis) axis = XAxisLine;
                else axis = YAxisLine;
                
                axis.X1 = MarginXLeft + ChartStyle.PaddingX;
                axis.Y1 = MarginYTop + ChartStyle.PaddingY + graphSizeY;
                if (isXAxis)
                {
                    axis.X2 = axis.X1 + graphSizeX;
                    axis.Y2 = axis.Y1;
                }
                else
                {
                    axis.X2 = axis.X1;
                    axis.Y2 = MarginYTop + ChartStyle.PaddingY;
                }

                axis.Stroke = new SolidColorBrush(style.Color);
                axis.StrokeThickness = style.Thickness;

                Elements.Add(axis);

                if (style.DrawMarkings)
                {

                    var markings = style.Markings;

                    if (markings is null)
                    {
                        List<double> stdMarkings = new List<double>();

                        double min, max;
                        if(isXAxis) { min = MinX; max = MaxX; }
                        else { min = MinY; max = MaxY; }

                        double interval = (max - min) / NrStdMarkings;
                        for (int i = 0; i < NrStdMarkings; i++)
                            stdMarkings.Add(min + i * interval);
                        markings = stdMarkings.ToArray();
                    }

                    var length = style.MarkingLength;

                    PointCollection markingPoints = new PointCollection();

                    foreach (var marking in markings)
                    {
                        if (isXAxis && marking >= MinX && marking <= MaxX) markingPoints.Add(new Point(marking, MinY));
                        else if(marking >= MinY  && marking <= MaxY) markingPoints.Add(new Point(MinX, marking));
                    }

                    SortPointCollection(markingPoints);
                    ClampPointCollection(markingPoints);
                    NormalizePointCollection(markingPoints, graphSizeX, graphSizeY);

                    foreach (var point in markingPoints)
                    {
                        Point start, end;
                        Line marking = new Line();
                        if (isXAxis)
                        {
                            start = new Point(point.X, point.Y - (length / 2.0f));
                            end = new Point(point.X, point.Y + (length / 2.0f));
                        }
                        else
                        {
                            start = new Point(point.X - (length / 2.0f), point.Y);
                            end = new Point(point.X + (length / 2.0f), point.Y);
                        }

                        marking.X1 = start.X;
                        marking.X2 = end.X;
                        marking.Y1 = start.Y;
                        marking.Y2 = end.Y;
                        marking.StrokeThickness = style.MarkingThickness;
                        marking.Stroke = new SolidColorBrush(style.Color);
                        marking.Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX, MarginYTop + ChartStyle.PaddingY, 0, 0);
                        Markings.Add(marking);
                    }

                    if (style.DrawNumbers)
                    {
                        int counter = 0;
                        foreach (var point in markingPoints)
                        {
                            while (!(isXAxis && markings[counter] >= MinX && markings[counter] <= MaxX
                                || !isXAxis && markings[counter] >= MinY && markings[counter] <= MaxY))
                                counter++;

                            TextBlock number = new TextBlock
                            {
                                Text = markings[counter].ToString()
                            };

                            if (!isXAxis)
                                number.RenderTransform = new RotateTransform(270.0f);
                            number.Foreground = new SolidColorBrush(style.Color);
                            number.FontSize = style.NumberFontSize;

                            number.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            number.Arrange(new Rect(number.DesiredSize));

                            if (isXAxis)
                                number.Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX + point.X - (number.ActualWidth / 2.0f), MarginYTop + ChartStyle.PaddingY + point.Y + length, 0.0f, 0.0f);
                            else number.Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX + point.X - length - number.ActualHeight, MarginYTop + ChartStyle.PaddingY + point.Y + (number.ActualWidth / 2.0f), 0.0f, 0.0f);

                            Numbers.Add(number);
                            counter++;
                        }
                    }

                    if(style.DrawArrow)
                    {
                        Polygon poly = new Polygon();
                        PointCollection trianglePoints = new PointCollection();

                        if (isXAxis)
                        {
                            Point root = new Point(MarginXLeft + ChartStyle.PaddingX + graphSizeX, MarginYTop + ChartStyle.PaddingY + graphSizeY - (length / 2.0f));
                            trianglePoints.Add(root);
                            trianglePoints.Add(new Point(MarginXLeft + ChartStyle.PaddingX + graphSizeX, MarginYTop + ChartStyle.PaddingY + graphSizeY + (length / 2.0f)));
                            trianglePoints.Add(new Point(MarginXLeft + ChartStyle.PaddingX + graphSizeX + length, MarginYTop + ChartStyle.PaddingY + graphSizeY));
                            trianglePoints.Add(root);
                        }
                        else
                        {
                            Point root = new Point(MarginXLeft + ChartStyle.PaddingX - (length / 2.0f), MarginYTop + ChartStyle.PaddingY);
                            trianglePoints.Add(root);
                            trianglePoints.Add(new Point(MarginXLeft + ChartStyle.PaddingX + (length / 2.0f), MarginYTop + ChartStyle.PaddingY));
                            trianglePoints.Add(new Point(MarginXLeft + ChartStyle.PaddingX, MarginYTop + ChartStyle.PaddingY - length));
                            trianglePoints.Add(root);
                        }

                        poly.Points = trianglePoints;
                        poly.Fill = new SolidColorBrush(style.Color);
                        poly.Stroke = new SolidColorBrush(style.Color);

                        Arrows.Add(poly);
                    }
                }
            }
        }

        private void UpdateAxisLayouts()
        {
            Markings.Clear();
            Numbers.Clear();
            Arrows.Clear();

            UpdateAxisLayout(ChartStyle.XAxisStyle, true);
            UpdateAxisLayout(ChartStyle.YAxisStyle, false);

            foreach (Line line in Markings)
                Elements.Add(line);
            foreach (TextBlock number in Numbers)
                Elements.Add(number);
            foreach (Polygon arrow in Arrows)
                Elements.Add(arrow);
        }

        private void UpdateContentLayout()
        {
            double graphSizeX = ActualWidth - MarginXLeft - MarginXRight - 2 * ChartStyle.PaddingX;
            double graphSizeY = ActualHeight - MarginYTop - MarginYBottom - 2 * ChartStyle.PaddingY;

            foreach (var series in ViewSeries)
            {
                var pointColl = series.Points;
                var normalizedColl = new PointCollection();
                foreach (Point point in pointColl)
                    normalizedColl.Add(new Point(point.X, point.Y));

                NormalizePointCollection(normalizedColl, graphSizeX, graphSizeY);


                if (series.DrawStyle == LineData.PresentationStyle.Lines
                    || series.DrawStyle == LineData.PresentationStyle.DashedLines)
                {

                    Polyline line = new Polyline
                    {
                        Points = normalizedColl,
                        Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX, MarginYTop + ChartStyle.PaddingY, MarginXRight + ChartStyle.PaddingX, MarginYBottom + ChartStyle.PaddingY),
                        Stroke = series.Outline,
                        StrokeThickness = series.Thickness,
                        Fill = series.Fill
                    };
                    if(series.DrawStyle == LineData.PresentationStyle.DashedLines)
                    {
                        line.StrokeDashArray = new DoubleCollection() { 3, 1 };
                    }


                    Elements.Add(line);
                }
                else if(series.DrawStyle == LineData.PresentationStyle.Point)
                {
                    foreach(var point in normalizedColl)
                    {
                        var ellipse = new Ellipse()
                        {
                            Width = series.Thickness,
                            Height = series.Thickness,
                            Stroke = series.Outline,
                            StrokeThickness = series.OutlineThickness,
                            Fill = series.Fill
                        };
                        ellipse.Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX + point.X - series.Thickness / 2.0f, MarginYTop + ChartStyle.PaddingY + point.Y - series.Thickness / 2.0f, 0, 0);
                        Elements.Add(ellipse);
                    }
                }
                else if(series.DrawStyle == LineData.PresentationStyle.Diamond)
                {
                    foreach(var point in normalizedColl)
                    {
                        var rectangle = new Rectangle()
                        {
                            Width = series.Thickness,
                            Height = series.Thickness,
                            Stroke = series.Outline,
                            StrokeThickness = series.OutlineThickness,
                            Fill = series.Fill,
                        };
                        rectangle.RenderTransform = new RotateTransform(45.0f);
                        rectangle.Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX + point.X - series.Thickness / 2.0f, MarginYTop + ChartStyle.PaddingY + point.Y - series.Thickness / 2.0f, 0, 0);
                        Elements.Add(rectangle);
                    }
                }
                

            }
        }

        private void UpdateGridLayout()
        {
            
            double graphSizeX, graphSizeY;

            graphSizeX = ActualWidth - MarginXLeft - MarginXRight - 2 * ChartStyle.PaddingX;
            graphSizeY = ActualHeight - MarginYTop - MarginYBottom - 2 * ChartStyle.PaddingY;

            var style = ChartStyle.GridStyle;

            if(!(style is null) && style.IsVisible && graphSizeX > 0 && graphSizeY > 0)
            {
                Rectangle background = new Rectangle
                {
                    Stroke = new SolidColorBrush(style.Background),
                    Fill = new SolidColorBrush(style.Background),
                    Width = graphSizeX,
                    Height = graphSizeY,

                    Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX, MarginYTop + ChartStyle.PaddingY, 0.0f, 0.0f)
                };
                Canvas.SetZIndex(background, -2);

                Grid.Clear();
                if (style.IntervalX != 0.0f)
                {
                    for (double x = MinX; x < MaxX; x += Math.Abs(style.IntervalX))
                    {
                        Line line = new Line
                        {
                            Stroke = new SolidColorBrush(style.Foreground),
                            Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX, MarginYTop + ChartStyle.PaddingY, MarginXRight + ChartStyle.PaddingX, MarginYBottom + ChartStyle.PaddingY),
                            StrokeThickness = style.Thickness
                        };
                        Canvas.SetZIndex(line, -1);

                        Point start, end;

                        start = new Point(x, MinY);
                        end = new Point(x, MaxY);

                        PointCollection collection = new PointCollection() { start, end };

                        ClampPointCollection(collection);
                        NormalizePointCollection(collection, graphSizeX, graphSizeY);

                        if (collection.Count() == 2)
                        {
                            start = collection[0];
                            end = collection[1];
                        }

                        line.X1 = start.X;
                        line.X2 = end.X;
                        line.Y1 = start.Y;
                        line.Y2 = end.Y;

                        Grid.Add(line);
                    }
                }
                else throw new ArgumentException("IntervalX of GridStyle is not allowed to be 0");

                if (style.IntervalY != 0.0f)
                {
                    for (double y = MinY; y < MaxY; y += Math.Abs(style.IntervalY))
                    {
                        Line line = new Line
                        {
                            Stroke = new SolidColorBrush(style.Foreground),
                            Margin = new Thickness(MarginXLeft + ChartStyle.PaddingX, MarginYTop + ChartStyle.PaddingY, MarginXRight + ChartStyle.PaddingX, MarginYBottom + ChartStyle.PaddingY),
                            StrokeThickness = style.Thickness
                        };
                        Canvas.SetZIndex(line, -1);

                        Point start, end;

                        start = new Point(MinX, y);
                        end = new Point(MaxX, y);

                        PointCollection collection = new PointCollection() { start, end };

                        ClampPointCollection(collection);
                        NormalizePointCollection(collection, graphSizeX, graphSizeY);

                        if (collection.Count() == 2)
                        {
                            start = collection[0];
                            end = collection[1];
                        }

                        line.X1 = start.X;
                        line.X2 = end.X;
                        line.Y1 = start.Y;
                        line.Y2 = end.Y;

                        Grid.Add(line);
                    }
                }
                else throw new ArgumentException("IntervalY of GridStyle is not allowed to be 0");

                GridBackground = background;
                Elements.Add(background);
                foreach (Line line in Grid)
                    Elements.Add(line);
            }
        }

        private void UpdateGraphLayout()
        {
            if (IsVisible && IsLoaded && !(ChartStyle is null))
            {
                foreach (var element in Elements)
                    Items.Remove(element);
                Elements.Clear();

                MarginYTop = 0;
                MarginYBottom = 0;
                MarginXLeft = 0;
                MarginXRight = 0;
                CenterX = ActualWidth / 2.0f;
                CenterY = ActualHeight / 2.0f;

                AdjustViewSeries();
                
                UpdateTextLayouts();
                if (ChartStyle.DrawGrid) UpdateGridLayout();
                if (ChartStyle.DrawAxis) UpdateAxisLayouts();
                UpdateContentLayout();

                MinHeight = MarginYTop + 2 * ChartStyle.PaddingY + MarginYBottom;

                foreach (var element in Elements)
                    Items.Add(element);
            }
        }

        #endregion



        #region Events

        protected void OnItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LineSeries.Clear();

            foreach(var item in Items)
            {
                var series = item as LineData;
                if (!(series is null))
                    LineSeries.Add(series);
            }
        }

        protected void OnItemChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateVisual();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadGraph();
            InvalidateVisual();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateGraphLayout();
            base.OnRender(drawingContext);
        }

        #endregion


    }
}
