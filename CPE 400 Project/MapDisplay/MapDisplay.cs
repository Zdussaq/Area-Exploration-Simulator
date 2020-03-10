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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CPE_400_Project"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CPE_400_Project;assembly=CPE_400_Project"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class MapDisplay : UserControl
    {
        #region Constructors
        public MapDisplay()
        {

            Canvas = new Canvas();
            Content = Canvas;

            DefaultStyleKeyProperty.OverrideMetadata(typeof(MapDisplay), new FrameworkPropertyMetadata(typeof(MapDisplay)));

        }

        #endregion Constructors

        #region Properties

        public Canvas Canvas { get; set; }

        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(Map), typeof(MapDisplay));
        public Map Map
        {
            get { return (Map)GetValue(MapProperty); }
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
            Random random = new Random();

            for (int i = 0; i < Map.Width; i++)
            {
                for (int j = 0; j < Map.Height; i++)
                {

                }
            }
        }

        #endregion Public Functions

    }
}
