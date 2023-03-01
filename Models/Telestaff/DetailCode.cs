using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>DetailCode</c> Telestaff representation of a Detail Staffing Code
    /// </summary>
	public class DetailCode {

		/// <summary>
		/// 	Detail code abbreviation ,
		/// </summary>
		[JsonProperty("abrv")]
		public string? Abrv { get; set; }

		/// <summary>
		/// 	Detail code id ,
		/// </summary>
		[JsonProperty("id")]
		public int? Id { get; set; }

		/// <summary>
		/// 	Detail code name
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
    }
}
