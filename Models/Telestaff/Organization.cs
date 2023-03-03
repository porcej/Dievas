using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>Organization</c> Telestaff bucket to hold organizational subgroups
    /// </summary>
	public class Organization {

		/// <summary>
		/// 	Agency details
		/// </summary>
		[JsonProperty("agency")]
		public ScheduleOrganizationNode Agency { get; set; }

		/// <summary>
		/// 	Institution details
		/// </summary>
		[JsonProperty("institution")]
		public ScheduleOrganizationNode Institution { get; set; }

		/// <summary>
		/// 	Physical Unit details
		/// </summary>
		[JsonProperty("physicalUnit")]
		public ScheduleOrganizationNode PhysicalUnit { get; set; }

		/// <summary>
		/// 	Position details
		/// </summary>
		[JsonProperty("position")]
		public ScheduleOrganizationNode Position { get; set; }

		/// <summary>
		/// 	Region details
		/// </summary>
		[JsonProperty("region")]
		public ScheduleOrganizationNode Region { get; set; }

		/// <summary>
		/// 	Station details
		/// </summary>
		[JsonProperty("station")]
		public ScheduleOrganizationNode Station { get; set; }

		/// <summary>
		/// 	Unit details
		/// </summary>
		[JsonProperty("unit")]
		public ScheduleOrganizationNode Unit { get; set; }

    }
}
