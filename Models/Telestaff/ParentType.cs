using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>ParentType</c> Telestaff representation of a Parent Organizational Node
    /// </summary>
	public class ParentType {

		/// <summary>
		/// 	Stores parent external id of organization and up to 40 alphanumeric characters ,
		/// </summary>
		[JsonProperty("parentExternalId")]
		public string ParentExternalId { get; set; }

		/// <summary>
		/// 	Organization Id of parent
		/// </summary>
		[JsonProperty("parentId")]
		public int ParentId { get; set; }

		/// <summary>
		/// 	Organization type of the parent = ['INSTITUTION/AGENCY/REGION/STATION/UNIT']
		/// </summary>
		[JsonProperty("parentType")]
		public string OrgParentType { get; set; }
    }
}
