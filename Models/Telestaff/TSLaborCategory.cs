using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>TSLaborCategory</c> Telestaff representation of a labor category
    /// </summary>
	public class TSLaborCategory {

		/// <summary>
		/// 	
		/// </summary>
		[JsonProperty("laborCategoryName")]
		public string LaborCategoryName { get; set; }

		/// <summary>
		/// 	
		/// </summary>
		[JsonProperty("laborCategoryEntry")]
		public string LaborCategoryEntry { get; set; }
    }
}
