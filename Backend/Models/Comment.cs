using System;

namespace Backend.Models {
    
    public class Comment {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public string commentText { get; set; }
        public string cautionNotePriority { get; set; }
        public int sequenceNumber { get; set; }
        public int commentCategoryID { get; set; }

        public override bool Equals(Object obj) {
            return (obj is Comment) && ((Comment)obj).id == id;
        }

        public override int GetHashCode() {
            return id;
        }
    }
}