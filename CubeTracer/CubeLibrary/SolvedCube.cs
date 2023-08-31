using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeTypes
{
    /// <summary>
    /// static class that contains all the starting positions for a solved cube as well as the tables for correctly turning the cube
    /// </summary>

    public static class SolvedCube
    {
        /// <summary>
        /// Represents the solved positions for edge pieces on a cube.
        /// </summary>
        public static string[] EdgePositions = { "UF", "UL", "UB", "UR", "FL", "BL", "BR", "FR", "DF", "DL", "DB", "DR" };
        /// <summary>
        /// Represents the solved positions for corner pieces on a cube.
        /// </summary>
        public static string[] CornerPositions = { "UFL", "ULB", "UBR", "URF", "DLF", "DBL", "DRB", "DFR" };
        /// <summary>
        /// Represents the letters for each piece/sticker corresponding to EdgePositions
        /// </summary>
        public static string[] EdgeLetters = { "AG", "BK", "CO", "DS", "FL", "PJ", "NT", "HR", "WE", "VI", "UM", "XQ" };
        /// <summary>
        /// Represents the letters for each piece/sticker corresponding to CornerPositions;
        /// </summary>
        public  static string[] CornerLetters = { "AFK", "BJO", "CNS", "DRG", "VLE", "UPI", "XTM", "WHQ" };
        /// <summary>
        /// Table of information needed for the cube to perform an R turn.
        /// The first 4 values of the first array represent the indexes of the corners in the Corners list to be rotated
        /// The first 4 values of the first array represent the indexes of the edges in the Edges list to be rotated
        /// The second array contains the direction that each piece in the first array needs to be oriented.
        /// Once all of the pieces are rotated and the correct orientations are applied, an R turn is completed.
        /// </summary>
        public static int[,] tableR = new int[2, 8] { { 3, 7, 6, 2, 3, 7, 11, 6 }, { -1, 1, -1, 1, 0, 0, 0, 0 } };
        /// <summary>
        /// Table of information needed for the cube to perform an F turn.
        /// The first 4 values of the first array represent the indexes of the corners in the Corners list to be rotated
        /// The first 4 values of the first array represent the indexes of the edges in the Edges list to be rotated
        /// The second array contains the direction that each piece in the first array needs to be oriented.
        /// Once all of the pieces are rotated and the correct orientations are applied, an F turn is completed.
        /// </summary>
        public static int[,] tableF = new int[2, 8] { { 0, 4, 7, 3, 0, 4, 8, 7 }, { -1, 1, -1, 1, 1, 1, 1, 1 } };
        /// <summary>
        /// Table of information needed for the cube to perform an L turn.
        /// The first 4 values of the first array represent the indexes of the corners in the Corners list to be rotated
        /// The first 4 values of the first array represent the indexes of the edges in the Edges list to be rotated
        /// The second array contains the direction that each piece in the first array needs to be oriented.
        /// Once all of the pieces are rotated and the correct orientations are applied, an L turn is completed.
        /// </summary>
        public static int[,] tableL = new int[2, 8] { { 1, 5, 4, 0, 1, 5, 9, 4 }, { -1, 1, -1, 1, 0, 0, 0, 0 } };
        /// <summary>
        /// Table of information needed for the cube to perform a B turn.
        /// The first 4 values of the first array represent the indexes of the corners in the Corners list to be rotated
        /// The first 4 values of the first array represent the indexes of the edges in the Edges list to be rotated
        /// The second array contains the direction that each piece in the first array needs to be oriented.
        /// Once all of the pieces are rotated and the correct orientations are applied, a B turn is completed.
        /// </summary>
        public static int[,] tableB = new int[2, 8] { { 2, 6, 5, 1, 2, 6, 10, 5 }, { -1, 1, -1, 1, 1, 1, 1, 1 } };
        /// <summary>
        /// Table of information needed for the cube to perform a U turn.
        /// The first 4 values of the first array represent the indexes of the corners in the Corners list to be rotated
        /// The first 4 values of the first array represent the indexes of the edges in the Edges list to be rotated
        /// The second array contains the direction that each piece in the first array needs to be oriented.
        /// Once all of the pieces are rotated and the correct orientations are applied, a U turn is completed.
        /// </summary>
        public static int[,] tableU = new int[2, 8] { { 0, 3, 2, 1, 1, 0, 3, 2 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
        /// <summary>
        /// Table of information needed for the cube to perform a D turn.
        /// The first 4 values of the first array represent the indexes of the corners in the Corners list to be rotated
        /// The first 4 values of the first array represent the indexes of the edges in the Edges list to be rotated
        /// The second array contains the direction that each piece in the first array needs to be oriented.
        /// Once all of the pieces are rotated and the correct orientations are applied, a D turn is completed.
        /// </summary>
        public static int[,] tableD = new int[2, 8] { { 7, 4, 5, 6, 11, 8, 9, 10 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
    }
}