using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dievas.Models.Settings {

   	/// <summary>
    ///     Class <c>SettingsType</c> Type of value stored in this setting
    /// </summary>
	public class SettingType {
		
		/// <summary>
		/// 	Setting Primary Key
		/// </summary>
		public int SettingTypeId { get; set; }

		/// <summary>
		/// 	Setting name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Setting type - used to generated input field 
		/// </summary>
		public string InputControlType { get; set; }

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public SettingType(int settingTypeId, string name, string inputControlType="text") {
            SettingTypeId = settingTypeId;
            Name = name;
            InputControlType = inputControlType;
        }

        /// <summary>
        ///     Copies properties
        /// </summary>
        /// <param name="st">SettingType: Setting type of which to copy</param>
        public void copy(SettingType st) {
            Name = st.Name;
            InputControlType = st.InputControlType;
        }
    }
}