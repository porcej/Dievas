using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>JobTitle</c> Telestaff representation of a Job Title
	public class JobTitle {

		/// <summary>
		/// 	Abbreviation of the Job Title
		/// </summary>
		[JsonProperty("abbreviation")]
		public string? Abbreviation { get; set; }

		/// <summary>
		/// 	Job Title Status
		/// </summary>
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		/// <summary>
		/// 	Should be unique for each node and type combination.
		/// </summary>
		[JsonProperty("externalId")]
		public string ExternalId { get; set; }

		/// <summary>
		/// 	Organization hierarchy path
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Name of the Job Title
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
    }
}
