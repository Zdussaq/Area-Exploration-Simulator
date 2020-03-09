using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE_400_Project.EnvironmentData
{
    /// <summary>
    /// Represents a pixel area in the map. Will be contained in a 2d Array within the map. 
    /// Contains status info for the particular chunk, i.e. exlplored, elevation, homeBase, and any other things that may need to be included
    /// </summary>
    class Chunk
    {
        #region Constructors

        public Chunk(int elevation, bool isHomeBase = false)
        {
            Elevation = elevation;
            Explored = false;
            IsHomeBase = isHomeBase;
            Drones = new List<int>();
        }

        #endregion Constructors

        #region Properties

        public float Elevation { get; }

        public bool Explored { get; set; }

        public bool IsHomeBase { get; set; }

        //This might need to go into the map itself instead - not sure.
        public IList<int/*to be replaced with drone class*/> Drones { get; set; }

        #endregion Properties
    }
}
