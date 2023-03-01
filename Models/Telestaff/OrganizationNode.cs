using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>OrganizationNode</c> Telestaff representation of an organizational unit
    /// </summary>
	public class OrganizationNode {

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
		/// 	Applicable for Station and Unit
		/// </summary>
		[JsonProperty("effectiveDate")]
		public DateTime? EffectiveDate { get; set; }

		/// <summary>
		/// 	Organization Status
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }

		/// <summary>
		/// 	Should be unique for each node and type combination.
		/// </summary>
		[JsonProperty("externalId")]
		public string ExternalId { get; set; }

		/// <summary>
		/// 	Organization hierarchy path
		/// </summary>
		[JsonProperty("hierarchyPath")]
		public string? HierarchyPath { get; set; }

		/// <summary>
		/// 	Organization's institution details
		/// </summary>
		[JsonProperty("institution")]
		public IdAndExternalIdType? Institution { get; set; }

		/// <summary>
		/// 	Name of the Organization
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// 	Organization Id
		/// </summary>
		[JsonProperty("organizationId")]
		public int? OrganizationId { get; set; }

		/// <summary>
		/// 	Applicable for Agency, Region, Station and Unit
		/// </summary>
		[JsonProperty("parent")]
		public ParentType? Parent { get; set; }

		/// <summary>
		/// 	Applicable for Station and Unit
		/// </summary>
		[JsonProperty("throughDate")]
		public DateTime? ThroughDate { get; set; }

		/// <summary>
		/// 	Type of the Organization = ['INSTITUTION/AGENCY/REGION/STATION/UNIT/POSITION']
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }

		/// <summary>
		/// 	The time zone associated with the target Organization.
		/// </summary>
		[JsonProperty("zoneId")]
		public string? ZoneId { get; set; }
    }
}
