using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>IdAndExternalIdType</c> Telestaff representation of an ID Field
    /// </summary>
	public class IdAndExternalIdType {

		/// <summary>
		/// 	Should be unique for each node and type combination.
		/// </summary>
		[JsonProperty("externalId")]
		public string ExternalId { get; set; }

		/// <summary>
		/// 	Organization Id
		/// </summary>
		[JsonProperty("Id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Name of the Organization
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
    }
}
