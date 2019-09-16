using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class LandmarkXticket
    {
        public int LandmarkDetailId { get; set; }
        public int? LandmarkId { get; set; }
        public int? TicketId { get; set; }

        public virtual Landmark Landmark { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
