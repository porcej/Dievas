using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>ScheduleOrganizationNode</c> Telestaff representation of an organizational unit within a schedule
    /// </summary>
	public class ScheduleOrganizationNode {

		/// <summary>
		/// 	Abbreviation of the Organization
		/// </summary>
		[JsonProperty("abbreviation")]
		public string? Abbreviation { get; set; }

		/// <summary>
		/// 	Stores up to 30 alphanumeric characters for an Organization Account Code. Field used in Payroll Export when required.
		/// </summary>
		[JsonProperty("account")]
		public string? Account { get; set; }

		/// <summary>
		/// 	False, if entity is unassigned. This is true for off roster scenarios
		/// </summary>
		[JsonProperty("assigned")]
		public bool? Assigned { get; set; }

		/// <summary>
		/// 	Attachments for the entity for that calender day
		/// </summary>
		[JsonProperty("attachments")]
		public List<string>? Attachments { get; set; }

		/// <summary>
		/// 	entity is disabled or not
		/// </summary>
		[JsonProperty("disabled")]
		public bool? Disabled { get; set; }

		/// <summary>
		/// 	Display Name of entity
		/// </summary>
		[JsonProperty("displayName")]
		public string? DisplayName { get; set; }

		/// <summary>
		/// 	Should be unique for each node and type combination.
		/// </summary>
		[JsonProperty("externalId")]
		public string ExternalId { get; set; }

		/// <summary>
		/// 	Your quess is as good as mine - this may
		/// </summary>
		[JsonProperty("formulaId")]
		public FormulaId? FormulaId { get; set; }

		/// <summary>
		/// 	Internal identifier of entity
		/// </summary>
		[JsonProperty("id")]
		public int? Id { get; set; }

		/// <summary>
		/// 	Name of the entity
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// 	Position rank details
		/// </summary>
		[JsonProperty("rank")]
		public Rank? Rank { get; set; }
    }
}
