using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using CubeTracer.DataAccess.Models;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Runtime.CompilerServices;

namespace CubeTracer.DataAccess
{
    /// <summary>
    /// Static class for SQLite DB connection.
    /// </summary>
    public static class SqLiteConnector

    {
        
        private static string db = "BLDDB";
        private static string connectionString = ConfigurationManager.ConnectionStrings[db].ConnectionString;
        /// <summary>
        /// This method is responsible for retrieving the images from the database that correspond to the given letter pairs;
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static List<ImageModel> GetImages(List<string> pairs)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                List<ImageModel> output = new List<ImageModel>();
                ImageModel image;
                foreach (string pair in pairs)
                {
                    //If the pair is a flip or twist, send substring of middle two chars inside the ()
                    if(pair.Length == 4)
                    {
                        image = connection.Query<ImageModel>($"SELECT * FROM IMAGES WHERE LetterPair = '{pair.Substring(1, 2)}';").ToList().First();
                    }
                    //if not, just send in the pair normally
                    else
                    {
                        image = connection.Query<ImageModel>($"SELECT * FROM IMAGES WHERE LetterPair = '{pair}';").ToList().First();
                    }
                    output.Add(image);
                }

                return output;
            }
        }
        /// <summary>
        /// This method is responsible for retrieving algorithms from the database based on the pairs given. The value field also tells the method 
        /// which type of piece we are working with. Value = 1 means edges, value = 2 means corners. Edge flip, corner twist, and parity algs are
        /// also handled by this method.
        /// </summary>
        /// <param name="pairs">list of letter pairs</param>
        /// <param name="value">tells method whether we are working with edges(1) or corners(2)</param>
        /// <returns></returns>
        public static List<AlgModel> GetAlgs(List<string> pairs, int value)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                List<AlgModel> output = new List<AlgModel>();
                AlgModel alg;
                //If Edges
                if (value == 1)
                {
                    foreach (string pair in pairs)
                    {
                        //If Edge Flip 
                        if (pair.Length == 4)
                        {
                            SQLiteCommand command = new SQLiteCommand($@"
                             SELECT FA.AlgID, BP1.Piece, FT1.Piece, ST1.Piece, FA.Alg, SA.AlgID, BP2.Piece, FT2.Piece, ST2.Piece, SA.Alg, Other
                             FROM EdgeFlipAlgs
                             left JOIN EdgeFlipCases
                             ON EdgeFlipAlgs.CaseID = EdgeFlipCases.CaseID
                             left join Pieces
                             ON Pieces.PieceID = EdgeFlipCases.Piece
                             left JOIN EdgeAlgs FA
                             ON FA.AlgID = EdgeFlipAlgs.FirstAlg
                             left join EdgeCases FC
                             ON FC.CaseID = FA.CaseID
                             left join  EdgeAlgs SA
                             ON SA.AlgID = EdgeFlipAlgs.SecondAlg
                             left join  EdgeCases SC
                             ON SC.CaseID = SA.CaseID
                             left join Pieces BP1
                             ON BP1.PieceID = FC.BufferPiece
                             left join Pieces FT1
                             ON FT1.PieceID = FC.FirstTarget
                             left join Pieces ST1
                             ON ST1.PieceID = FC.SecondTarget
                             left join Pieces BP2
                             ON BP2.PieceID = SC.BufferPiece
                             left join Pieces FT2
                             ON FT2.PieceID = SC.FirstTarget
                             left join  Pieces ST2
                             ON ST2.PieceID = SC.SecondTarget
                             WHERE Pieces.Letter = '{pair[1]}' OR Pieces.Letter = '{pair[2]}';", connection); ;
                            connection.Open();
                            
                            SQLiteDataReader reader = command.ExecuteReader();
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string other = SafeGetString(reader, 10);
                                    //If edge flip alg is of type "other"
                                    if (other != null)
                                    {
                                        //set all values null except for other
                                        AlgModel newAlg = new AlgModel();
                                        newAlg.ID = 0;
                                        newAlg.BufferPiece = null;
                                        newAlg.FirstTarget = null;
                                        newAlg.SecondTarget = null;
                                        newAlg.Alg = other;
                                        output.Add(newAlg);
                                    }
                                    //This edge flip alg is a combination of two edge algs. Collect all information
                                    //for both algs and store in output.
                                    else
                                    {
                                        AlgModel alg1 = new AlgModel();
                                        AlgModel alg2 = new AlgModel();
                                        alg1.ID = SafeGetInt(reader, 0);
                                        alg1.BufferPiece = SafeGetString(reader, 1);
                                        alg1.FirstTarget = SafeGetString(reader, 2);
                                        alg1.SecondTarget = SafeGetString(reader, 3);
                                        alg1.Alg = SafeGetString(reader, 4);
                                        alg2.ID = SafeGetInt(reader, 5);
                                        alg2.BufferPiece = SafeGetString(reader, 6);
                                        alg2.FirstTarget = SafeGetString(reader, 7);
                                        alg2.SecondTarget = SafeGetString(reader, 8);
                                        alg2.Alg = SafeGetString(reader, 9);
                                        output.Add(alg1);
                                        output.Add(alg2);
                                    }
                                }
                                reader.NextResult();
                            }
                            reader.Close();
                            connection.Close();
                        }
                        //Normal Edge Algorithm, simple fetch from database and add to output
                        else
                        {
                            string command = $@"SELECT EdgeAlgs.AlgID AS ID, BF.Piece AS BufferPiece, FT.Piece AS FirstTarget, ST.Piece AS SecondTarget, EdgeAlgs.Alg as Alg
                            FROM EdgeAlgs
                            INNER JOIN EdgeCases
                            ON EdgeAlgs.CaseID = EdgeCases.CaseID
                            INNER JOIN Pieces BF
                            ON BF.PieceID = EdgeCases.BufferPiece
                            INNER JOIN Pieces FT
                            ON FT.PieceID = EdgeCases.FirstTarget
                            INNER JOIN Pieces ST
                            ON ST.PieceID = EdgeCases.SecondTarget
                            INNER JOIN Images 
                            ON Images.LetterPair = FT.Letter || ST.Letter
                            WHERE LetterPair = '{pair}';";
                            alg = connection.Query<AlgModel>(command).ToList().First();
                            output.Add(alg);


                        }
                    }
                }
                //If Corners
                if (value == 2)
                {
                    foreach (string pair in pairs)
                    {
                        //If Corner Twist
                        if (pair.Length == 4)
                        {
                            SQLiteCommand command = new SQLiteCommand($@"SELECT FA.AlgID, BP1.Piece, FT1.Piece, ST1.Piece, FA.Alg, SA.AlgID, BP2.Piece, FT2.Piece, ST2.Piece, SA.Alg, Other
                                FROM CornerTwistAlgs
                                left JOIN CornerTwistCases
                                ON CornerTwistAlgs.CaseID = CornerTwistCases.CaseID
                                left join Pieces
                                ON Pieces.PieceID = CornerTwistCases.Piece
                                left JOIN CornerAlgs FA
                                ON FA.AlgID = CornerTwistAlgs.FirstAlg
                                left join CornerCases FC
                                ON FC.CaseID = FA.CaseID
                                left join  CornerAlgs SA
                                ON SA.AlgID = CornerTwistAlgs.SecondAlg
                                left join  CornerCases SC
                                ON SC.CaseID = SA.CaseID
                                left join Pieces BP1
                                ON BP1.PieceID = FC.BufferPiece
                                left join Pieces FT1
                                ON FT1.PieceID = FC.FirstTarget
                                left join Pieces ST1
                                ON ST1.PieceID = FC.SecondTarget
                                left join Pieces BP2
                                ON BP2.PieceID = SC.BufferPiece
                                left join Pieces FT2
                                ON FT2.PieceID = SC.FirstTarget
                                left join  Pieces ST2
                                ON ST2.PieceID = SC.SecondTarget
                                WHERE Pieces.Letter = '{pair[1]}'", connection);
                            connection.Open();
                            SQLiteDataReader reader = command.ExecuteReader();
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string other = SafeGetString(reader, 10);
                                    //If alg is of type "Other", then store alg
                                    //in other and set all other fields to null
                                    //add to output
                                    if (other != null)
                                    {
                                        AlgModel newAlg = new AlgModel();
                                        newAlg.ID = 0;
                                        newAlg.BufferPiece = null;
                                        newAlg.FirstTarget = null;
                                        newAlg.SecondTarget = null;
                                        newAlg.Alg = other;
                                        output.Add(newAlg);
                                    }
                                    //If not, that means this corner twist alg
                                    //is a combination of two corner algs. Collect
                                    //information for both algs and store in output
                                    else
                                    {
                                        AlgModel alg1 = new AlgModel();
                                        AlgModel alg2 = new AlgModel();
                                        alg1.ID = SafeGetInt(reader, 0);
                                        alg1.BufferPiece = SafeGetString(reader, 1);
                                        alg1.FirstTarget = SafeGetString(reader, 2);
                                        alg1.SecondTarget = SafeGetString(reader, 3);
                                        alg1.Alg = SafeGetString(reader, 4);
                                        alg2.ID = SafeGetInt(reader, 5);
                                        alg2.BufferPiece = SafeGetString(reader, 6);
                                        alg2.FirstTarget = SafeGetString(reader, 7);
                                        alg2.SecondTarget = SafeGetString(reader, 8);
                                        alg2.Alg = SafeGetString(reader, 9);
                                        output.Add(alg1);
                                        output.Add(alg2);
                                    }
                                }
                                reader.NextResult();
                            }
                            reader.Close();
                            connection.Close();
                        }
                        //If not twist alg, then there are two other options: normal corner algorithm or parity algorithm
                        else
                        {
                            //If parity algorithm (always double letters)
                            if (pair[0] == pair[1])
                            {
                                string command = $@"SELECT ParityAlgs.AlgID as ID, BF.Piece as BufferPiece, LT.Piece as FirstTarget, ParityAlgs.Alg as Alg
                                FROM ParityAlgs
                                INNER JOIN ParityCases
                                ON ParityCases.CaseID = ParityAlgs.CaseID
                                INNER JOIN Pieces BF
                                ON BF.PieceID = ParityCases.BufferPiece
                                INNER JOIN Pieces LT
                                ON LT.PieceID= ParityCases.LastTarget
                                INNER JOIN Images
                                ON Images.LetterPair = LT.Letter || LT.Letter
                                WHERE LetterPair = '{pair}';";
                                alg = connection.Query<AlgModel>(command).ToList().First();
                                output.Add(alg);
                            }
                            //If normal corner algorithm
                            else
                            {
                                string command = $@"SELECT CornerAlgs.AlgID AS ID, BF.Piece AS BufferPiece, FT.Piece AS FirstTarget, ST.Piece AS SecondTarget, CornerAlgs.Alg AS Alg
                                FROM CornerAlgs
                                INNER JOIN CornerCases
                                ON CornerAlgs.CaseID = CornerCases.CaseID
                                INNER JOIN Pieces BF
                                ON BF.PieceID = CornerCases.BufferPiece
                                INNER JOIN Pieces FT
                                ON FT.PieceID = CornerCases.FirstTarget
                                INNER JOIN Pieces ST
                                ON ST.PieceID = CornerCases.SecondTarget
                                INNER JOIN Images
                                ON Images.LetterPair = FT.Letter || ST.Letter
                                WHERE LetterPair = '{pair}';";
                                alg = connection.Query<AlgModel>(command).ToList().First();
                                output.Add(alg);
                            }


                        }
                        
                    }
                }
                return output;
            }
        }
        /// <summary>
        /// Method to prevent retrieving DBNulls on strings
        /// </summary>
        /// <param name="reader">SQLite reader currently using</param>
        /// <param name="colIndex">Column index</param>
        /// <returns></returns>
        public static string SafeGetString(this SQLiteDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetString(colIndex);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Method to prevent retrieving DBNulls on integers
        /// </summary>
        /// <param name="reader">SQLite reader currently using</param>
        /// <param name="colIndex">Column index</param>
        /// <returns></returns>
        public static int SafeGetInt(this SQLiteDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetInt32(colIndex);
            }
            else
            {
                return 0;
            }
        }
    }
}
