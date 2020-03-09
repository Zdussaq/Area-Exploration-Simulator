using CPE_400_Project.EnvironmentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE400Project.EnvironmentData
{
    class Map
    {
        #region Constructors

        public Map(int mapSize)
        {
            GenerateMapRandom(mapSize);
        }

        public Map(int length, int width)
        {
            GenerateMapBlock(length, width);
        }


        #endregion Constructors

        #region Properties

        public IList<IList<Chunk>> Chunks { get; set; }

        #endregion Properties

        #region Public Functions

        public void GenerateMapBlock(int length, int width)
        {
            Chunks = new List<IList<Chunk>>();
        }

        public void GenerateMapRandom(int mapSize)
        {
            Chunks = new List<IList<Chunk>>();
        }

        #endregion Public Fucntions
    }
}
