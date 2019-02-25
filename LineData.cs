using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChartSharp
{
    /* Class: LineData
    
       This class is used to represent the data displayed in a <LineChart>. The data is handed over
       to a <LineChart> via XAML. You can add data to a chart in code by adding an instance of this class to the property Items of
       an instance of <LineChart>.
    */
    public class LineData : Control
    {
        /* Constructor: LineData
        
           Initializes all properties to their default values.
        */
        public LineData()
        {
            Points = new PointCollection();
            Outline = new SolidColorBrush(Colors.Black);
            Fill = new SolidColorBrush(Colors.Transparent);
            DrawStyle = PresentationStyle.Lines;
            OutlineThickness = 1;
            Thickness = 1;
        }

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(PointCollection), typeof(LineData));
        public static readonly DependencyProperty OutlineProperty = DependencyProperty.Register("Outline", typeof(Brush), typeof(LineData));
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(LineData));
        public static readonly DependencyProperty DrawStyleProperty = DependencyProperty.Register("DrawStyle", typeof(PresentationStyle), typeof(LineData));
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(double), typeof(LineData));
        public static readonly DependencyProperty OutlineThicknessProperty = DependencyProperty.Register("OutlineThickness", typeof(double), typeof(LineData));

        /* Property: DrawStyle
        
           Affects the form of the rendering of the line.
           Possible values are:

           - Lines: Represents <Points> as a continuous line.
           - DashedLines: Same as lines. However, the line is dashed.
           - Point: Will draw <Points> as small circles.
           - Diamond: Same as Point; instead of small circles, it will draw squares rotated by 45 degrees("Diamonds")

           Default Value:

           Lines
        */
        public PresentationStyle DrawStyle
        {
            get { return (PresentationStyle)GetValue(DrawStyleProperty); }
            set { SetValue(DrawStyleProperty, value); }
        }

        /* Property: Fill
        
           A Brush, used to fill rendered shapes. If <DrawStyle> is either Point or Diamond, this property is used as
           the Brush to fill out those shapes. If it is either Lines or DashedLines, it is used to try to fill a Polygon spanned
           by the internally created Polyline.

           Default Value:

           SolidColorBrush(Colors.Transparent)
        */
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /* Property: Outline
        
           A Brush, used to render the outline of shapes. If <DrawStyle> is either Point or Diamond, this property is used as the
           Brush for the outline of those shapes. If it is either Lines or DashedLines, it is used as the Brush for the created
           Lines.

           Default Value:

           SolidColorsBrush(Colors.Black)
        */
        public Brush Outline
        {
            get { return (Brush)GetValue(OutlineProperty); }
            set { SetValue(OutlineProperty, value); }
        }

        /* Property: OutlineThickness
        
           This property is only used if <DrawStyle> is either Point or Diamond. As the name suggests, this value specifies
           the thickness of the outline of shapes.

           Default Value:

           1
        */
        public double OutlineThickness
        {
            get { return (double)GetValue(OutlineThicknessProperty); }
            set { SetValue(OutlineThicknessProperty, value); }
        }

        /* Property: Points
        
           The collection of Points used for a <LineChart>. The collection is not required to be sorted in any way. However,
           it should be noted that it will be sorted by X-Values internally before it is used.

           Default Value:

           Constructor of PointCollection
        */
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        /* Property: Thickness
           
           Specifies the thickness of lines or shapes. 

           Default Value:

           1
        */
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        public enum PresentationStyle
        {
            Lines, DashedLines, Point, Diamond
        }


    }
}
