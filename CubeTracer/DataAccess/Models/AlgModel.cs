using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CubeTracer.DataAccess.Models
{
    public class AlgModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// BufferPiece for the algorithm
        /// </summary>
        public string BufferPiece { get; set; }
        /// <summary>
        /// The algorithm's First Target piece
        /// </summary>
        public string FirstTarget { get; set; }
        /// <summary>
        /// The algorithm's Second Target piece
        /// </summary>
        public string SecondTarget { get; set; }
        /// <summary>
        /// The algorithm itself
        /// </summary>
        public string Alg { get; set; }
        /// <summary>
        /// Display names for each type of algorithm.
        /// </summary>
        public string DisplayName
        {
            get
            {
                //Flip or Twist algorithm of type "Other"
                if(BufferPiece == null && FirstTarget == null && SecondTarget == null)
                {
                    return $"Flip/Twist:   {Alg}";
                }
                //Parity algorithm
                else if(SecondTarget == null)
                {
                    return $"{BufferPiece}-{FirstTarget} Parity:   {Alg}";
                }
                else
                //Normal BufferPiece-FirstTarget-SecondTarget algorithm
                {
                    return $"{BufferPiece}-{FirstTarget}-{SecondTarget}:   {Alg}";
                }
            }
        }
    }
}
