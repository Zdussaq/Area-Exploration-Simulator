using CPE400Project.MapDisplay;
using System.Collections.Generic;
using CPE400Project.Exploration;
using System.Diagnostics;

namespace CPE400Project.Controller
{
    /// <summary>
    /// Class controller will create drone paths, store their info, and update the drone positions.
    /// In a model-view-contoroller design pattern, this is the controller.
    /// </summary>
    public class ClassController
    {

        #region Constructors

        /// <summary>
        /// Main constructor for the controller. Drones and map MUST exist on initialization
        /// </summary>
        /// <param name="currentMap">Map generated in the main function. This is pass-by-reference, so the controller will be editing the one seen in the display.</param>
        /// <param name="drones">Set of drones. They are initially not loaded with any instructions, however, have certian variables like battery and range assigned.</param>
        public ClassController(MapElement currentMap, IList<Drone> drones)
        {
            Drones = drones;
            Map = currentMap;
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// This is the property for how many instructions remain in the drones memory.
        /// This is  read-only
        /// </summary>
        public int MaxInstructionsRemaining
        {
            get
            {
                int ret = 0;
                foreach (var i in Drones)
                {
                    int moves = CalculateBatteryUsage(i.Instructions);
                    if (moves > ret)
                    {
                        ret = moves;
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// This is the primary list of all drones - it is loaded with instructions
        /// </summary>
        public IList<Drone> Drones { get; set; }

        /// <summary>
        /// This is the display seen in the main window. It will be updated whenever the drones positions change.
        /// </summary>
        MapElement Map { get; set; }

        //Constant running functions
        #endregion Properties

        #region Public Functions
        /// <summary>
        /// Controller update will go through and update all drones in the system
        /// </summary>
        public void ControllerUpdate()
        {
            foreach (var i in Drones)
            {
                i.Update();
            }
                
        }

        /// <summary>
        /// This will pre-determin all drones flight path.
        /// Will map each region, breaking them up into sections based off width.
        /// Each drone will then explore its own sections.
        /// While this is not the most effecient way, it is very very close. 
        /// There are some fringe cases in which it would be slightly more time efficient to have a
        /// drone explore into another region.
        /// However, this effect is negligable.
        /// </summary>
        public void DetermineFlight()
        {
            //Coordinates of recharge point.
            //Store reference to them to minimze access time.
            int baseX = Map.Map.HomeBase.XCenter;
            int baseY = Map.Map.HomeBase.YCenter;

            //Width of each region the drones will explore. 
            int regionSize = (int)((Map.Map.Width / Drones.Count) + 1);

            //Loop to define position for each drone, one at a time.
            for (int i = 0; i < Drones.Count; i++)
            {
                //Define relative positions. Drones position is not changed here, we are just creating the instruction set THEN moving the drone
                int currentX = Drones[i].X;
                int currentY = Drones[i].Y;
                int battery = Drones[i].Battery;

                //This is the target path - this will change as time goes on in the loop to where the last mapped area is.
                int destX = i * regionSize;
                int destY = 5;

                //How far vertically the drone will move. Scales with vision to speed up exploration
                int verticalStep = Map.DroneVision;

                //How far east and west the drone will move, the conditional ensure the drone won't clip out of the map.
                int moveDistance = (destX + regionSize < Map.Map.Width) ? regionSize : (Map.Map.Width - destX - 1);

                bool explored = false;
                bool firstRun = true;
                bool east = false;
                while (!explored)
                {
                    //Moves drone to dest location
                    var list = MapTo(currentX, currentY, destX, destY);
                    foreach (var j in list)
                    {
                        //push instructions to drone list
                        Drones[i].Instructions.Add(j);
                    }

                    //update coordinates & battery
                    currentX = destX;
                    currentY = destY;
                    battery -= CalculateBatteryUsage(list);


                    //This elimnates one unnesesary movement
                    if (firstRun)
                    {
                        Drones[i].Instructions.Add(new Instruction(moveDistance, Directions.E));
                        battery -= moveDistance;
                        currentX += moveDistance;
                        firstRun = false;
                    }

                    //Moves the drone down until its battery gets too low for another movement + recharge
                    //Other exit condition is for when the region is fully explored
                    while (battery > CalculateDistanceToHome(currentX, currentY) + (2 * moveDistance) + (2 * verticalStep) && currentY + verticalStep < Map.Map.Height)
                    {
                        Directions direction = (east) ? Directions.E : Directions.W;
                        
                        Drones[i].Instructions.Add(new Instruction(verticalStep, Directions.N));
                        Drones[i].Instructions.Add(new Instruction(moveDistance, direction));
                        

                        if (east)
                        {
                            currentX += moveDistance;
                        }
                        else
                        {
                            currentX -= moveDistance;
                        }

                        east = !east;

                        battery -= verticalStep + moveDistance;
                        currentY += verticalStep;
                    }

                    destX = currentX;
                    destY = currentY;
                    //If on the edge of the map, route it back
                    if (currentY + verticalStep >= Map.Map.Height)
                    {
                        explored = true;
                        list = MapTo(currentX, currentY, baseX, baseY);
                        foreach (var j in list)
                        {
                            Drones[i].Instructions.Add(j);
                        }
                        currentX = baseX;
                        currentY = baseY;
                        battery = Drones[i].Battery;
                    }
                    //When battery needs to be recharged to continue
                    if (battery <= CalculateDistanceToHome(currentX, currentY) + (2 * moveDistance) + (2 * verticalStep) && !explored)
                    {
                        list = MapTo(currentX, currentY, baseX, baseY);
                        foreach (var j in list)
                        {
                            Drones[i].Instructions.Add(j);
                        }
                        currentX = baseX;
                        currentY = baseY;
                        battery = Drones[i].Battery;
                    }


                    
                }
            }

        }

        /// <summary>
        /// Gets the range from a point to the home base recharge point
        /// </summary>
        /// <param name="startX">Initial X position</param>
        /// <param name="startY">Initial Y Position</param>
        /// <returns>Amount of units to move to get back home.</returns>
        public int CalculateDistanceToHome(int startX, int startY)
        {
            return CalculateBatteryUsage(MapTo(startX, startY, Map.Map.HomeBase.XCenter, Map.Map.HomeBase.YCenter));
        }

        /// <summary>
        /// Calcualtes the battery used to in a set of instructions
        /// </summary>
        /// <param name="instructions">List of instructions to calculate</param>
        /// <returns>Number of units in battery cost for instruction set</returns>
        public int CalculateBatteryUsage(IList<Instruction> instructions)
        {
            int total = 0;
            foreach (var i in  instructions)
            {
                total += i.NumUnits;
            }

            return total;
        }

        /// <summary>
        /// Creates a route from point a to point b
        /// Will move diagnoally toward target, then straight at it.
        /// Is about 10% more moves than an estimated line.
        /// </summary>
        /// <param name="srcX">Start X pos</param>
        /// <param name="srcY">Start Y pos</param>
        /// <param name="targetX">Target X position</param>
        /// <param name="targetY">Target X position</param>
        /// <returns></returns>
        public IList<Instruction> MapTo(int srcX, int srcY, int targetX, int targetY)
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


        #endregion Public Functions
    }
}
