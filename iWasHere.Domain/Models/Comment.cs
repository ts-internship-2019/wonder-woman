using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class Comment
    {
        public Comment() { }
        public int CommentId { get; set; }
        public int? LandmarkId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int RatingValue { get; set; }
        public string OwnerName { get; set; }
        public string UserId { get; set; }
        public DateTime? SubmitedDate { get; set; }

        public virtual Landmark Landmark { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
