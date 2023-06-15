using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Dievas.Models.Settings;

namespace Dievas.Models.Settings {
   	/// <summary>
    ///     Class <c>SystemSetting</c> Application specific settings
    /// </summary>
	public class SystemSetting {
		
		/// <summary>
		/// 	Setting Primary Key
		/// </summary>
		public int SystemSettingId { get; set; }

		/// <summary>
		/// 	Setting name
		/// </summary>
		public string Field { get; set; }

		/// <summary>
		/// 	Setting Value
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// 	Setting type - used to generated input field 
		/// </summary>
		public SettingType SettingType { get; set; }

		/// <summary>
		/// 	Setting Type Foriegn Key
		/// </summary>
		public int SettingTypeId { get; set; }

		/// <summary>
        ///     Copies properties
        /// </summary>
        /// <param name="SystemSetting">SystemSetting: Setting of which to copy</param>
        public void copy(SystemSetting setting) {
            Field = setting.Field;
            Value = setting.Value;
            SettingTypeId = setting.SettingTypeId;
            SettingType = setting.SettingType;
        }
    }
}
