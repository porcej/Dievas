using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models {

    public class AssignedUnit {
        public string RadioName { get; set; }
        public int StatusId { get; set; }

        public override bool Equals(Object obj) {
            return (obj is AssignedUnit) && ((AssignedUnit)obj).RadioName == RadioName;
        }
 
        public override int GetHashCode() {
            return RadioName.GetHashCode();
        }
    }
}