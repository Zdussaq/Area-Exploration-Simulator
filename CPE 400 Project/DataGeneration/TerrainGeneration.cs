﻿using CPE_400_Project.EnvironmentData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPE_400_Project.DataGeneration
{
    /// <summary>
    /// Terain generation for elevation creation using the simplex noise algorithm
    /// Implementation here is based on a C# implementation by Benjamin Ward
    /// Usage and modification of his code is allowed under the BSD-3 Clause.
    /// </summary>
    public static class TerrainGeneration
    {

        public static IList<IList<Chunk>> GenerateElevationProfile(int width, int height)
        {

            //These are used as the baseline to generate the map.
            //Altering these will change the scale of the image
            //Lower values = larger objects
            //Higher values = smaller objects
            //More values = fewer objects
            float[] scales = new[] { 0.02f, 0.01f, 0.0025f };

            
            IList<IList<Chunk>> MapData = new List<IList<Chunk>>();
            for (var i = 0; i < height; i++)
            {
                IList<Chunk> temp = new List<Chunk>();
                for (var j = 0; j < width; j++)
                {
                    float elevation = 0;

                    foreach (var scale in scales)
                    {
                        elevation += Generate(i * scale, j * scale);
                    }

                    elevation /= scales.Length; // Value is now between -1 and 1
                    elevation += 1;
                    elevation = (float)Math.Pow(elevation, 5);
                    

                    //elevation = (float)Math.Round(((elevation / scales.Length) * 128 + 128), 0);
                    temp.Add(new Chunk(elevation));
                }
                MapData.Add(temp);
            }

            float min = 1000000000000;
            float max = -1;
            foreach (var i in MapData)
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

            Debug.WriteLine($"Min: {min}, Max: {max}");

            return MapData;

        }

        private static float Generate(float x, float y)
        {
            const float F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
            const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

            float n0, n1, n2; // Noise contributions from the three corners

            // Skew the input space to determine which simplex cell we're in
            var s = (x + y) * F2; // Hairy factor for 2D
            var xs = x + s;
            var ys = y + s;
            var i = FastFloor(xs);
            var j = FastFloor(ys);

            var t = (i + j) * G2;
            var X0 = i - t; // Unskew the cell origin back to (x,y) space
            var Y0 = j - t;
            var x0 = x - X0; // The x,y distances from the cell origin
            var y0 = y - Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0) { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6

            var x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            var y1 = y0 - j1 + G2;
            var x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            var y2 = y0 - 1.0f + 2.0f * G2;

            // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
            var ii = Mod(i, 256);
            var jj = Mod(j, 256);

            // Calculate the contribution from the three corners
            var t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 < 0.0f) n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * Grad(_perm[ii + _perm[jj]], x0, y0);
            }

            var t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 < 0.0f) n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * Grad(_perm[ii + i1 + _perm[jj + j1]], x1, y1);
            }

            var t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 < 0.0f) n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * Grad(_perm[ii + 1 + _perm[jj + 1]], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 40.0f * (n0 + n1 + n2); // TODO: The scale factor is preliminary!
        }

        private static int FastFloor(float x)
        {
            return (x > 0) ? ((int)x) : (((int)x) - 1);
        }
        private static int Mod(int x, int m)
        {
            var a = x % m;
            return a < 0 ? a + m : a;
        }

        private static float Grad(int hash, float x, float y)
        {
            var h = hash & 7;      // Convert low 3 bits of hash code
            var u = h < 4 ? x : y;  // into 8 simple gradient directions,
            var v = h < 4 ? y : x;  // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
        }

        private static byte[] _perm;

        private static readonly byte[] PermOriginal = {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        public static int Seed
        {
            get => _seed;
            set
            {
                if (value == 0)
                {
                    _perm = new byte[PermOriginal.Length];
                    PermOriginal.CopyTo(_perm, 0);
                }
                else
                {
                    _perm = new byte[512];
                    var random = new Random(value);
                    random.NextBytes(_perm);
                }

                _seed = value;
            }
        }

        private static int _seed;
    }
}
