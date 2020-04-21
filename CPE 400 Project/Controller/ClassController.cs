using CPE400Project.MapDisplay;
using System.Collections.Generic;
using CPE400Project.Exploration;

namespace CPE400Project.Controller
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
        public IList<float> currentDroneBatteries { get; set; }

        public IList<Drone> droneList { get; set; }

        MapElement map { get; set; }

        //Constant running functions

        public ClassController()
        {
            currentXCoords = new List<float>();
            currentYCoords = new List<float>();
            droneList = new List<Drone>();
            map = new MapElement();
        }
        
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
            updateDroneCoords();
            updateDroneBatteries();
        }

        //Function that will update both X and Y coordinates of each drone
        public void updateDroneCoords()
        {
            //droneList = new IList<Drone>();
            for(int i = 0; i < droneList.Count; i++)
            {
                droneList[i].X = currentXCoords[i];
                droneList[i].Y = currentYCoords[i];
            }
        }

        //Function that will update all drone's battery levels
        public void updateDroneBatteries()
        {
            for(int i = 0; i < droneList.Count; i++)
            {
                //droneList[i].battery = currentDroneBatteries[i];
            }
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
