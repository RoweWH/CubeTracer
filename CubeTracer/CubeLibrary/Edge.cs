using CubeTypes;

public class Edge : Piece
{
    /// <summary>
    /// Constructor, populates Stickers with s, initializes boolean fields to false
    /// </summary>
    /// <param name="s"></param>
    public Edge(string s)
    {
        Hit = false;
        FlipTwist = false;
        Stickers = s;
    }
    /// <summary>
    /// Orientation method specific to Edge types
    /// Simply reverses the Stickers string to represent the piece being flipped.
    /// </summary>
    /// <param name="direction"></param>
    public override void orient(int direction)
    {
        if (direction == 1)
        {
            string newString = "";
            newString += Stickers[1];
            newString += Stickers[0];
            Stickers = newString;
        }
     }
}