using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeTracer.DataAccess.Models
{
    public class ReconstructionModel
    {
        /// <summary>
        /// List of Edge Images for the reconstruction
        /// </summary>
        public List<ImageModel> EdgeImages { get; set; }
        /// <summary>
        /// List of Corner Images for the reconstruction
        /// </summary>
        public List<ImageModel> CornerImages { get; set; }
        /// <summary>
        /// List of EdgeAlgs for the reconstruction
        /// </summary>
        public List<AlgModel> EdgeAlgs { get; set; }
        /// <summary>
        /// List of Corner Algs for the reconstruction
        /// </summary>
        public List<AlgModel> CornerAlgs { get; set; }
        


    }
}
