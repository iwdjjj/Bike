using Microsoft.EntityFrameworkCore;

namespace Bike.Models
{
    [Keyless]
    public class Route_CountOtchet
    {
        public int? id { get; set; }
        public string? nm { get; set; }
        public int? kol { get; set; }
    }
}
