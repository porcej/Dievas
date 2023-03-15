using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dievas.Models {

   	/// <summary>
    ///     Class <c>ApiWrapper</c> Generic Wrapper for API Data
    /// </summary>
	public class ApiWrapper {

		public ApiWrapper (){
			StatusCode = 200;
		}

		/// <summary>
		/// 	Data Contained In API Response
		/// </summary>
		[JsonProperty("data")]
		public Object Data { get; set; }

		/// <summary>
		/// 	Status Code 
		/// </summary>
		[JsonProperty("status_code")]
		public int StatusCode { get; set; }
    }
}
