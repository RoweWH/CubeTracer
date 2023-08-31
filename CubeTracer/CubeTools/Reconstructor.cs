using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CubeTracer.DataAccess.Models;
using CubeTracer.DataAccess;



namespace CubeTracer.CubeTools
{
    /// <summary>
    /// Static class responsible for reconstructing cube and storing all relevant information into a reconstruction model for viewing.
    /// </summary>
    public static class Reconstructor
    {
        /// <summary>
        /// Constructor that takes in EdgeMemo and CornerMemo. It then creates a reconstruction model and populates its
        /// contents with information from SqLiteConnector based on the memos.
        /// </summary>
        /// <param name="EdgeMemo"></param>
        /// <param name="CornerMemo"></param>
        /// <returns></returns>
        public static ReconstructionModel Reconstruct(string EdgeMemo, string CornerMemo)
        {
            ReconstructionModel recon = new ReconstructionModel();
            recon.EdgeImages = SqLiteConnector.GetImages(separatePairs(EdgeMemo));
            recon.CornerImages = SqLiteConnector.GetImages(separatePairs(CornerMemo));
            recon.EdgeAlgs = SqLiteConnector.GetAlgs(separatePairs(EdgeMemo), 1);
            recon.CornerAlgs = SqLiteConnector.GetAlgs(separatePairs(CornerMemo), 2);
            return recon;
        }

        /// <summary>
        /// This method is for separating a memo string into a list of letter pairs.
        /// Flip and twist pairs that have parentheses are also acknowledged and keep their
        /// parentheses.
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        public static List<string> separatePairs(string memo)
        {
            List<string> pairs = new List<string>();
            string newPair = string.Empty;
            for(int i = 0; i < memo.Length; i++)
            {
                //Stores pairs surrounded with ()
                if (char.IsPunctuation(memo[i]))
                {
                    newPair = memo.Substring(i, 4);
                    i += 3;
                    pairs.Add(newPair);
                    newPair = string.Empty;
                }
                //adds char to newPair
                else if(i % 2 == 0)
                {
                    newPair += memo[i];
                }
                //adds char to newPair and adds to pairs. Resets newPair
                else if(i % 2 == 1)
                {
                    pairs.Add(newPair + memo[i]);
                    newPair = string.Empty;
                }
            }
            return pairs;
        }

    }
}
