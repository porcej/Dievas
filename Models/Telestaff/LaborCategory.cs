using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>LaborCategory</c> Telestaff representation of a Detail Labor Category
    /// </summary>
	public class LaborCategory {

		/// <summary>
		/// 	Labor Category value ,
		/// </summary>
		[JsonProperty("laborCategoryEntry")]
		public string LaborCategoryEntry { get; set; }

		/// <summary>
		/// 	Labor Category name
		/// </summary>
		[JsonProperty("laborCategoryName")]
		public string LaborCategoryName { get; set; }
    }
}
