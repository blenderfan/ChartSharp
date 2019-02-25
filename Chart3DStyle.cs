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
    /* Class: Chart3DStyle

       Defines the rendering style of a <SurfaceChart>.
    */
    public class Chart3DStyle : INotifyPropertyChanged
    {

        /* Constructor: Chart3DStyle

           Initializes all properties to their default values
        */
        public Chart3DStyle()
        {
            BackgroundColor = Colors.White;

            TitleFontSize = 24;
            TitleColor = Colors.Black;
            PaddingX = 30;
            PaddingY = 30;

            DrawGrid = true;
            DrawAxis = true;

            SizeX = 10;
            SizeY = 10;
            SizeZ = 10;
            XAxisStyle = new AxisStyle();
            YAxisStyle = new AxisStyle();
            ZAxisStyle = new AxisStyle();
            GridStyle = new Grid3DStyle();
        }

        private Color backgroundColor;
        private bool drawAxis;
        private bool drawGrid;
        private Grid3DStyle gridStyle;
        private double paddingX;
        private double paddingY;
        private double sizeX;
        private double sizeY;
        private double sizeZ;
        private Color titleColor;
        private double titleFontSize;
        private AxisStyle xAxisStyle;
        private AxisStyle yAxisStyle;
        private AxisStyle zAxisStyle;

        /* Property: BackgroundColor
        
           The color of the background of the <SurfaceChart>.

           Default Value:

           Colors.White
        */
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                if(value != backgroundColor)
                {
                    backgroundColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: DrawAxis
          
           If true, axis are drawn in the <SurfaceChart>. If one of the three <AxisStyle> members is null, the
           axis is not rendered. E.g. if <XAxisStyle> and <ZAxisStyle> are defined, but <YAxisStyle> is null, only the X-Axis will be rendered.

           Default Value:

           true
        */
        public bool DrawAxis
        {
            get { return drawAxis; }
            set
            {
                if(value != drawAxis)
                {
                    drawAxis = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: DrawGrid
        
           If true, a grid is drawn in the background of the <SurfaceChart>. If <GridStyle> is null, it will still be not rendered.

           Default Value:

           true
        */
        public bool DrawGrid
        {
            get { return drawGrid; }
            set
            {
                if(value != drawGrid)
                {
                    drawGrid = value;
                    NotifyPropertyChanged();
                }
            }
        }


        /* Property: GridStyle
        
           The style of the grid of the <SurfaceChart>. See <Grid3DStyle>.

           Default Value:

           Constructor of <Grid3DStyle>
        */
        public Grid3DStyle GridStyle
        { 
            get { return gridStyle; }
            set
            {
                if(value != gridStyle)
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

        /* Property: SizeX
        
           The actual size of a <SurfaceChart> in the X-Direction. The triple <SizeX>, <SizeY> and <SizeZ> form the
           three-dimensional aspect ratio of the chart.

           Default Value:

           10
        */
        public double SizeX
        {
            get { return sizeX; }
            set
            {
                if(value != sizeX)
                {
                    sizeX = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: SizeY
        
           The actual size of a <SurfaceChart> in the Y-Direction. The triple <SizeX>, <SizeY> and <SizeZ> form the
           three-dimensional aspect ratio of the chart.

           Default Value:

           10
        */
        public double SizeY
        {
            get { return sizeY; }
            set
            {
                if (value != sizeY)
                {
                    sizeY = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: SizeZ
        
           The actual size of a <SurfaceChart> in the Z-Direction. The triple <SizeX>, <SizeY> and <SizeZ> form the
           three-dimensional aspect ratio of the chart.

           Default Value:

           10
        */
        public double SizeZ
        {
            get { return sizeZ; }
            set
            {
                if(value != sizeZ)
                {
                    sizeZ = value;
                    NotifyPropertyChanged();
                }
            }
        }


        /* Property: TitleColor
        
           The color of the title (defined in <SurfaceChart>). 
           
           Default Value:

           Colors.Black
        */
        public Color TitleColor
        {
            get { return titleColor; }
            set
            {
                if(value != titleColor)
                {
                    titleColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: TitleFontSize
        
           The size of the font of the title of the <SurfaceChart>. An exception can occur if this value is 0 or smaller.

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

        /* Property: ZAxisStyle
        
           The style of the Z-Axis. See <AxisStyle>.

           Default Value:

           Constructor of <AxisStyle>
        */
        public AxisStyle ZAxisStyle
        {
            get { return zAxisStyle; }
            set
            {
                if(value != zAxisStyle)
                {
                    zAxisStyle = value;
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
