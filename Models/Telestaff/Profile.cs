using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>Profile</c> Telestaff representation of a profile
    /// </summary>
	public class Profile {

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
		/// 	external system id of the profile
		/// </summary>
		[JsonProperty("externalId")]
		public string ExternalId { get; set; }

		/// <summary>
		/// 	Formula Id assigned to profile
		/// </summary>
		[JsonProperty("formulaId")]
		public FormulaId FormulaId { get; set; }

		/// <summary>
		/// 	Internal identifier of entity
		/// </summary>
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 	Name of entity
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// 	Specialities assigned to profile
		/// </summary>
		[JsonProperty("specialities")]
		public string Specialities { get; set; }
    }
}
