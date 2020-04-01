using CPE_400_Project.EnvironmentData;
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
            //Create new stackpanel to place objects in. This way it's easier to manage what's in there.
            //Adding a buggon or something for debuggin is easier this way.
            ParentContainer = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            MapImagePane = new Image();
            ParentContainer.Children.Add(MapImagePane);



            //Make this element's content that of the parent canvas.
            //this means the user will see the Parent Canvas.
            Content = ParentContainer;

            DefaultStyleKeyProperty.OverrideMetadata(typeof(MapDisplay), new FrameworkPropertyMetadata(typeof(MapDisplay)));

        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Main Container for image. All of the elements will be children of this.
        /// Stack panel gives easy additions and customizability.
        /// </summary>
        public StackPanel ParentContainer { get; set; }
        /// <summary>
        /// This will hold the 2D image. the Writeablebitmap needs to be contained within this.
        /// </summary>
        public Image MapImagePane { get; set; }
        /// <summary>
        /// This will be the 2d image. Writeable bitmap allows for editing of small regions - something that will need to happen often.
        /// </summary>
        public WriteableBitmap MapImage { get; set; }

        #endregion Properties

        #region Dependency Properties
        /// <summary>
        /// This is the map itself - user must allocate one to their desired size and pass it in.
        /// When this value is updated, the map will re-draw.
        /// </summary>
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


        #endregion Dependency Properties

        #region Public Functions

        /// <summary>
        /// Code to draw and place the map.
        /// Will first define basic info for drawing
        /// Then creates a byte array to define colors at each point in bitmap
        /// Then the bitmap will be placed on screen.
        /// </summary>
        public void DrawMap()
        {

            //First define parameters.
            ParentContainer.Width = Map.Width;
            ParentContainer.Height = Map.Height;

            PixelFormat pf = PixelFormats.Bgr32;

            int rawStride = (Map.Width * pf.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * Map.Height];


            //Next use those parameters to populate the array and define each pixel's colour.
            for (int i = 0; i < Map.Height; i++)
            {
                for (int j = 0; j < rawStride; j += 4)
                {
                    int index = (i * rawStride) + j;
                    int actualJ = j / 4;

                    if (Map[i][actualJ].Explored)
                    {

                        int BiomeIndex = (int)(10 * Map[i][actualJ].Elevation);


                        for (int k = 0; k < 4; k++)
                        {
                            rawImage[index + k] = ColorScale.BiomeColors[BiomeIndex, k];
                        }
                    }

                    else
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            rawImage[index + k] = ColorScale.BiomeColors[11, k];
                        }
                    }
                }
            }

            //Finally generate and apply bitmap image.
            BitmapSource baseMap;

            MapImagePane.Width = Map.Width;
            MapImagePane.Height = Map.Height;
            
            baseMap = BitmapSource.Create(Map.Width, Map.Height, 96, 96, pf, null, rawImage, rawStride);

            WriteableBitmap bitmap = new WriteableBitmap(baseMap);

            MapImagePane.Source = bitmap;


            /*
             * PREVIOUS CODE FOR HOW TO EDIT THE WRITABLE BITMAP - KEPT FOR REFERENCE
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
