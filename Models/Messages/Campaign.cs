using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dievas.Models.Messages {

   	/// <summary>
    ///     Class <c>Campaign</c> A grouping of internal messages
    /// </summary>
	public class Campaign {
		
		/// <summary>
		/// 	Primary Key
		/// </summary>
		public int CampaignId { get; set; }

		/// <summary>
		/// 	Campaign Title
		/// </summary>
		public string Title { get; set; }

        /// <summary>
        ///     Active flag - only displays messages if active
        /// </summary>
        public bool Active { get; set; } = false;

        /// <summary>
        ///     Start Time
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        ///     End Time
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        ///     Included Messages
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        ///     Copies properties
        /// </summary>
        /// <param name="c">Campaign: Object to copy params</param>c# 64 b
        public void copy(Campaign c) {
            Title = c.Title;
            Active = c.Active;
            StartDateTime = c.StartDateTime;
            EndDateTime = c.EndDateTime;
            Messages = c.Messages;
        }
    }
}