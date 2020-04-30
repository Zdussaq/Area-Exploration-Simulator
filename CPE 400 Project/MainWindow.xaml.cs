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
using System.Threading;
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
using System.Windows.Threading;

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
            DroneVision = 15;
            
            NumDrones = 20;
            MapWidth = 800;
            MapHeight = 600;
            OptionsVis = Visibility.Visible;
            MapVis = Visibility.Collapsed;
            LoadingVis = Visibility.Collapsed;
            restartBtn.Visibility = Visibility.Collapsed;
            BatterySlider.Minimum = 3 *  Math.Sqrt(Math.Pow(MapWidth, 2) + Math.Pow(MapHeight, 2));
            DroneBattery = 2 * (int)BatterySlider.Minimum;
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

                BatterySlider.Minimum = (int)(3 * Math.Sqrt(Math.Pow(MapWidth, 2) + Math.Pow(MapHeight, 2))) + 1;
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

                BatterySlider.Minimum = (int)(3 * Math.Sqrt(Math.Pow(MapWidth, 2) + Math.Pow(MapHeight, 2))) + 1;
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
        //manages when interface is interacted w/
        public event PropertyChangedEventHandler PropertyChanged;

        //Data backing the map. For the actual code on the displayed map in runtime see the class: "MapElement"
        public Map Map { get; set; }

        //This will manage all data changes
        public ClassController Controller { get; set; }



        #endregion Properties

        #region PrivateFunctions

        /// <summary>
        /// Manages interface changes
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Manages interface changes
        /// </summary>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Starts the simulation itself- will call the controller and have it run through all fucnitons
        /// </summary>
        private void BeginSimulation(object sender, RoutedEventArgs e)
        {
            //modifies 
            OptionsVis = Visibility.Collapsed;
            LoadingVis = Visibility.Visible;
            restartBtn.Visibility = Visibility.Collapsed;

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
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
                    Controller.DetermineFlight();



                    LoadingVis = Visibility.Collapsed;
                    MapVis = Visibility.Visible;

                    int step = DroneVision - 4; //Offset by size of the drone itself
                    int numInstructions = (Controller.MaxInstructionsRemaining / step) + 1;
                    for (int j = 0; j < numInstructions; j++)
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            DispatcherPriority.Background,
                            new Action(() =>
                            {
                                for (int _ = 0; _ < step; _++)
                                {
                                    Controller.ControllerUpdate();
                                }
                                MapGrid.UpdateMap(Controller.Drones);
                            }));

                    }

                    MapGrid.UpdateMap(Controller.Drones);
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.ContextIdle,
                    new Action(() =>
                    {
                        restartBtn.Visibility = Visibility.Visible;
                    }));

                }));


        }

        /// <summary>
        /// Restart button in the end of the mapping process.
        /// Will reset the program to the state of when it first started.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Map = null;
            Controller = null;
            LoadingVis = Visibility.Collapsed;
            MapVis = Visibility.Collapsed;
            OptionsVis = Visibility.Visible;
        }

        #endregion PrivateFunctions

        #region PublicFunctions


        #endregion Public FUnctions



    }
}
