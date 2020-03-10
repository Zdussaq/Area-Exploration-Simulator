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

        public Map(int height, int width)
        {
            GenerateMap(height, width);
        }


        #endregion Constructors

        #region Properties

        public IList<IList<Chunk>> Chunks { get; set; }

        #endregion Properties

        #region Public Functions

        public void GenerateMap(int height, int width)
        {
            Chunks = new List<IList<Chunk>>();

            for (int i = 0; i < height; i++)
            {
                IList<Chunk> tempList = new List<Chunk>();
                for (int j = 0; j < width; j++)
                {
                    tempList.Add(new Chunk(5));
                }
                Chunks.Add(tempList);
            }
        }

        #endregion Public Fucntions
    }
}
