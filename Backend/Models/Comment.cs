using System;

namespace Backend.Models {
    
    public class Comment {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public string commentText { get; set; }
        public string cautionNotePriority { get; set; }
        public int sequenceNumber { get; set; }
        public int commentCategoryID { get; set; }
    }
}