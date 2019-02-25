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
    /* Class: Grid3DStyle

       Defines the rendering style of a grid in a <SurfaceChart>. It is a member of <Chart3DStyle>.
    */
    public class Grid3DStyle : INotifyPropertyChanged
    {
        /* Constructor: GridStyle 

           Initializes all properties to their default values.
        */
        public Grid3DStyle()
        {
            ForegroundXY = Colors.Black;
            ForegroundXZ = Colors.Black;
            ForegroundYZ = Colors.Black;
            IntervalX = 1;
            IntervalY = 1;
            IntervalZ = 1;
            IsVisible = true;
            IsXYVisible = true;
            IsXZVisible = true;
            IsYZVisible = true;
            Thickness = 0.1f;
        }

        public Color foregroundXY;
        public Color foregroundXZ;
        public Color foregroundYZ;
        public double intervalX;
        public double intervalY;
        public double intervalZ;
        public bool isVisible;
        public bool isXYVisible;
        public bool isXZVisible;
        public bool isYZVisible;
        public double thickness;

        /* Property: ForegroundXY
         
           The foreground color of the grid in the XY-Plane. This is the color of the lines of the grid.

           Default Value:

           Colors.Black
        */
        public Color ForegroundXY
        {
            get { return foregroundXY; }
            set
            {
                if(value != foregroundXY)
                {
                    foregroundXY = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: ForegroundXZ
         
           The foreground color of the grid in the XZ-Plane. This is the color of the lines of the grid.

           Default Value:

           Colors.Black
        */
        public Color ForegroundXZ
        {
            get { return foregroundXZ; }
            set
            {
                if(value != foregroundXZ)
                {
                    foregroundXZ = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: ForegroundYZ
         
           The foreground color of the grid in the YZ-Plane. This is the color of the lines of the grid.

           Default Value:

           Colors.Black
        */
        public Color ForegroundYZ
        {
            get { return foregroundYZ; }
            set
            {
                if(value != foregroundYZ)
                {
                    foregroundYZ = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IntervalX
        
           This interval determines how often lines are drawn in the X-Direction. 
           If set to 0, <SurfaceChart> will throw an ArgumentException.

           Default Value:
        
           1
        */
        public double IntervalX
        {
            get { return intervalX; }
            set
            {
                if(value != intervalX)
                {
                    intervalX = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IntervalY
        
           This interval determines how often lines are drawn in the Y-Direction. 
           If set to 0, <SurfaceChart> will throw an ArgumentException.

           Default Value:
        
           1
        */
        public double IntervalY
        {
            get { return intervalY; }
            set
            {
                if(value != intervalY)
                {
                    intervalY = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IntervalZ
        
           This interval determines how often lines are drawn in the Z-Direction. 
           If set to 0, <SurfaceChart> will throw an ArgumentException.

           Default Value:
        
           1
        */
        public double IntervalZ
        {
            get { return intervalZ; }
            set
            {
                if(value != intervalZ)
                {
                    intervalZ = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IsVisible
         
           If true, the grid will be rendered. Otherwise it is not visible

           Default Value:

           true
        */
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if(value != isVisible)
                {
                    isVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IsXYVisible
         
           If true, the grid in the XY-Plane will be rendered. Otherwise it is not visible

           Default Value:

           true
        */
        public bool IsXYVisible
        {
            get { return isXYVisible; }
            set
            {
                if(value != isXYVisible)
                {
                    isXYVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IsXZVisible
         
           If true, the grid in the XZ-Plane will be rendered. Otherwise it is not visible

           Default Value:

           true
        */
        public bool IsXZVisible
        {
            get { return isXZVisible; }
            set
            {
                if(value != isXZVisible)
                {
                    isXZVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IsYZVisible
         
           If true, the grid in the YZ-Plane will be rendered. Otherwise it is not visible

           Default Value:

           true
        */
        public bool IsYZVisible
        {
            get { return isYZVisible; }
            set
            {
                if(value != isYZVisible)
                {
                    isYZVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: Thickness
           
           Specifies the thickness of lines of the grid. 

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
