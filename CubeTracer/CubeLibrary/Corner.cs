using CubeTypes;

public class Corner : Piece
{
    /// <summary>
    /// Constructor, populates Stickers with s, initializes boolean fields to false
    /// </summary>
    /// <param name="s"></param>
    public Corner(string s)
    {
        Hit = false;
        FlipTwist = false;
        Stickers = s;
    }
    /// <summary>
    /// Orientation function specific to Corner Types
    /// If direction == 1, the Stickers are cycled to represent a clockwise twist
    /// If direction == -1, the Stickers are cycled to represent a counter-clockwise twist
    /// </summary>
    /// <param name="direction"></param>
    public override void orient(int direction)
    {
        string newString = "";
        if (direction == 1)
        {
            newString += Stickers[2];
            newString += Stickers[0];
            newString += Stickers[1];
            Stickers = newString;
        }
        else if (direction == -1)
        {
            newString += Stickers[1];
            newString += Stickers[2];
            newString += Stickers[0];
            Stickers = newString;
        }
    }
}