using System;

namespace Backend.Models {
    
    public class Comment {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string CommentText { get; set; }
        public string CautionNotePriority { get; set; }
        public int SequenceNumber { get; set; }
        public int CommentCategoryID { get; set; }

        public override bool Equals(Object obj) {
            return (obj is Comment) && ((Comment)obj).Id == Id;
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}