using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChartSharp
{
    /* Class: AxisStyle
    
       Defines the rendering style of an axis.
    */
    public class AxisStyle : INotifyPropertyChanged
    {
        /* Constructor: AxisStyle
        
           Initializes all properties to their default values
        */
        public AxisStyle()
        {
            IsVisible = true;
            DrawArrow = true;

            Thickness = 0.1;
            FontSize = 16;
            Color = Colors.Black;

            DrawMarkings = true;
            DrawNumbers = true;
            NumberFontSize = 12;
            Markings = null;
            MarkingThickness = 1;
            MarkingLength = 4;
        }

        private Color color;
        private bool drawArrow;
        private bool drawMarkings;
        private bool drawNumbers;
        private double fontSize;
        private bool isVisible;
        private double markingLength;
        private double[] markings;
        private double markingThickness;
        private double numberFontSize;
        private double thickness;


        /* Property: Color
         
           The color of the axis.

           Detail:

           The color of the arrows (If <DrawArrow> is true), the text label, the <Markings> (If <DrawMarkings> is true), 
           the numbers (If <DrawNumbers> and <DrawMarkings> is true) and the axis itself are affected.

           Default Value:

           Colors.Black
        */
        public Color Color
        {
            get { return color; }
            set
            {
                if (value != color)
                {
                    color = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: DrawArrow
          
           If true, an arrow is drawn at the end of the axis. Otherwise no arrow is rendered.

           Detail:

           The bounds of the polygon of the arrow are at the end (<LineChart.MaxX> or <LineChart.MaxY>) of the specified interval of the chart.
           E.g. If the interval of the X-Axis of a chart is [0;1], then the drawing of the arrow would
           start at position 1. 
           
           The "Thickness" of the arrow is the same as the length of the
           markings (<MarkingLength>). The length of the arrow is twice that of <MarkingLength>.

           Default Value:

           true
        */
        public bool DrawArrow
        {
            get { return drawArrow; }
            set
            {
                if(value != drawArrow)
                {
                    drawArrow = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: DrawMarkings
           
           If true, the axis will draw markings at positions defined by <Markings>.
         
           Default Value:

           true
        */
        public bool DrawMarkings
        {
            get { return drawMarkings; }
            set
            {
                if (value != drawMarkings)
                {
                    drawMarkings = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: DrawNumbers
         
           If <DrawMarkings> and this value are both true, numbers will be displayed besides the <Markings> of the axis. 
           The numbers represent the numerical values of the <Markings>. 

           Default Value:

           true
        */
        public bool DrawNumbers
        {
            get { return drawNumbers; }
            set
            {
                if (value != drawNumbers)
                {
                    drawNumbers = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: FontSize
           
           Specifies the size of the font for the Label besides or under the axis. E.g. for the X-Axis, it would determine the size of <LineChart.XLabel>.
           An exception can occur if this value is 0 or smaller.

           Default Value:

           16
        */
        public double FontSize
        {
            get { return fontSize; }
            set
            {
                if (value != fontSize)
                {
                    fontSize = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IsVisible
         
           If true, the axis will be rendered. Otherwise it is not visible

           Default Value:

           true
        */
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value != isVisible)
                {
                    isVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }


        /* Property: MarkingLength
         
           The length of the markings when rendered.

           Default Value:

           4
        */
        public double MarkingLength
        {
            get { return markingLength; }
            set
            {
                if (value != markingLength)
                {
                    markingLength = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: Markings
        
           An array, that specifies the Markings on the axis. Markings are only displayed if <DrawMarkings> is true.

           Detail:

           If <DrawMarkings> is true and <Markings> is null, the interval on the axis is divided into equal parts, seperating
           the axis into intervals. The number of intervals is 10 if unchanged. The standard value can be changed in <LineChart>.

           If some of the numerical values of the <Markings> is outside the interval specified for this axis in <LineChart>, the
           markings are not rendered as expected.

           Default Value:

           null
        */
        public double[] Markings
        {
            get { return markings; }
            set
            {
                if (value != markings)
                {
                    markings = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: MarkingThickness
         
           The thickness of the markings when rendered.

           Default Value:

           1
        */
        public double MarkingThickness
        {
            get { return markingThickness; }
            set
            {
                if (value != markingThickness)
                {
                    markingThickness = value;
                    NotifyPropertyChanged();
                }
            }
        }


        /* Property: NumberFontSize
        
           Specifies the size of the font for the numbers besides the axis(If <DrawNumbers> and <DrawMarkings> is true).
           An exception can occur if this value is 0 or smaller.  
           
           Default Value:

           12
        */
        public double NumberFontSize
        {
            get { return numberFontSize; }
            set
            {
                if (value != numberFontSize)
                {
                    numberFontSize = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: Thickness
           
           Specifies the thickness of the axis. 

           Default Value:

           0.1
        */
        public double Thickness
        {
            get { return thickness; }
            set
            {
                if(value != thickness)
                {
                    thickness = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
