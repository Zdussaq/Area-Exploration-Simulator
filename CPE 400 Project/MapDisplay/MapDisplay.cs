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

            for (int i = 0; i < Map.Height; i++)
            {
                for (int j = 0; j < rawStride; j += 4)
                {
                    int index = (i * rawStride) + j;
                    int actualJ = j / 4;

                    if (Map[i][actualJ].Elevation < 128)
                    {
                        rawImage[index] = 255;//B
                        rawImage[index + 1] = 80; //G
                        rawImage[index + 2] = 0; //R
                        rawImage[index + 3] = 0;
                    }
                    else
                    {
                        rawImage[index] = (byte)Map[i][actualJ].Elevation;//B
                        rawImage[index + 1] = (byte)Map[i][actualJ].Elevation; //G
                        rawImage[index + 2] = (byte)Map[i][actualJ].Elevation; //R
                        rawImage[index + 3] = 0;
                    }
                    
                }
            }

            MapImage = BitmapSource.Create(Map.Width, Map.Height, 96, 96, pf, null, rawImage, rawStride);

            MapImagePane.Width = Map.Width;
            MapImagePane.Height = Map.Height;


            MapImage = BitmapSource.Create(Map.Width, Map.Height, 96, 96, pf, null, rawImage, rawStride);

            WriteableBitmap bitmap = new WriteableBitmap(MapImage);

            MapImagePane.Source = bitmap;

            /*
            int rawStrideTest = (300 * pf.BitsPerPixel + 7) / 8;
            byte[] editArea = new byte[rawStrideTest * 300];

            if (Map.Height > 600)
            {
                for (int j = 0; j < 100; j++)
                {
                    for (int i = 0; i < editArea.Length; i += 4)
                    {
                        editArea[i] = 255; //B
                        editArea[i + 1] = 0; //G
                        editArea[i + 2] = 255; //R
                        editArea[i + 3] = 0;
                    }

                    bitmap.WritePixels(
                        new Int32Rect(0 + j, 0 + j, 300, 300),
                        editArea,
                        rawStrideTest,
                        0
                        );

                    MapImagePane.Source = bitmap;
                }
            }
            else
            {
                MapImagePane.Source = bitmap;

            }



            byte[] colorData = { 0, 0, 0, 0 };
            */

        }
            

        

        #endregion Public Functions

    }
}
