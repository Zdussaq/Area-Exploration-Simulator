using CPE400Project.EnvironmentData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CPE400Project.MapDisplay
{
    /// <summary>
    /// Creates a display for the drones and current map.
    /// Non-discovered regions will be shaded gray while discoverd will show elevation
    /// </summary>
    public class MapDisplay : UserControl
    {
        #region Constructors
        public MapDisplay()
        {

            ParentCanvas = new Canvas();

            MapImagePane = new Image();
            ParentCanvas.Children.Add(MapImagePane);

            //Make this element's content that of the parent canvas.
            //this means the user will see the Parent Canvas.
            Content = ParentCanvas;

            DefaultStyleKeyProperty.OverrideMetadata(typeof(MapDisplay), new FrameworkPropertyMetadata(typeof(MapDisplay)));

        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Container for all imagedata of the map. Both drones and 
        /// </summary>
        public Canvas ParentCanvas { get; set; }
        public Image MapImagePane { get; set; }
        public BitmapSource MapImage { get; set; }

        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(Map), typeof(MapDisplay));
        public Map Map
        {
            get
            { 
                return (Map)GetValue(MapProperty);
            }
            set 
            {
                SetValue(MapProperty, value);
                DrawMap();
            }
        }

        #endregion Properties

        #region Public Functions

        public void DrawMap()
        {
            ParentCanvas.Width = Map.Width;
            ParentCanvas.Height = Map.Height;


            PixelFormat pf = PixelFormats.Bgr32;

            int rawStride = (Map.Width * pf.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * Map.Height];

            Random rand = new Random();
            rand.NextBytes(rawImage);

            MapImage = BitmapSource.Create(Map.Width, Map.Height, 96, 96, pf, null, rawImage, rawStride);

            MapImagePane.Width = Map.Width;
            MapImagePane.Height = Map.Height;

            MapImagePane.Source = MapImage;
        }
            

        

        #endregion Public Functions

    }
}
