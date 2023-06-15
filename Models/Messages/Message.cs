using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dievas.Models.Messages {

   	/// <summary>
    ///     Class <c>Message</c> An internal message to display
    /// </summary>
	public class Message {
		
		/// <summary>
		/// 	Primary Key
		/// </summary>
		public int MessageId { get; set; }

		/// <summary>
		/// 	Message Title
		/// </summary>
		public string Title { get; set; }

        /// <summary>
        ///     Message Type
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        ///     Message Type foreign key
        /// </summary>
        public int MessageTypeId { get; set; }

        /// <summary>
        ///     Approved flag - only displays message if active
        /// </summary>
        public bool Approved { get; set; } = false;

        /// <summary>
        ///     Active flag - only displays message if active
        /// </summary>
        public bool Active { get; set; } = false;

        /// <summary>
        ///     Emergent flag - if true displays skips message que
        /// </summary>
        public bool Emergent { get; set; } = false;

        /// <summary>
        ///     Message Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Message Campaign
        /// </summary>
        public List<Campaign> Campaigns { get; set; } = new();

        /// <summary>
        ///     Copies properties
        /// </summary>
        /// <param name="mt">Message: Object to copy params</param>c# 64 b
        public void copy(Message msg) {
            Title = msg.Title;
            MessageTypeId = msg.MessageTypeId;
            MessageType = msg.MessageType;
            Approved = msg.Approved;
            Active = msg.Active;
            Emergent = msg.Emergent;
            Content = msg.Content;
            Campaigns = msg.Campaigns;
        }
    }
}