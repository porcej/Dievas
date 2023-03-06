using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingPosition</c> Staffing information for a specific position generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingPosition: StaffingNode {

		/// <summary>
		/// 	Badge number associated with person filling position
		/// </summary>
		[JsonProperty("Badge")]
		public string Badge { get; set; }

		/// <summary>
		/// 	Duration of shift in hours
		/// </summary>
		[JsonProperty("Duration")]
		public string Duration { get; set; }

		/// <summary>
		/// 	Position ID
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// 	Name of person filling this spot
		/// </summary>
		[JsonProperty("name")]
	    public string Name { get; set; }

	    /// <summary>
		/// 	Specialties associated with the person filling this spot
		/// </summary>
		[JsonProperty("specialties")]
	    public string Specialties { get; set; }

	    /// <summary>
		/// 	Workcodes associated with this instance of this position
		/// </summary>
		[JsonProperty("workcode")]
	    public string Workcode { get; set; }

	    /// <summary>
	    /// 	Exception codes associated with this position
	    /// </summary>
	    [JsonProperty("exceptioncode")]
	    public string Exceptioncode { get; set; }

	    /// <summary>
	    /// 	true is this is a request that has not been approved
	    /// </summary>
	    [JsonProperty("isRequest")]
	    public bool IsRequest { get; set; }

	    /// <summary>
	    /// 	Time at the start of this record
	    /// 	<example>"03/06/2023 07:00 AM"</example>
	    /// </summary>
	    [JsonProperty("startTime")]
	    public string StartTime { get; set; }

	    /// <summary>
	    /// 	Time at the end of this record
	    /// 	<example>"03/06/2023 07:00 AM"</example>
	    /// </summary>
	    [JsonProperty("endTime")]
	    public string EndTime { get; set; }

	    /// <summary>
	    /// 	true is this is a working vs standby code
	    /// </summary>
	    [JsonProperty("isWorking")]
	    public bool IsWorking { get; set; }

	    /// <summary>
	    /// 	true is this is position is an assigned position
	    /// </summary>
	    [JsonProperty("isAssigned")]
	    public bool IsAssigned { get; set; }

	    /// <summary>
	    /// 	true is this position has not been filled
	    /// </summary>
	    [JsonProperty("isVacant")]
	    public bool IsVacant { get; set; }
    }
}
