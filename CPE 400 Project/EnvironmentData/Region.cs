using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE400Project.EnvironmentData
{
    /// <summary>
    /// Denotes a region - used to track home base position.
    /// </summary>
    public class Region
    {
        #region Constructors
        public Region(int xMin, int xMax, int yMin, int yMax)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
        }
        #endregion Constructors

        #region Properties
        public int XMin { get; set; }
        public int XMax { get; set; }
        public int XCenter { 
            get
            {
                return (XMax + XMin) / 2;
            } 
        }
        public int YMin { get; set; }
        public int YMax { get; set; }
        public int YCenter
        {
            get
            {
                return (YMax + YMin) / 2;
            }
        }
        #endregion Properties
    }
}
