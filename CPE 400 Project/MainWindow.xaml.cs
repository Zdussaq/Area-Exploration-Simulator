using CPE400Project.Controller;
using CPE400Project.EnvironmentData;
using CPE400Project.Exploration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace CPE400Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ////Initialize a map to test.
            //Map = new Map(1000, 1600);
            //MapGrid.Map = Map;
            //DataContext = this;

            //for (i = 0; i < 100; i += 4)
            //{
            //    for (j = 0; j < 100; j += 4)
            //    {
            //        MapGrid.MarkRegionExplored(i, j);
            //    }
            //}
            
            DroneVision = 15;
            
            NumDrones = 20;
            MapWidth = Width;
            MapHeight = Height;
            OptionsVis = Visibility.Visible;
            MapVis = Visibility.Collapsed;
            LoadingVis = Visibility.Collapsed;
            BatterySlider.Minimum = 2*  Math.Sqrt(Math.Pow(MapWidth, 2) + Math.Pow(MapHeight, 2));
            DroneBattery = 2 * (int)BatterySlider.Minimum;




            Map = new Map((int)90, (int)160);
            MapGrid.DroneVision = DroneVision;
            MapGrid.Map = Map;

            IList<Drone> drones = new List<Drone>();
            int startX = MapGrid.Map.HomeBase.XCenter;
            int startY = MapGrid.Map.HomeBase.YCenter;
            for (int i = 0; i < NumDrones; i++)
            {
                drones.Add(new Drone(startX, startY, DroneBattery, startX, startY));
            }

            this.Controller = new ClassController(MapGrid, drones);


            LoadingVis = Visibility.Collapsed;
            MapVis = Visibility.Visible;
            UpdateLayout();


            Controller.controllerUpdate();


        }

        #endregion Constructors

        #region Binding Properties

        private int _numDrones;

        public int NumDrones
        {
            get { return _numDrones; }
            set
            {
                if (_numDrones == value)
                    return;

                _numDrones = value;
                OnPropertyChanged();
            }
        }

        private int _droneBattery;

        public int DroneBattery
        {
            get { return _droneBattery; }
            set
            {
                if (_droneBattery == value)
                    return;

                _droneBattery = value;
                OnPropertyChanged();
            }
        }

        private int _droneVision;

        public int DroneVision
        {
            get { return _droneVision; }
            set
            {
                if (_droneVision == value)
                    return;

                _droneVision = value;
                OnPropertyChanged();
            }
        }

        private double _mapWidth;

        public double MapWidth
        {
            get { return _mapWidth; }
            set
            {
                if (_mapWidth == value)
                    return;

                _mapWidth = value;

                BatterySlider.Minimum = (int)(2 * Math.Sqrt(Math.Pow(MapWidth, 2) + Math.Pow(MapHeight, 2))) + 1;
                BatterySlider.Maximum = (BatterySlider.Minimum > 20000) ? 3 * BatterySlider.Minimum : 20000;
                OnPropertyChanged();
            }
        }

        private double _mapHeight;

        public double MapHeight
        {
            get { return _mapHeight; }
            set
            {
                if (_mapHeight == value)
                    return;

                _mapHeight = value;

                BatterySlider.Minimum = 2 * Math.Sqrt(Math.Pow(MapWidth, 2) + Math.Pow(MapHeight, 2));
                BatterySlider.Maximum = (BatterySlider.Minimum > 20000) ? 3 * BatterySlider.Minimum : 20000;
                OnPropertyChanged();
            }
        }

        private Visibility _mapVis;
        public Visibility MapVis
        {
            get { return _mapVis; }
            set
            {
                if (_mapVis == value)
                    return;

                _mapVis = value;
                OnPropertyChanged();
            }
        }

        private Visibility _optionsVis;
        public Visibility OptionsVis
        {
            get { return _optionsVis; }
            set
            {
                if (_optionsVis == value)
                    return;

                _optionsVis = value;
                OnPropertyChanged();
            }
        }

        private Visibility _loadingVis;
        public Visibility LoadingVis
        {
            get { return _loadingVis; }
            set
            {
                if (_loadingVis == value)
                    return;

                _loadingVis = value;
                OnPropertyChanged();
            }
        }

        #endregion Binding Properties

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        public Map Map { get; set; }

        public ClassController Controller { get; set; }

        

        #endregion Properties

        #region PrivateFunctions


        #endregion PrivateFunctions

        #region PublicFunctions


        #endregion Public FUnctions

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void BeginSimulation(object sender, RoutedEventArgs e)
        {
            OptionsVis = Visibility.Collapsed;
            LoadingVis = Visibility.Visible;
            UpdateLayout();

            Map = new Map((int)MapHeight, (int)MapWidth);
            MapGrid.DroneVision = DroneVision;
            MapGrid.Map = Map;

            IList<Drone> drones = new List<Drone>();
            int startX = MapGrid.Map.HomeBase.XCenter;
            int startY = MapGrid.Map.HomeBase.YCenter;
            for (int i = 0; i < NumDrones; i++)
            {
                drones.Add(new Drone(startX, startY, DroneBattery, startX, startY));
            }

            this.Controller = new ClassController(MapGrid, drones);
            

            LoadingVis = Visibility.Collapsed;
            MapVis = Visibility.Visible;
            UpdateLayout();


            Controller.controllerUpdate();
        }

    }
}
