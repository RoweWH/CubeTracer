namespace CubeTypes
{
    public class Piece
    {
        /// <summary>
        /// This string represents the stickers on this piece.
        /// For edges: always length 2, first character representing sticker on top or bottom.
        /// If edge is in the middle layer, the first character represents the sticker on front or back
        /// For corners: always length 3, first character representing sticker on top or bottom.
        /// The other two characters represent the other two stickers read in a clockwise direction.
        /// </summary>
        public string Stickers { get; set; }
        /// <summary>
        /// This boolean keeps track of whether or not this piece has been traced already.
        /// </summary>
        public bool Hit { get; set; }
        /// <summary>
        /// This boolean keeps track of whether or not this piece is in a solved position but oriented incorrectly.
        /// </summary>
        public bool FlipTwist { get; set; }
        /// <summary>
        /// Constructor, initializes Piece to default values
        /// </summary>
        public Piece()
        {
            Hit = false;
            FlipTwist = false;
            Stickers = "";
        }
        /// <summary>
        /// Virtual method to be overwritten by derived piece types.
        /// </summary>
        /// <param name="direction"></param>
        public virtual void orient(int direction) { }
    }

  
}