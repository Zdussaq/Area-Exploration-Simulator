using System;

public class Drone
{

	//set all coordinates as float?
	//NEED TO GET WINDOW SIZE SOMEHOW
	private int battery { get; set; }
	private float X { get; set; }
	private float Y { get; set; }
	private float startX { get; set; }
	private float startY { get; set; }
	private float speed { get; set; }
	private float windowSize { get; set; }
	private System.Timers.Timer batteryCountdown;


	/// <summary>
    /// Calculate the distance from the starting position to the current position, not square rooted
	/// </summary>
	void distFromStart() 
	{
		int distance = 0;
		distance = ((startX - X) * (startX - X)) + ((startY - Y) * (startY - Y));
	}

	/// <summary>
    /// Immediately return from current point to starting point
	/// </summary>
	void returnToBase()//starting coordinates 
	{
		while (X != startX && Y != startY) {
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
	void update(object sender, ElapsedEventArgs e) 
	{
		battery--;
		moveDrone();

	}


	/// <summary>
    /// If needed to pause the program, then the battery will also pause
	/// </summary>
	void pauseBattery(bool pause) 
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
	bool sufficentBattery() 
	{
		if (battery > (distance / speed)+1)
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
	void moveDrone(int untilX, int untilY)
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

		batteryCountdown = new System.Timers.Timer(battery);
		batteryCountdown.Tick += update;
		batteryCountdown.Start();

		while (sufficentBattery()){ }
		returnToBase();
		//call destructor

	}
}
