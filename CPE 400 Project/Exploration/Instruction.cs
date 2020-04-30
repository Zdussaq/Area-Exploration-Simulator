using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE400Project.Exploration
{
    /// <summary>
    /// Measures a direction and a number of units to move - basic building block for movement.
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// Defines actual drection to move, i.e. N, NE, E, etc.
        /// </summary>
        public Directions Direction { get; set; }
        /// <summary>
        /// Defines how many units to move in this direction
        /// </summary>
        public int NumUnits { get; set; }

        #region Constructors
        public Instruction(int numUnits, Directions direction)
        {
            Direction = direction;
            NumUnits = numUnits;
        }

        public Instruction()
        {
            NumUnits = -1;
        }
        #endregion
    }
}
