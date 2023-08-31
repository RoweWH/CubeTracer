using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CubeTypes;

namespace CubeTools
{
    public static class CubeTracer
    {
        /// <summary>
        /// public method to trace cube
        /// </summary>
        /// <param name="c"></param>
        public static void traceCube(Cube c)
        {
            traceEdges(c);
            traceCorners(c);
        }
        private static void traceEdges(Cube c)
        {
            //INITIAL TRACE
            //First we convert c.Edges to a generic List of Piece. This is done with the corners too in
            //the traceCorners method. This way we can use the same methods on both piece types throughout the tracing process
            //VARIABLES:
            //pieceIndex: keeps track of index in SolvedCube.EdgeLetters that we want. For edges it's always between 0 and 11
            //stickerIndex: keeps track of sticker of SolvedCube.EdgeLetters[pieceIndex] that we want. For edges it's always 0 or 1
            //bufferPiece: keeps track of the piece we are currently memoing from. When we encounter the two letters on this piece again
            //while memoing, that means the cycle is over.
            //next: this stores the value of the next sticker we have to look at.
            List<Piece> e = new List<Piece>();
            foreach (var edge in c.Edges)
            {
                Edge newEdge = new Edge(edge.Stickers);
                e.Add(newEdge);
            }
            e[0].Hit = true;
            searchSolvedPieces(e);
            searchFlipsandTwists(e);
            int pieceIndex = 0, stickerIndex = 0;
            string bufferPiece = e[0].Stickers;
            char next = bufferPiece[0];
            bufferPiece = SolvedCube.EdgeLetters[0];
            c.EdgeMemo += memoCycle(e, 12, ref next, bufferPiece, ref pieceIndex, ref stickerIndex);
           //WEAKSWAP:
           //If the memo isn't complete and UR isn't hit yet, this section of code is called.
           //UF/UR weakswap means assuming UF/UR swap to handle parity later in the solve. If it turns out we don't
           //have parity in the solve, then we simply fix it later on by adding UR to the memo towards the end.
           //If UR is a flipped piece, we still use it now. Similar process to the one above, but starting from UR or RU depending on
           //last stickerIndex.
            if ((!piecesSolved(e)) && (!(e[3].Hit) || e[3].FlipTwist))
            {
                e[3].FlipTwist = false;
                stickerSearch(e, 12, ref next, ref pieceIndex, ref stickerIndex);
                bufferPiece = e[3].Stickers;
                e[3].Hit = true;
                next = bufferPiece[stickerIndex];
                bufferPiece = SolvedCube.EdgeLetters[3];
                c.EdgeMemo += bufferPiece[stickerIndex];
                c.EdgeMemo += memoCycle(e, 12, ref next, bufferPiece, ref pieceIndex, ref stickerIndex);
            }
            //REMAINING CYCLES(MINUS EDGEFLIPS)
            //This section of code is run if there are still cycles of pieces left to be traced into EdgeMemo. There are
            //often small cycles of pieces and two swaps of pieces on scrambles, so this is very common. For this section,
            //the process is just to find a piece that has not yet been used and start another memo cycle. This process will repeat
            //until there are no more available pieces.
            
            while (!piecesSolved(e))
            {
                next = findUnsolvedPiece(e);
                c.EdgeMemo += next;
                stickerSearch(e, 12, ref next, ref pieceIndex, ref stickerIndex);
                bufferPiece = SolvedCube.EdgeLetters[pieceIndex];
                next = findNextPiece(e, pieceIndex, stickerIndex);
                c.EdgeMemo += memoCycle(e, 12, ref next, bufferPiece, ref pieceIndex, ref stickerIndex);
                c.EdgeMemo += next;
            }
            //EdgeFlip scenarios
            //case 1: even length memo and no flips, done
            //case 2: odd length memo and no flips, add UR letter to memo
            //case 3: even length memo and odd # flips: add flips to memo
            //case 4: odd length memo and odd + flips: 
            //- If UR is included in edgeFlips, add RU letter to memo and remove it from edgeFlips list
            //- If not, just add first edgeFlip to memo and add UR letter to make the memo length even
            //- add remaining edgeFlips to memo
            //case 5: even length memo and even # flips: add flips to memo
            //case 6: odd length memo and even number of edgeFlips:
            //-Add UR letter to memo
            //-If UR is included in edgeFlips, remove it from list
            //-add rest of flips to memo
            
            
            //create separate list for the edgeFlips
            List<string> edgeFlips = new List<string>();
            foreach (var p in e)
            {
                if (p.FlipTwist) edgeFlips.Add(p.Stickers);
            }
            //case 1: do nothing
            //case 2:
            if((c.EdgeMemo.Length % 2 == 1) && edgeFlips.Count == 0)
            {
                c.EdgeMemo += SolvedCube.EdgeLetters[3][0];
            }
            //case 3
            else if ((c.EdgeMemo.Length % 2 == 0) && (edgeFlips.Count % 2 == 1))
            {
                foreach (var f in edgeFlips)
                {
                    c.EdgeMemo += "(" + f + ")";
                }
            }
            //case 4
            else if ((c.EdgeMemo.Length % 2 == 1) && (edgeFlips.Count % 2 == 1))
            {
                if (edgeFlips.Contains("SD"))
                {
                    c.EdgeMemo += 'S';
                    edgeFlips.RemoveAt(edgeFlips.IndexOf("SD"));
                }
                else
                {
                    c.EdgeMemo += edgeFlips[0] + SolvedCube.EdgeLetters[3][0];
                    edgeFlips.RemoveAt(0);
                }
                foreach(var f in edgeFlips)
                {
                    c.EdgeMemo += "(" + f + ")";
                }




            }
            //case 5
            else if ((c.EdgeMemo.Length % 2 == 0) && (edgeFlips.Count % 2 == 0))
            {
                foreach (var f in edgeFlips)
                {
                    c.EdgeMemo += "(" + f + ")";
                }
            }
            //case 6
            else if ((c.EdgeMemo.Length % 2 == 1) && (edgeFlips.Count % 2 == 0))
            {
                c.EdgeMemo += SolvedCube.EdgeLetters[3][0];
                if (edgeFlips.Contains("SD")){
                    edgeFlips.RemoveAt(edgeFlips.IndexOf("SD"));
                }
                foreach(var f in edgeFlips)
                {
                    c.EdgeMemo += "(" + f + ")";
                }
            }
            //fix DD error
            //In this algorithm, some cases leave a leftover 'DD' added to edgeMemo that is unnecessary. This code simply removes it.
            if (c.EdgeMemo.Contains("DD"))
            {
                
                c.EdgeMemo = c.EdgeMemo.Remove(c.EdgeMemo.IndexOf("DD"), 2);
                
            }
            //fix SD error
            //In this algorithm, there are some rare weak swap cases that leave SD in the memo string without parens. It is technically an edge flip,
            //so it needs to be (SD) so SqLiteConnector knows to ask the database for a flip alg for that letter pair, not a normal one. This
            //code simply adds the parentheses around the SD in the memo string.
            if (c.EdgeMemo.Contains("SD"))
            {
                if(!c.EdgeMemo.Contains("(SD)"))
                {
                    
                    c.EdgeMemo = c.EdgeMemo.Replace("SD", "(SD)");
                    
                }

            }
            


        }
        private static void traceCorners(Cube c)
        {
            //INITIAL TRACE
            //First we convert c.Corners to a generic List of Piece. This way we can use the same methods on both piece types
            //throughout the tracing process
            //VARIABLES:
            //pieceIndex: keeps track of index in SolvedCube.CornerLetters that we want. For corners it's always between 0 and 7
            //stickerIndex: keeps track of sticker of SolvedCube.EdgeLetters[pieceIndex] that we want. For corners its always between 0 and 2
            //bufferPiece: keeps track of the piece we are currently memoing from. When we encounter the two letters on this piece again
            //while memoing, that means the cycle is over.
            //next: this stores the value of the next sticker we have to look at.
            List<Piece> corners = new List<Piece>();
            foreach (var corner in c.Corners)
            {
                Corner newCorner = new Corner(corner.Stickers);
                corners.Add(newCorner);
            }
            corners[3].Hit = true;
            searchSolvedPieces(corners);
            searchFlipsandTwists(corners);
            int pieceIndex = 0, stickerIndex = 0;
            string bufferPiece = corners[3].Stickers;
            char next = bufferPiece[0];
            bufferPiece = SolvedCube.CornerLetters[3];
            c.CornerMemo += memoCycle(corners, 8, ref next, bufferPiece, ref pieceIndex, ref stickerIndex);
            //REMAINING CYCLES(MINUS CORNER TWISTS)
            //This section of code is run if there are still cycles of pieces left to be traced into CornerMemo. There are
            //often small cycles of pieces and two swaps of pieces on scrambles, so this is very common. For this section,
            //the process is just to find a piece that has not yet been used and start another memo cycle. This process will repeat
            //until there are no more available pieces.
            while (!piecesSolved(corners))
            {

                next = findUnsolvedPiece(corners);
                c.CornerMemo += next;
                stickerSearch(corners, 8, ref next, ref pieceIndex, ref stickerIndex);
                bufferPiece = SolvedCube.CornerLetters[pieceIndex];
                next = findNextPiece(corners, pieceIndex, stickerIndex);
                c.CornerMemo += memoCycle(corners, 8, ref next, bufferPiece, ref pieceIndex, ref stickerIndex);
                c.CornerMemo += next;
            }
            //Corner Twist Cases
            //For corners, an odd number of targets means parity.
            //If our memo string is odd, then the last character of the string will be 
            //considered our parity target. That letter is then repeated onto the string to keep
            //it even and to easily see parity.
            //Example: PKBCETS -> PKBCETSS, S being the parity target
            //case 1: Even length corner memo and 0 twists. Done
            //case 2: Odd length corner memo and 0 twists. Add Parity letter
            //case 3: Even length corner memo and even twists. Add twists
            //case 4: Odd length corner memo and odd twists. Add one twist, add parity letter. Then add remaining twists
            //case 5: Odd length corner memo and even twists. Add Parity letter, then add twists.
            //case 6: Even length corner memo and odd twists. Just Add twists
            //***Can definitely reduce these cases now after simplified version of the program, but for now I'll leave it be.

            List<string> cornerTwists = new List<string>();
            for (int i = 0; i < corners.Count; i++)
            {
                if (corners[i].FlipTwist)
                {
                    cornerTwists.Add(getCornerTwistPair(corners[i].Stickers, i));
                }
            }
            //case1
            if ((c.CornerMemo.Length % 2 == 0) && cornerTwists.Count == 0)
            {
                //done
            }
            //case2
            else if ((c.CornerMemo.Length % 2 == 1) && cornerTwists.Count == 0)
            {
                c.CornerMemo += c.CornerMemo[c.CornerMemo.Length - 1];
            }

            //case3
            else if ((c.CornerMemo.Length % 2 == 0) && cornerTwists.Count % 2 == 0)
            {

                while (cornerTwists.Count > 0)
                {
                    c.CornerMemo += "(" + cornerTwists[0] + ")";
                    cornerTwists.RemoveAt(0);
                }
            }
            //case 4
            else if ((c.CornerMemo.Length % 2 == 1) && cornerTwists.Count % 2 == 1)
            {
                c.CornerMemo += cornerTwists[0];
                c.CornerMemo += c.CornerMemo[c.CornerMemo.Length - 1];
                cornerTwists.RemoveAt(0);
                while (cornerTwists.Count > 0)
                {
                    c.CornerMemo += "(" + cornerTwists[0] + ")";
                    cornerTwists.RemoveAt(0);
                }
            }
            //case5
            else if ((c.CornerMemo.Length % 2 == 1) && cornerTwists.Count % 2 == 0)
            {
                c.CornerMemo += c.CornerMemo[c.CornerMemo.Length - 1];
                while (cornerTwists.Count > 0)
                {
                    c.CornerMemo += "(" + cornerTwists[0] + ")";
                    cornerTwists.RemoveAt(0);
                }
            }
            //case6
            else if ((c.CornerMemo.Length % 2 == 0) && cornerTwists.Count % 2 == 1)
            {
                while (cornerTwists.Count > 0)
                {
                    c.CornerMemo += "(" + cornerTwists[0] + ")";
                    cornerTwists.RemoveAt(0);
                }
            }
            





        }
        /// <summary>
        /// This method searches through all the pieces given and marks their hit field true if they are a solved piece.
        /// </summary>
        /// <param name="pieces"></param>
        private static void searchSolvedPieces(List<Piece> pieces)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] is Edge)
                {
                    if (pieces[i].Stickers == SolvedCube.EdgeLetters[i])
                    {
                        pieces[i].Hit = true;
                    }
                }
                else if (pieces[i] is Corner)
                {
                    if (pieces[i].Stickers == SolvedCube.CornerLetters[i])
                    {
                        pieces[i].Hit = true;
                    }
                }

            }
        }
        /// <summary>
        /// This method searches through list of pieces and marks their flipTwist and hit as true if
        /// The piece is in it's correct position but oriented incorrectly.
        /// </summary>
        /// <param name="pieces"></param>
        private static void searchFlipsandTwists(List<Piece> pieces)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (!pieces[i].Hit)
                {
                    //For edges
                    if (pieces[i] is Edge)
                    {
                        //create solved edge for currentPosition, misorient it and compare
                        //if they are the same value, that means we have a flipped edge.
                        //Mark it as hit and flipTwist
                        string s = SolvedCube.EdgeLetters[i];
                        Edge twistedEdge = new Edge(s);
                        twistedEdge.orient(1);
                        if (pieces[i].Stickers == twistedEdge.Stickers)
                        {
                            pieces[i].FlipTwist = true;
                            pieces[i].Hit = true;
                        }

                    }
                    //For corners
                    else if (pieces[i] is Corner)
                    {
                        //create 2 solved corners for this current position.
                        //Misorient them each in different directions and compare
                        //to current piece. If the current piece matches either
                        //test corner, that means we have a corner twist. Mark it as hit
                        //and flipTwist.
                        string s = SolvedCube.CornerLetters[i];
                        char UD = s[0];
                        Corner twistedCorner1 = new Corner(s);
                        Corner twistedCorner2 = new Corner(s);
                        twistedCorner1.orient(1);
                        twistedCorner2.orient(-1);
                        if (pieces[i].Stickers == twistedCorner1.Stickers)
                        {
                            pieces[i].FlipTwist = true;
                            pieces[i].Hit = true;


                        }
                        else if (pieces[i].Stickers == twistedCorner2.Stickers)
                        {
                            pieces[i].FlipTwist = true;
                            pieces[i].Hit = true;


                        }
                    }
                }
            }
        }
        /// <summary>
        /// This method creates a string of memo for one cycle of pieces. Once next becomes equal to any sticker on the given bufferPiece,
        /// that means this cycle is over and method returns cycleMemo.
        /// </summary>
        /// <param name="pieces">List of pieces we are cycling through. Always a list of edges or corners</param>
        /// <param name="size">stores size of list</param>
        /// <param name="next">stores each sticker we find, we always compare with bufferPiece and usually add it to our cycleMemo</param>
        /// <param name="bufferPiece">stores the values of the stickers that mean to terminate the cycle</param>
        /// <param name="pieceIndex">keeps track of the piece we are looking at (SolvedCube)</param>
        /// <param name="stickerIndex">keeps track of the sticker of the piece at pieceIndex</param>
        /// <returns></returns>
        private static string memoCycle(List<Piece> pieces, int size, ref char next, string bufferPiece, ref int pieceIndex, ref int stickerIndex)
        {
            string cycleMemo = "";
            while (next != bufferPiece[0] && next != bufferPiece[1] && next != bufferPiece[bufferPiece.Length - 1])
            {
                cycleMemo += next;
                stickerSearch(pieces, size, ref next, ref pieceIndex, ref stickerIndex);
                next = findNextPiece(pieces, pieceIndex, stickerIndex);

            }
            return cycleMemo;
        }
        /// <summary>
        /// This method takes value of next and finds the correct position of that sticker on SolvedCube. By passing pieceIndex 
        /// and stickerIndex to this method by reference, we can keep track of what values these are left at once the sticker is
        /// found and the search is terminated. 
        /// 
        /// </summary>
        /// <param name="pieces">List of piece sent to method to be searched</param>
        /// <param name="size">size of the current list<param>
        /// <param name="next">the character we are searching for the position of</param>
        /// <param name="pieceIndex">keeps track of the piece index of the piece we are searching for</param>
        /// <param name="stickerIndex">keeps track of the sticker index on the piece that we are searching for</param>

        private static void stickerSearch(List<Piece> pieces, int size, ref char next, ref int pieceIndex, ref int stickerIndex)
        {
            
            bool found = false;
            pieceIndex = 0;
            stickerIndex = 0;
            while (!found)
            {
                string s = string.Empty;
                if (pieces[0] is Edge)
                {
                    s = SolvedCube.EdgeLetters[pieceIndex];
                }
                else if (pieces[0] is Corner)
                {
                    s = SolvedCube.CornerLetters[pieceIndex];
                }
                for (stickerIndex = 0; stickerIndex < pieces[0].Stickers.Length; stickerIndex++)
                {
                    if (s[stickerIndex] == next)
                    {
                        found = true;
                        break;
                    }
                }
                if (found) break;
                else pieceIndex++;
            }
            pieces[pieceIndex].Hit = true;
        }
        /// <summary>
        /// This method is responsible for finding char next based on pieceIndex and stickerIndex
        /// found in the stickersearch. After looking through this, this and stickerSearch can probably be
        /// combined into one method.
        /// </summary>
        /// <param name="pieces">List of pieces</param>
        /// <param name="pieceIndex">index of the piece in the list</param>
        /// <param name="stickerIndex">sticker index of the piece</param>
        /// <returns></returns>
        private static char findNextPiece(List<Piece> pieces, int pieceIndex, int stickerIndex)
        {
            string newPiece = pieces[pieceIndex].Stickers;
            return newPiece[stickerIndex];
        }
        /// <summary>
        /// Method to check if all the pieces have been hit or not
        /// </summary>
        /// <param name="pieces"></param>
        /// <returns></returns>
        private static bool piecesSolved(List<Piece> pieces)
        {
            bool solved = true;
            foreach (Piece p in pieces)
            {
                if (!p.Hit) solved = false;
            }
            return solved;
        }
        /// <summary>
        /// This method returns a sticker on the cube that has not yet been used.
        /// </summary>
        /// <param name="pieces"></param>
        /// <returns></returns>
        private static char findUnsolvedPiece(List<Piece> pieces)
        {
            foreach (var p in pieces)
            {
                if (!p.Hit)
                {
                    return p.Stickers[0];
                }
            }
            return '0';
        }

        /// <summary>
        /// This method is used to find a valid 2character string that correctly portrays a corner twist.
        /// </summary>
        /// <param name="twist"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string getCornerTwistPair(string twist, int index)
        {
            char UD = SolvedCube.CornerLetters[index][0];
            Corner twistedCorner = new Corner(SolvedCube.CornerLetters[index]);
            twistedCorner.orient(1);
            if (twist == twistedCorner.Stickers)
            {
                return "" + twist[2] + UD;
            }

            else return "" + twist[1] + UD;


        }



    }


}

