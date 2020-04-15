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

        /// <summary>
        /// This is the radius a drone can see in pixels. Default will 10 Pixels
        /// </summary>
        public int DroneVision { get { return 10; } set { this.DroneVision = value; } }

        public int RawStride { get; set; }
        public byte[] RawImage { get; set; }

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

        public void MarkRegionExplored(int x, int y)
        {
            //First mark a sphere as explored
            for (int i = 0; i < DroneVision; i++)
            {
                for (double j = 0; j < 360; j += 0.1)
                {
                    int xMod = (int)(Math.Cos(j * Math.PI / 180) * i);
                    int yMod = (int)(Math.Sin(j * Math.PI / 180) * i);


                    if (xMod + x > 0 || xMod + x <= Map.Width
                        || yMod + y > 0 || yMod + y <= Map.Height )
                    {
                        Map[xMod + x][yMod + y].Explored = true;
                    }
                }
            
            }

            DrawMap();

            ////Next we need to update the mapitself
            //int drawArea = (int)Math.Pow((DroneVision * 2 + 1), 2);
            //byte[] editArea = new byte[RawStride * drawArea];

            //int imageCenter = (y * RawStride * x) + x;
            //int start = imageCenter - (((drawArea * RawStride) / 2) + 1);
                                          
            //for ( int i = 0; i < editArea.Length; i+= 4)
            //{
            //    editArea[i] = RawImage[i + 0]; //B
            //    editArea[i + 1] = RawImage[i + 1]; //G
            //    editArea[i + 2] = RawImage[i + 2]; //R
            //    editArea[i + 3] = RawImage[i + 3];
            //}

            //MapImage.WritePixels(
            //        new Int32Rect(x, y, DroneVision * 2 + 1, DroneVision * 2 + 1),
            //        editArea,
            //        RawStride,
            //        0
            //        );

            //MapImagePane.Source = MapImage;

            //for (int j = 0; j < 100; j++)
            //{
            //    for (int i = 0; i < editArea.Length; i += 4)
            //    {
            //        editArea[i] = 255; //B
            //        editArea[i + 1] = 0; //G
            //        editArea[i + 2] = 255; //R
            //        editArea[i + 3] = 0;
            //    }

                
            //}


        }

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

            RawStride = (Map.Width * pf.BitsPerPixel + 7) / 8;
            RawImage = new byte[RawStride * Map.Height];


            //Next use those parameters to populate the array and define each pixel's colour.
            for (int i = 0; i < Map.Height; i++)
            {
                for (int j = 0; j < RawStride; j += 4)
                {
                    int index = (i * RawStride) + j;
                    int actualJ = j / 4;

                    if (Map[i][actualJ].Explored)
                    {

                        int BiomeIndex = (int)(10 * Map[i][actualJ].Elevation);


                        for (int k = 0; k < 4; k++)
                        {
                            RawImage[index + k] = ColorScale.BiomeColors[BiomeIndex, k];
                        }
                    }

                    else
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            RawImage[index + k] = ColorScale.BiomeColors[11, k];
                        }
                    }
                }
            }

            //Finally generate and apply bitmap image.
            BitmapSource baseMap;

            MapImagePane.Width = Map.Width;
            MapImagePane.Height = Map.Height;
            
            baseMap = BitmapSource.Create(Map.Width, Map.Height, 96, 96, pf, null, RawImage, RawStride);

            WriteableBitmap bitmap = new WriteableBitmap(baseMap);

            MapImagePane.Source = bitmap;

        }
            

        

        #endregion Public Functions

    }
}
