using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>NameAndId</c> Telestaff tool to add additional information to a staffing record for display on a Roster
    /// </summary>
	public class NameAndId {

		/// <summary>
		/// 	Internal identifier of entity
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Name of the entity
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
    }
}
