﻿using System;
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
		public int Battery { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int HomeX { get; set; }
		public int HomeY { get; set; }
		public int MaxBattery { get; set; }
		public IList<Instruction> Instructions { get; set; }


		/// <summary>
		/// Updates drone - will move its position by one and reduce battery by one
		/// </summary>
		/// <returns>if the update was a success. Will fail if no instructions are avaliable to run.</returns>
		public bool Update()
		{
			
			if (X == HomeX && Y == HomeY)
			{
				Battery = MaxBattery;
			}
			if (Battery > 0)
			{
				Battery--;
				return ExecuteInstruction();
				
			}
			return false;
		}

		/// <summary>
		/// Will perform a movement action for the drone
		/// </summary>
		/// <returns>if the movement succeeded. Will return false if no instructions exist</returns>
		public bool ExecuteInstruction(){
			

			if(Instructions.Count != 0)
			{

				
				
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
					default:
						break;
				
				}
				Instructions[0].NumUnits--;

				if (Instructions[0].NumUnits <= 0)
				{
					Instructions.RemoveAt(0);
				}

				return true;

				

			}
			return false;


		}

		/// <summary>
		/// Constructor of the drone class
		/// </summary>
		public Drone(int x, int y, int batteryLife, int homeX, int homeY)
		{
			Battery = batteryLife;
			MaxBattery = batteryLife;
			X = x;
			Y = y;
			HomeX = homeX;
			HomeY = homeY;
			Instructions = new List<Instruction>();

		}
	}
}