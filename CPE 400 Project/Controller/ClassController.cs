using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE_400_Project.EnvironmentData;
using CPE400Project.MapDisplay;

namespace CPE_400_Project.Controller
{
    /// <summary>
    /// Class controller that will determine where the drones will travel,
    /// as well as what information will be displayed on the GUI
    /// </summary>
    public class ClassController
    {
        //IList that will store X coordinates of drones
        public IList<float> currentXCoords { get; set; }
        //IList that will store Y coordinates of drones
        public IList<float> currentYCoords { get; set; }
        //IList that will store all drone's battery levels
        public IList<float> droneBatteries { get; set; }

        //Constant running functions
        
        //GENERAL UPDATE FUNCTION OF CONTROLLER
        //Function will update all drone properties as well as map properties
        public void controllerUpdate()
        {
            updateDrone();
            updateMap();
            determineFlight();
        }

        //Function that will update the controller with the current coordinates of each drone
        //and the drones battery life
        public void updateDrone()
        {
            //updateDroneCoords(/*will take in drone coordinate IList (both x IList and y IList)*/);
        }

        //Function that will update both X and Y coordinates of each drone
        public void updateDroneCoords(IList<float> xCoords, IList<float> yCoords)
        {
            currentXCoords = xCoords;
            currentYCoords = yCoords;
        }

        //Function that will update all drone's battery levels
        public void updateDroneBatteries(IList<float> currentDroneBatteries)
        {
            droneBatteries = currentDroneBatteries;
        }

        //Function to calculate algorithm for where the drones should travel
        public void determineFlight(/*access to Drone class here*/)
        {
            //Reveal all parts of map within 20 points of the drone's path?
        }

        //Function that will call Dean's Map.updateView() function
        public void updateMap()
        {
            
        }
    }
}
