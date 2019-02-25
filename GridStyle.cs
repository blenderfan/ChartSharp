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
    /* Class: GridStyle
     
       Defines the rendering style of a grid in the background of a chart. It is a member of <ChartStyle>.
    */
    public class GridStyle : INotifyPropertyChanged
    {
        /* Constructor: GridStyle 
        
           Initializes all properties to their default values.
        */
        public GridStyle()
        {
            IsVisible = true;
            Foreground = Colors.Black;
            Background = Colors.Transparent;
            IntervalX = 1;
            IntervalY = 1;
            Thickness = 0.1f;
        }

        private Color background;
        private Color foreground;
        private double intervalX;
        private double intervalY;
        private bool isVisible;
        private double thickness;

        /* Property: Background
         
           The background color for the grid. More specifically, this is the color for everything between the grid lines.

           Default Value:

           Colors.Transparent
        */
        public Color Background
        {
            get { return background; }
            set
            {
                if (value != background)
                {
                    background = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: Foreground
         
           The foreground color of the grid. This is the color of the lines of the grid.

           Default Value:

           Colors.Black
        */
        public Color Foreground
        {
            get { return foreground; }
            set
            {
                if (value != foreground)
                {
                    foreground = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IntervalX
        
           This interval determines how often lines are drawn in the X-Direction. The lines itself are drawn in the Y-Direction.
           If set to 0, <LineChart> will throw an ArgumentException.

           Default Value:
        
           1
        */
        public double IntervalX
        {
            get { return intervalX; }
            set
            {
                if (value != intervalX)
                {
                    intervalX = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /* Property: IntervalY
         
           This interval determines how often lines are drawn in the Y-Direction. The lines itself are drawn in the X-Direction.
           If set to 0, <LineChart> will throw an ArgumentException.

           Default Value:

           1
        */
        public double IntervalY
        {
            get { return intervalY; }
            set
            {
                if (value != intervalY)
                {
                    intervalY = value;
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
