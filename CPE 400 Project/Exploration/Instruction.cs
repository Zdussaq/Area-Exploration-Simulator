using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE400Project.Exploration
{
    public class Instruction
    {
        Directions Direction;
        int NumUnits;

         public Instruction(int numUnits, Directions direction)
        {
            Direction = direction;
            NumUnits = numUnits;
        }
    }
}
