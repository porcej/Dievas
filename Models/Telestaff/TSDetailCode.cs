using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>TSDetailCode</c> Telestaff representation of a cost center
    /// </summary>
	public class TSDetailCode {

		/// <summary>
		/// 	Detail abbreviation
		/// </summary>
		[JsonProperty("abrv")]
		public string Abrv { get; set; }

		/// <summary>
		/// 	Detail ID
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Detail Name
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
    }
}
