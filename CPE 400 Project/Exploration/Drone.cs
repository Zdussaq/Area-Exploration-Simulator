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
		public float speed { get; set; }
		public Timer batteryCountdown { get; set; }
		public IList<Instruction> Instructions { get; set; }


		/// <summary>
		/// Reduce the battery amount every second by 1, starts at 100
		/// </summary>
		public void update(object sender, ElapsedEventArgs e)
		{
			battery--;
			executeInstruction();
		}


		/// <summary>
		/// If needed to pause the program, then the battery will also pause
		/// </summary>
		public void pauseBattery(bool pause)
		{
			if (batteryCountdown != null && pause)
			{
				batteryCountdown.Stop();
			}
			else
			{
				batteryCountdown.Start();
			}

		}


		void executeInstruction(){

			if(Instructions != null)
			{
				units = Instructions[0].NumUnits;
				
				switch(Instructions[0].Direction)
				{
					case 1:
						Y+ units;
						break;
					case 2:
						Y+ units;
						X+ units;
						break;
					case 3:
						X+ units;
						break;
					case 4:
						Y- units;
						X+ units;
						break;
					case 5:
						Y- units;
					case 6:
						Y- units;
						X- units;
						break;
					case 7:
						X- units;
						break;
					case 8:
						Y+ units;
						X- units;
						break;
				
				}
			
				Instructions.RemoveAt(0);

			}
			else
			{
				//no more instructions
				//time to delete object
				//return value and then how to remove object?
			
			
			}
				
		
		}

		/// <summary>
		/// Constructor of the drone class
		/// </summary>
		public Drone(int X, int Y, int batteryLife)
		{
			battery = batteryLife;

			batteryCountdown = new Timer(battery);
			Instructions = new List<Instruction>();
			//batteryCountdown.Tick += update;
			batteryCountdown.Start();

		}
	}
}