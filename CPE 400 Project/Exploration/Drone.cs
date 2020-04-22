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
		public float windowSize { get; set; }
		public Timer batteryCountdown { get; set; }

		public IList<Instruction> Instructions { get; set; }


		/// <summary>
		/// Calculate the distance from the starting position to the current position, not square rooted
		/// </summary>
		public float distFromStart()
		{
			float distance = 0;
			distance = ((startX - X) * (startX - X)) + ((startY - Y) * (startY - Y));
			return distance;
		}

		/// <summary>
		/// Immediately return from current point to starting point
		/// </summary>
		public void returnToBase()//starting coordinates 
		{
			while (X != startX && Y != startY)
			{
				if (X > startX)
				{ X--; }
				else
				{ X++; }

				if (Y > startY)
				{ Y--; }
				else
				{ Y++; }
			}

		}

		/// <summary>
		/// Reduce the battery amount every second by 1, starts at 100
		/// </summary>
		public void update(object sender, ElapsedEventArgs e)
		{
			battery--;
			moveDrone(0, 0);
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

		/// <summary>
		/// Compares time remaining to distance / speed to know if it can return to base
		/// </summary>
		public bool sufficentBattery()
		{
			if (battery > (distFromStart() / speed) + 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Move drone in any direction until the desired x and y are reached
		/// </summary>
		public void moveDrone(int untilX, int untilY)
		{
			while (Y != untilY)
			{
				if (Y > untilY)
				{ Y--; }
				else
				{ Y++; }

			}

			while (X != untilX)
			{
				if (X > untilX)
				{ X--; }
				else
				{ X++; }
			}


		}

		/// <summary>
		/// Constructor of the drone class
		/// </summary>
		public Drone(int X, int Y, int droneSpeed)
		{
			battery = 100;
			speed = droneSpeed;

			batteryCountdown = new Timer(battery);
			Instructions = new List<Instruction>();
			//batteryCountdown.Tick += update;
			batteryCountdown.Start();

			while (sufficentBattery()) { }
			returnToBase();
			//call destructor

		}
	}
}