using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinook.Data.POCOs
{
    public class GenreAlbumReport
    {
        public string GenreName { get; set; }

        public string AlbumTitle { get; set; }

        public string TrackName { get; set; }
            
        public int Milliseconds { get; set; }

        public int? Bytes { get; set; }

        public decimal UnitPrice { get; set; }
    }
}//eoc

