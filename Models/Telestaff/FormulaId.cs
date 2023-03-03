using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models.Telestaff {

   	/// <summary>
    ///     Class <c>FormulaId</c> Telestaff representation of a formula ID
    /// </summary>
	public class FormulaId {

		/// <summary>
		/// 	Formula id expression
		/// </summary>
		[JsonProperty("expression")]
		public string Expression { get; set; }

		/// <summary>
		/// 	Formula id evaluated value
		/// </summary>
		[JsonProperty("value")]
		public string Value { get; set; }
    }
}
