using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>TSOrganization</c> Telestaff representation of an organization
    /// </summary>
	public class TSCostCenter {

		/// <summary>
		/// 	Cost center name
		/// </summary>
		[JsonProperty("name")]
		public string name { get; set; }

		/// <summary>
		/// 	Cost center reference string
		/// </summary>
		[JsonProperty("ref")]
		public string Ref { get; set; }
    }
}