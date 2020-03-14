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

        public Map(int height, int width, float[] scales)
        {
            GenerateMap(height, width, scales);
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

        public void GenerateMap(int height, int width, float[] scales)
        {
            //Create the set
            Random random = new Random();
            TerrainGeneration.Seed = random.Next();
            Chunks = TerrainGeneration.Generate2DSpace(width, height, scales);

            //Adjust the values - first find min and max.
            float min = 1000000000000;
            float max = -1;
            foreach (var i in Chunks)
            {
                foreach (var j in i)
                {
                    if (j.Elevation > max)
                    {
                        max = j.Elevation;
                    }

                    if (j.Elevation < min)
                    {
                        min = j.Elevation;
                    }
                }
            }

            float multiplier = 222 / max;

            foreach (var i in Chunks)
            {
                foreach (var j in i)
                {
                    j.Elevation *= multiplier;
                }
            }

            min = 1000000000000;
            max = -1;
            foreach (var i in Chunks)
            {
                foreach (var j in i)
                {
                    if (j.Elevation > max)
                    {
                        max = j.Elevation;
                    }

                    if (j.Elevation < min)
                    {
                        min = j.Elevation;
                    }
                }
            }

            Debug.WriteLine($"Adjusted min: {min}, and max: {max}");

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
