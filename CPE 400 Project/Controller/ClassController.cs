using CPE400Project.MapDisplay;
using System.Collections.Generic;
using CPE400Project.Exploration;
using System.Diagnostics;

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

        
        //GENERAL UPDATE FUNCTION OF CONTROLLER
        //Function will update all drone properties as well as map properties
        public void controllerUpdate()
        {
            bool executing = true;
            for (int _ = 0; _ < 10; _++) { 
                executing = false;
                for (int i = 0; i < this.droneList.Count; i++)
                {
                    bool status = droneList[i].update();
                    if (status)
                    {
                        executing = true;
                    }
                }
                //updateMap();
            }
        }

        //Function to calculate algorithm for where the drones should travel
        public void determineFlight()
        {

            int baseX = map.Map.HomeBase.XCenter;
            int baseY = map.Map.HomeBase.YCenter;

            int regionSize = map.Map.Width / droneList.Count;

            for (int i = 0; i < droneList.Count; i++)
            {
                int currentX = droneList[i].X;
                int currentY = droneList[i].Y;
                int battery = droneList[i].battery;
                int destX = i * regionSize;
                int destY = 5;

                bool explored = false;

                while (!explored)
                {
                    var list = mapTo(currentX, currentY, destX, destY);
                    foreach (var j in list)
                    {
                        droneList[i].Instructions.Add(j);
                    }

                    currentX = destX;
                    currentY = destY;
                    battery -= calculateBatteryUsage(list);

                    

                    while (battery > calculateDistanceToHome(currentX, currentY) + ( 3 * regionSize) + 10 && currentY + 14 < map.Map.Height)
                    {
                        droneList[i].Instructions.Add(new Instruction(regionSize, Directions.E));
                        droneList[i].Instructions.Add(new Instruction(7, Directions.N));
                        droneList[i].Instructions.Add(new Instruction(regionSize, Directions.W));
                        droneList[i].Instructions.Add(new Instruction(7, Directions.N));
                        battery -= 14 + (2 * regionSize);
                        currentY += 14;
                    }


                    if (!(currentY + 14 < map.Map.Height))
                    {
                        explored = true;
                        list = mapTo(currentX, currentY, baseX, baseY);
                        foreach (var j in list)
                        {
                            droneList[i].Instructions.Add(j);
                        }
                        currentX = baseX;
                        currentY = baseY;
                        battery = droneList[i].battery;

                    }


                    destX = currentX;
                    destY = currentY;
                    
                    if ((battery <= calculateDistanceToHome(currentX, currentY) + (3 * regionSize) + 10) && !explored) {
                        list = mapTo(currentX, currentY, baseX, baseY);
                        foreach (var j in list)
                        {
                            droneList[i].Instructions.Add(j);
                        }
                        currentX = baseX;
                        currentY = baseY;
                        battery = droneList[i].battery;
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
            while (currentX != targetX || currentY != targetY)
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
                    if (instruction.Direction != 0)
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

            instructions.Add(instruction);
            return instructions;

        }

        //Function that will call Dean's Map.updateView() function
        public void updateMap()
        {
            map.UpdateMap(droneList);
        }
    }
}
