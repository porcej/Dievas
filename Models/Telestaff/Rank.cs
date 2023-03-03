using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>Rank</c> Telestaff rank details
    /// </summary>
	public class Rank {

		/// <summary>
		/// 	Abbreviation of entity
		/// </summary>
		[JsonProperty("abbreviation")]
		public string Abbreviation { get; set; }

		/// <summary>
		/// 	entity is disabled or not
		/// </summary>
		[JsonProperty("disabled")]
		public bool Disabled { get; set; }

		/// <summary>
		/// 	External Id of Rank
		/// </summary>
		[JsonProperty("externalId")]
		public string ExternalId { get; set; }

		/// <summary>
		/// 	Internal identifier o
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }
    }
}
