using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using static System.Console;


namespace CubeTypes
{

    public class Cube
    {
        /// <summary>
        /// The ColorString is used to create the scramble image on the form. This is created by the Solver class when we
        /// create a new scramble.
        /// </summary>
        public string ColorString { get; set; }
       /// <summary>
       /// List of Edges to represent the edges of a full cube
       /// </summary>
        public List<Edge> Edges { get; set; }
        /// <summary>
        /// List of Corners to represent the corners of a full cube
        /// </summary>
        public List<Corner> Corners { get; set; }
        /// <summary>
        /// Represents the string of edge memorization
        /// </summary>
        public string EdgeMemo { get; set; }
        /// <summary>
        /// Represents the string of corner memorization
        /// </summary>
        public string CornerMemo { get; set; }
        /// <summary>
        /// Constructor to initialize and set cube to a solved state
        /// </summary>
        public Cube()
        {
            ColorString = string.Empty;
            Edges = new List<Edge>();
            Corners = new List<Corner>();
            EdgeMemo = string.Empty;
            CornerMemo = string.Empty;
            setSolvedState();
           
        }
        /// <summary>
        /// Method to set Edges and Corners to a solved Cube
        /// </summary>
        public void setSolvedState()
        {
            Edge newEdge;
            Corner newCorner;
            foreach (var s in SolvedCube.EdgeLetters)
            {
                newEdge = new Edge(s);
                Edges.Add(newEdge);
            }
            foreach (var s in SolvedCube.CornerLetters)
            {
                newCorner = new Corner(s);
                Corners.Add(newCorner);
            }
        }
        /// <summary>
        /// Method for printing the cube in its current position
        /// </summary>
       public void printCube()
        {
            for (int i = 0; i < 12; i++)
            {
                WriteLine(Edges[i].Stickers);
            }
            for (int i = 0; i < 8; i++)
            {
                WriteLine(Corners[i].Stickers);
            }
        }
        /// <summary>
        /// method that sends the appropriate table to the applyTurn method the appropriate number of times.
        /// Clockwise turns call applyTurn once
        /// Double turns call applyTurn twice
        /// Counter-Clockwise turns call applyTurn three times
        /// </summary>
        /// <param name="turn"></param>

        public void turnCube(string turn)
        {
            switch (turn)
            {
                case "R":
                    {
                        applyTurn(SolvedCube.tableR);
                        break;
                    }
                case "F":
                    {
                        applyTurn(SolvedCube.tableF);
                        break;
                    }
                case "L":
                    {
                        applyTurn(SolvedCube.tableL);
                        break;
                    }
                case "B":
                    {
                        applyTurn(SolvedCube.tableB);
                        break;
                    }
                case "U":
                    {
                        applyTurn(SolvedCube.tableU);
                        break;
                    }
                case "D":
                    {
                        applyTurn(SolvedCube.tableD);
                        break;
                    }
                case "R\'":
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            applyTurn(SolvedCube.tableR);
                        }
                        break;
                    }
                case "F\'":
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            applyTurn(SolvedCube.tableF);
                        }
                        break;
                    }
                case "L\'":
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            applyTurn(SolvedCube.tableL);
                        }
                        break;
                    }
                case "B\'":
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            applyTurn(SolvedCube.tableB);
                        }
                        break;
                    }
                case "U\'":
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            applyTurn(SolvedCube.tableU);
                        }
                        break;
                    }
                case "D\'":
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            applyTurn(SolvedCube.tableD);
                        }
                        break;
                    }
                case "R2":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            applyTurn(SolvedCube.tableR);
                        }
                        break;
                    }
                case "F2":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            applyTurn(SolvedCube.tableF);
                        }
                        break;
                    }
                case "L2":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            applyTurn(SolvedCube.tableL);
                        }
                        break;
                    }
                case "B2":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            applyTurn(SolvedCube.tableB);
                        }
                        break;
                    }
                case "U2":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            applyTurn(SolvedCube.tableU);
                        }
                        break;
                    }
                case "D2":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            applyTurn(SolvedCube.tableD);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Cycles the list of pieces by doing 3 simple swaps
        /// </summary>
        /// <param name="pieceCycle"></param>
        public void cycle(List<Piece> pieceCycle)
        {
            string temp;
            for (int i = 0; i < 3; i++)
            {
                temp = pieceCycle[i + 1].Stickers;
                pieceCycle[i + 1].Stickers = pieceCycle[i].Stickers;
                pieceCycle[i].Stickers = temp;
            }
        }
     
        
        /// <summary>
        /// Applies turn to the cube based on the table that is passed to the method
        /// </summary>
        /// <param name="table"></param>
        public void applyTurn(int[,] table)
        {
            //create two lists, one for edges and one for corners
            List<Piece> cornerCycle = new List<Piece>();
            List<Piece> edgeCycle = new List<Piece>();
            //populate the lists with pieces based on the indexes provided by the table
            for (int i = 0; i < 4; i++)
            {
                cornerCycle.Add(Corners[table[0, i]]);//these are the corners to be cycled for the turn
                edgeCycle.Add(Edges[table[0, i + 4]]);//these are the edge to be cycled for the turn
            }
            cycle(cornerCycle);
            cycle(edgeCycle);
            List<Piece> pieces = new List<Piece>();
            //combine the two lists and orient each piece based on the orientation value provided by the table
            pieces.AddRange(cornerCycle);
            pieces.AddRange(edgeCycle);
            for (int i = 0; i < 8; i++)
            {
                pieces[i].orient(table[1, i]);
            }

        }
    }
}

