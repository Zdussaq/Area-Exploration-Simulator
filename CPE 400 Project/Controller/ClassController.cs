using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE_400_Project.EnvironmentData;

namespace CPE_400_Project.Controller
{
    /// <summary>
    /// Class controller that will determine where the drones will travel,
    /// as well as what information will be displayed on the GUI
    /// </summary>
    public class ClassController
    {
        //IList could potentially store all current locations of the drones: (xCoord, yCoord) per drone
        public IList<float> currentLocation { get; set; }

        //Constant running functions
        public void callController()
        {
            //class ideas: should access data available in MapDisplay?
                        // should interact with future Drone class (gather & update coordinates frequently)
                        // should constantly update parts of map to go from black to colored when discovered
        }

        //Functions to calculate algorithm for where the drones should travel
        public void determineFlight(/*access to Drone class here*/)
        {
            //Reveal all parts of map within 20 points of the drone's path?
        }

        //Function that will update the current coordinates of each drone with the controller
        public void updateFlightCoordinates(IList<float> currentCoords)
        {
            currentCoords = currentLocation;
        }
    }
}
