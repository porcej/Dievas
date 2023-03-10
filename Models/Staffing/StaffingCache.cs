using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Staffing {

   	/// <summary>
    ///     Class <c>StaffingCache</c> Caches Staffing Data
    /// </summary>
	public class StaffingCache {


		public StaffingCache(StaffingRoster roster, double ExpirationTimeInMinutes) {
			Roster = roster;
			Expiration = DateTime.Now.AddMinutes(ExpirationTimeInMinutes);
		}

		/// <summary>
		/// 	Data Contained In API Response
		/// </summary>
		public StaffingRoster Roster { get; set; }

		/// <summary>
		/// 	Expiration
		/// </summary>
		public DateTime Expiration { get; set; }

		/// <summary>
		/// 	Is Valid
		/// </summary>
		/// <returns> Retursn true, iff the current time is prior to this record's expiration date and time.  Returns false otherwise</returns>
		public bool IsValid() {
			return (DateTime.Compare(DateTime.Now, Expiration) < 0 );
		}

		/// <summary>
		/// 	Is Expired
		/// </summary>
		/// <returns> Retursn true, iff the current time is equal to or after this record's expiration date and time.  Returns false otherwise</returns>
		public bool IsExpired() {
			return (DateTime.Compare(DateTime.Now, Expiration) >= 0 );
		}
    }
}
