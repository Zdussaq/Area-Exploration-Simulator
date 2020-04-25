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

        public ClassController(MapElement currentMap, IList<Drone> drones)
        {
            XCoords = new List<float>();
            YCoords = new List<float>();
            droneList = drones;
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
            bool executing = true;
            while (executing)
            {
                foreach (var i in droneList)
                {
                    executing = false;
                    if (i.update())
                    {
                        executing = true;
                    }
                }
                updateMap();
            }
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
            
        }

        //Function that will update all drone's battery levels
        public void updateDroneBatteries()
        {
            for(int i = 0; i < droneList.Count; i++)
            {
                droneList[i].battery = (int)currentDroneBatteries[i];
            }
        }

        //Function to calculate algorithm for where the drones should travel
        public void determineFlight(/*access to Drone class here*/)
        {
            //Define number drones
            //Create n number of regions
            //define relative 0 per drone
            //create instruction set

            //bool finished = false;

            int numDrones = droneList.Count;
            int sectionWidth = map.Map.Width / numDrones;

            for(int i = 0; i < droneList.Count; i++)
            {
                int startX = sectionWidth * i;
                int startY = 3;

                int currentX = droneList[i].X;
                int currentY = droneList[i].Y;

                bool finished = false;
                while (!finished)
                {
                    int battery = droneList[i].battery;

                    IList<Instruction> list = mapTo(currentX, currentY, startX, startY);
                    battery -= calculateBatteryUsage(list);

                    foreach (var k in list)
                    {
                        droneList[i].Instructions.Add(k);       //might use insert
                    }

                    currentX = startX;
                    currentY = startY;

                    while ((calculateDistanceToHome(currentX, currentY) + 10 + (3 * sectionWidth)) < battery)
                    {
                        if (currentY < map.Map.Height)
                        {
                            while ((calculateDistanceToHome(currentX, currentY) + 10 + (3 * sectionWidth)) < battery)
                            {
                                droneList[i].Instructions.Add(new Instruction(sectionWidth, Directions.E));
                                currentX += sectionWidth;
                                battery -= sectionWidth;
                                if (currentX > 160)
                                {

                                }
                                droneList[i].Instructions.Add(new Instruction(5, Directions.N));
                                currentY += 5;
                                battery -= 5;
                                if (currentX > 160)
                                {

                                }
                                droneList[i].Instructions.Add(new Instruction(sectionWidth, Directions.W));
                                currentX -= sectionWidth;
                                battery -= sectionWidth;
                            }
                        
                            
                            var homeRoute = mapTo(currentX, currentY, map.Map.HomeBase.XCenter, map.Map.HomeBase.YCenter);
                            foreach ( var j in homeRoute )
                            {
                                droneList[i].Instructions.Add(j);
                            }

                            startX = currentX;
                            startY = currentY;

                            currentX = map.Map.HomeBase.XCenter;
                            currentY = map.Map.HomeBase.YCenter;

                        }
                        else
                        {
                            finished = true;
                            battery = 0;
                        }
                    }

                    
                }
            }

        }

        public int calculateDistanceToHome(int startX, int startY)
        {
            return calculateBatteryUsage(mapTo(startX, startY, map.Map.HomeBase.XCenter, map.Map.HomeBase.YCenter));
        }

        public int calculateBatteryUsage(IList<Instruction> instructions)
        {
            int total = 0;
            foreach (var i in  instructions)
            {
                total += i.NumUnits;
            }

            return total;
        }

        public IList<Instruction> mapTo(int srcX, int srcY, int targetX, int targetY)
        {

            IList<Instruction> instructions = new List<Instruction>();

            int currentX = srcX;
            int currentY = srcY;

            Instruction instruction = new Instruction();
            while (currentX != targetX && currentY != targetY)
            {
                Directions direction;
                if (currentX == targetX && currentY < targetY)
                {
                    direction = Directions.N;
                    currentY++;
                }
                else if (currentX < targetX && currentY < targetY)
                {
                    direction = Directions.NE;
                    currentX++;
                    currentY++;
                }
                else if (currentX < targetX && currentY == targetY)
                {
                    direction = Directions.E;
                    currentX++;
                }
                else if (currentX < targetX && currentY > targetY)
                {
                    direction = Directions.SE;
                    currentX++;
                    currentY--;
                }
                else if (currentX == targetX && currentY > targetY)
                {
                    direction = Directions.S;
                    currentY--;
                }
                else if (currentX > targetX && currentY > targetY)
                {
                    direction = Directions.SW;
                    currentY--;
                    currentX--;
                }
                else if (currentX > targetX && currentY == targetY)
                {
                    direction = Directions.W;
                    currentX--;
                }
                else
                {
                    direction = Directions.NW;
                    currentX--;
                    currentY++;
                }

                if (instruction.Direction != direction)
                {
                    if (instruction.Direction != null)
                    {
                        instructions.Add(instruction);
                    }
                    instruction = new Instruction(1, direction);
                }
                else
                {
                    instruction.NumUnits++;
                }


            }

            return instructions;

        }

        //Function that will call Dean's Map.updateView() function
        public void updateMap()
        {
            map.UpdateMap(droneList);
        }
    }
}
