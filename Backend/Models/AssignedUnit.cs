using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models {

    public class AssignedUnit {
        public string radioName { get; set; }
        public int statusId { get; set; }

        public override bool Equals(Object obj) {
            return (obj is AssignedUnit) && ((AssignedUnit)obj).radioName == radioName;
        }
 
        public override int GetHashCode() {
            return radioName.GetHashCode();
        }
    }
}