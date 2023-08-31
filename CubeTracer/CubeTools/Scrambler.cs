using Kociemba;
using CubeTypes;

namespace CubeTools
{
    /// <summary>
    /// The static Scrambler class is responsible for scrambling a cube.
    /// Instead of random move scrambling, this class uses the Solver project to generate a completely random
    /// state for the cube and find its solution. The scramble is then found by inverting the solution. This is known 
    /// as random state scrambling, which is the preferred method for scrambling in the cubing community. 
    /// To accomplish this I borrowed the code for the Solver project from Megalomatt, who has a C# implementation 
    /// of the Kociemba Solver program on his GitHub. All credit for the Solver program goes to him.
    /// https://github.com/Megalomatt/Kociemba/tree/master
    /// </summary>
    public static class Scrambler
    {
        /// <summary>
        /// This method fetches the scramble and applies each turn to the cube
        /// </summary>
        /// <param name="cube"></param>
        /// <returns></returns>
        public static string scrambleCube(Cube cube)
        {
            string s = generateRandomCube(cube);
            var turns = s.Split(" ");
            foreach (var t in turns)
            {
                cube.turnCube(t);
            }
            return s;

        }
        /// <summary>
        /// This method returns the scramble.
        /// 1. Generate Random Cube
        /// 2. Find solution to random cube
        /// 3. Inverts the solution and returns the scramble
        /// </summary>
        /// <returns></returns>

        private static string generateRandomCube(Cube cube)
        {
            string info = "";
            string searchString = Tools.randomCube();
            cube.ColorString = searchString;
            //string solution = SearchRunTime.solution(searchString, out info, buildTables: true);
            string solution = Search.solution(searchString, out info);
            string scramble = string.Empty;
            var turns = solution.Split(" ");
            for (int i = turns.Length - 1; i >= 0; i--)
            {
                if (turns[i].Contains('\''))
                {
                    scramble += turns[i][0] + " ";
                }
                else if (turns[i].Length == 1 && (!turns[i].Contains('\'')))
                {
                    scramble += turns[i] + "' ";
                }
                else scramble += turns[i] + " ";
            }
            return scramble;


        }
    }
}