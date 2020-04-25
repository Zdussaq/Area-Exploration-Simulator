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
        public IList<float> XCoords { get; set; }
        //IList that will store Y coordinates of drones
        public IList<float> YCoords { get; set; }
        //IList that will store all drone's battery levels
        public IList<float> currentDroneBatteries { get; set; }

        public IList<Drone> droneList { get; set; }

        MapElement map { get; set; }

        //Constant running functions

        public ClassController(MapElement currentMap)
        {
            XCoords = new List<float>();
            YCoords = new List<float>();
            droneList = new List<Drone>();
            map = currentMap;
        }

        //Function that will run multiple times to sync the dronesList to the current drone variables
        public void syncDrones(Drone drone, int numDrones, int index)
        {
            if(droneList.Count < numDrones)
            {
                droneList.Add(drone);
            }
            else
            {
                droneList[index] = drone;
            }
        }
        
        //GENERAL UPDATE FUNCTION OF CONTROLLER
        //Function will update all drone properties as well as map properties
        public void controllerUpdate()
        {
            determineFlight();
            updateDrone();
            updateMap();
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
            for(int i = 0; i < droneList.Count; i++)
            {
                droneList[i].X = XCoords[i];
                droneList[i].Y = YCoords[i];
            }
        }

        //Function that will update all drone's battery levels
        public void updateDroneBatteries()
        {
            for(int i = 0; i < droneList.Count; i++)
            {
                droneList[i].battery = currentDroneBatteries[i];
            }
        }

        //Function to calculate algorithm for where the drones should travel
        public void determineFlight(/*access to Drone class here*/)
        {
            //Reveal all parts of map within 20 points of the drone's path?
            //will put new coordinates into XCoords, YCoords
        }

        //Function that will call Dean's Map.updateView() function
        public void updateMap()
        {
            map.UpdateMap(droneList);
        }
    }
}
