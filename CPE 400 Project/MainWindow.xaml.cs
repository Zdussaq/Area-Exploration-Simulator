using CPE400Project.EnvironmentData;
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

            //Initialize a map to test.
            Map = new Map(1000, 1600);
            MapGrid.Map = Map;
            DataContext = this;

            for (i = 0; i < 100; i += 4)
            {
                for (j = 0; j < 100; j += 4)
                {
                    MapGrid.MarkRegionExplored(i, j);
                }
            }

        }

        #endregion Constructors

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        public Map Map { get; set; }


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

        #endregion Properties

        #region PrivateFunctions


        #endregion PrivateFunctions

        #region PublicFunctions


        #endregion Public FUnctions
        public int i = 0;
        public int j = 0;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
