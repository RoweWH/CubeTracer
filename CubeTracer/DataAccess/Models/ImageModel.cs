using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeTracer.DataAccess.Models
{
    public class ImageModel
    {
        /// <summary>
        /// stores ID for the image
        /// </summary>
        public int ImageID { get; set; }
        /// <summary>
        /// Stores the letter pair associated with the image
        /// </summary>
        public string LetterPair { get; set; }
        /// <summary>
        /// The image itself
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// Display name for the form
        /// </summary>
        public string DisplayName
        {
            get
            {
                return $"{LetterPair}:   {Img}";
            }
        }
        
    }
    
}
