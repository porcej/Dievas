using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Festis {

   	/// <summary>
    ///     Class <c>StaffingNode</c> Staffing information as generrated by <see href="https://github.com/porcej/festis">Festis</see>
    /// </summary>
	public class StaffingNode {

		/// <summary>
		/// 	Node Title
		/// </summary>
		[JsonProperty("title")]
		public string Title { get; set; }

		/// <summary>
		/// 	Notes associated with this node
		/// </summary>
		[JsonProperty("notes")]
		public string Notes { get; set; }

		/// <summary>
		/// 	Notes associated with this level
		/// </summary>
		[JsonProperty("isSurrpressed")]
		public bool IsSurrpressed { get; set; }
    }
}