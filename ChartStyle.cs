using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChartSharp
{
    /* Class: ChartStyle

       Defines the rendering style of a <LineChart>.
    */
    public class ChartStyle : INotifyPropertyChanged
    {
        /* Constructor: ChartStyle
        
           Initializes all properties to their default values
        */
        public ChartStyle()
        {
            TitleFontSize = 24;
            TitleColor = Colors.Black;
            PaddingX = 30;
            PaddingY = 30;

            DrawGrid = true;
            DrawAxis = true;

            XAxisStyle = new AxisStyle();
            YAxisStyle = new AxisStyle();
            GridStyle = new GridStyle();
        }

        private bool drawAxis;
        private bool drawGrid;
        private GridStyle gridStyle;
        private double paddingX;
        private double paddingY;
        private Color titleColor;
        private double titleFontSize;
        private AxisStyle xAxisStyle;
        private AxisStyle yAxisStyle;

        /* Property: DrawAxis
          
           If true, axis are drawn in the <LineChart>. If one or both <AxisStyle> members (<XAxisStyle> and <YAxisStyle>) are null, they
           are still not rendered. E.g. if <XAxisStyle> is defined, but <YAxisStyle> is null, only the X-Axis will be rendered.

           Default Value:

           true
        */
        public bool DrawAxis
        {
            get { return drawAxis; }
            set
            {
                if (value != drawAxis)
                {
                    drawAxis = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: DrawGrid
        
           If true, a grid is drawn in the background of the <LineChart>. If <GridStyle> is null, it will still be not rendered.

           Default Value:

           true
        */
        public bool DrawGrid
        {
            get { return drawGrid; }
            set
            {
                if (value != drawGrid)
                {
                    drawGrid = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: GridStyle
        
           The style of the grid of the <LineChart>. See <ChartSharp.GridStyle>.

           Default Value:

           Constructor of <ChartSharp.GridStyle>
        */
        public GridStyle GridStyle
        {
            get { return gridStyle; }
            set
            {
                if (value != gridStyle)
                {
                    gridStyle = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: PaddingX
        
           The distance in pixels from the left and right side of the border of the chart itself that are left empty.

           Default Value:

           30
        */
        public double PaddingX
        {
            get { return paddingX; }
            set
            {
                if(value != paddingX)
                {
                    paddingX = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: PaddingY
        
           The distance in pixels from the top and bottom side of the border of the chart itself that are left empty.

           Default Value:

           30
        */
        public double PaddingY
        {
            get { return paddingY; }
            set
            {
                if(value != paddingY)
                {
                    paddingY = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: TitleColor
        
           The color of the title (defined in <LineChart>). 
           
           Default Value:

           Colors.Black
        */
        public Color TitleColor
        {
            get { return titleColor; }
            set
            {
                if (value != titleColor)
                {
                    titleColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: TitleFontSize
        
           The size of the font of the title of the <LineChart>. An exception can occur if this value is 0 or smaller.

           Default Value:

           24
        */
        public double TitleFontSize
        {
            get { return titleFontSize; }
            set
            {
                if (value != titleFontSize)
                {
                    titleFontSize = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: XAxisStyle
        
           The style of the X-Axis. See <AxisStyle>.

           Default Value:

           Constructor of <AxisStyle>
        */
        public AxisStyle XAxisStyle
        {
            get { return xAxisStyle; }
            set
            {
                if(value != xAxisStyle)
                {
                    xAxisStyle = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: YAxisStyle
        
           The style of the Y-Axis. See <AxisStyle>.

           Default Value:

           Constructor of <AxisStyle>
        */
        public AxisStyle YAxisStyle
        {
            get { return yAxisStyle; }
            set
            {
                if(value != yAxisStyle)
                {
                    yAxisStyle = value;
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
