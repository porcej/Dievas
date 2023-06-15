using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dievas.Models.Messages {

    /// <summary>
    ///     Class <c>MessageType</c> a description of where a message wil be displayed
    /// </summary>
    public class MessageType {
        
        /// <summary>
        ///     Primary Key
        /// </summary>
        public int MessageTypeId { get; set; }

        /// <summary>
        ///     Message Type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public MessageType(int messageTypeId, string name) {
            MessageTypeId = messageTypeId;
            Name = name;
        }

        /// <summary>
        ///     Copies properties
        /// </summary>
        /// <param name="mt">MessageType: Object to copy params</param>c# 64 b
        public void copy(MessageType mt) {
            Name = mt.Name;
        }
    }
}