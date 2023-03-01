using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>CostCenter</c> Telestaff representation of a Cost Center
    /// </summary>
	public class CostCenter {

		/// <summary>
		/// 	Name of cost center. This is generally derived from Account field. ,
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// 	Reference of cost center
		/// </summary>
		[JsonProperty("ref")]
		public string? Ref { get; set; }
    }
}
