using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ChartSharp
{
    /* Class: SurfaceData
    
       This class is used to represent the data displayed in a <SurfaceChart>. The data is handed over
       to a <SurfaceChart> via XAML. You can add data to a chart in code by adding an instance of this class to the property Items of
       an instance of <SurfaceChart>.
    */
    public class SurfaceData : Control
    {
        /* Constructor: SurfaceData
        
           Initializes all properties to their default values.
        */
        public SurfaceData()
        {
            Outline = new SolidColorBrush(Colors.Black);
            Fill = new SolidColorBrush(Colors.Gray);
            DrawStyle = PresentationStyle.Lines;
            Thickness = 1;
            Points = null;
        }

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(double[,]), typeof(SurfaceData));
        public static readonly DependencyProperty OutlineProperty = DependencyProperty.Register("Outline", typeof(Brush), typeof(SurfaceData));
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(SurfaceData));
        public static readonly DependencyProperty DrawStyleProperty = DependencyProperty.Register("DrawStyle", typeof(PresentationStyle), typeof(SurfaceData));
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(double), typeof(SurfaceData));

        /* Property: DrawStyle
        
           Affects the form of the rendering of the surface.
           Possible values are:

           - Lines: Represents <Points> as a surface grid.
           - Area: Represents <Points> as a surface area. The polygons(Triangles) making up the surface are shaded flat.

           Default Value:

           Lines
        */
        public PresentationStyle DrawStyle
        {
            get { return (PresentationStyle)GetValue(DrawStyleProperty); }
            set { SetValue(DrawStyleProperty, value); }
        }

        /* Property: Fill
        
           A Brush, used to fill rendered shapes. If <DrawStyle> is either Area, this property is used as
           the Brush to paint the polygons of the surface. If it is Lines, this property is ignored.

           Default Value:

           SolidColorBrush(Colors.Gray)
        */
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /* Property: Outline
        
           A Brush, used to render the outline of shapes. If <DrawStyle> is Lines, this property is used as the
           Brush for the surface grid. If it is Area, this property is ignored.

           Default Value:

           SolidColorsBrush(Colors.Black)
        */
        public Brush Outline
        {
            get { return (Brush)GetValue(OutlineProperty); }
            set { SetValue(OutlineProperty, value); }
        }

        /* Property: Points
        
           The collection of Points used for a <SurfaceChart>. The first dimension represents the X-Direction, and
           the second dimension the Z-Direction. The distance between points in the X- and Z-Direction is uniformly adjusted, depending
           on the <SurfaceChart.MinX>, <SurfaceChart.MaxX>, <SurfaceChart.MinY> and <SurfaceChart.MaxY>. E.g. if
           the interval in X is [0;1] and the first dimension has a size of 10; the distance between each points is
           0,1.

           Default Value:

           null
        */
        public double[,] Points
        {
            get { return (double[,])GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        /* Property: Thickness
           
           Specifies the thickness of lines in a grid. Only applies if <DrawStyle> is Lines. 

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
            Lines, Area
        }


    }
}
