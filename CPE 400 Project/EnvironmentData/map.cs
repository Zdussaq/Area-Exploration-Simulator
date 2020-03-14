using CPE_400_Project.DataGeneration;
using CPE_400_Project.EnvironmentData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE400Project.EnvironmentData
{
    public class Map
    {
        #region Constructors

        public Map(int height, int width)
        {
            GenerateMap(height, width);
        }


        #endregion Constructors

        #region Properties

        public IList<IList<Chunk>> Chunks { get; set; }

        public int Width
        {
            get { return Chunks[0].Count; }
        }

        public int Height
        {
            get { return Chunks.Count; }
        }

        #endregion Properties

        #region Public Functions

        public void GenerateMap(int height, int width)
        {
            //Create the Terrain
            Random random = new Random();
            TerrainGeneration.Seed = random.Next();
            Chunks = TerrainGeneration.GenerateElevationProfile(width, height);

        }

        #endregion Public Fucntions

        #region Overides

        public IList<Chunk> this[int key]
        {
            get
            {
                return Chunks[key];
            }
            set
            {
                if (key > Chunks.Count)
                {
                    throw new IndexOutOfRangeException("Cannot index outside of range of list.");
                }
                Chunks[key] = value;
            }
        }

        #endregion Overides
    }
}
