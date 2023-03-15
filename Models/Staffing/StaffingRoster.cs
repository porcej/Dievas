using System;
using System.Collections.Generic;
using Dievas.Models.Telestaff;
using Newtonsoft.Json;

namespace Dievas.Models.Staffing {

   	/// <summary>
    ///     Class <c>StaffingRoster</c> Staffing roster for dashboarding purposes
    /// </summary>
	public class StaffingRoster {


	    /// <summary>
	    /// 	Constructor
	    ///	</summary>
	    public StaffingRoster(DateTime rosterDate, List<StaffingRecord> records) {
			RosterDate = rosterDate;
			Records = records;
	    }

	    /// <summary>
	    /// 	Date for this roster information
	    ///	</summary>
	    [JsonProperty("rosterDate")]
	    public DateTime RosterDate { get; set; }

	    /// <summary>
	    /// 	List of Staffing Records
	    ///	</summary>
	    [JsonProperty("records")]
	    public List<StaffingRecord> Records { get; set; }
    }
}
