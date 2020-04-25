using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE400Project.Exploration;

namespace CPE400Project.Exploration
{
	public class Drone
	{

		//set all coordinates as float?
		//NEED TO GET WINDOW SIZE SOMEHOW
		public int battery { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public float startX { get; set; }
		public float startY { get; set; }
		public IList<Instruction> Instructions { get; set; }


		/// <summary>
		/// Reduce the battery amount every second by 1, starts at 100
		/// </summary>
		public void update()
		{
			battery--;
			if (battery > 0)
			{
				executeInstruction();
			}
		}


		void executeInstruction(){
			
			if(Instructions.Count > 0)
			{

				if (Instructions[0].NumUnits == 0)
				{
					Instructions.RemoveAt(0);
				}
				
				switch(Instructions[0].Direction)
				{
					case Directions.N:
						Y += 1;
						break;
					case Directions.NE:
						Y += 1;
						X += 1;
						break;
					case Directions.E:
						X += 1;
						break;
					case Directions.SE:
						Y -= 1;
						X += 1;
						break;
					case Directions.S:
						Y -= 1;
						break;
					case Directions.SW:
						Y -= 1;
						X -= 1;
						break;
					case Directions.W:
						X -= 1;
						break;
					case Directions.NW:
						Y += 1;
						X -= 1;
						break;
				
				}

				

			}


		}

		/// <summary>
		/// Constructor of the drone class
		/// </summary>
		public Drone(int x, int y, int batteryLife)
		{
			battery = batteryLife;
			X = x;
			Y = y;
			Instructions = new List<Instruction>();

		}
	}
}