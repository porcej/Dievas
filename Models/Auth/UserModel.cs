using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Dievas.Models.Auth;

namespace Dievas.Models.Auth {

    /// <summary>
    ///     Class <c>UserModel</c> Model for a user of this application
    /// </summary>
    [PrimaryKey(nameof(Username))]
    public class UserModel {

        /// <summary>
        ///     Username for user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Roles assigned to user, seperated by ";"
        /// </summary>
        // public string Roles { get; set; }
        public List<string> Roles { get; set; }

        /// <summary>
        ///     Adds a list of roles
        /// </summary>
        /// <param name="newRoles">List\<string\> list of roles</param>
        public void AddRolesFromList(List<string> newRoles) {
            Console.WriteLine(newRoles);
        }
            
        /// <summary>
        ///     Adds a single role
        /// </summary>
        /// <param name="newRole">string: role to add</param>
        public void AddRole(string newRole) {
            Roles.Add(newRole);
        }

        /// <summary>
        ///     Copies properties
        /// </summary>
        /// <param name="model">UserModel: Model of which to copy</param>
        public void copy(UserModel model) {
            Username = model.Username;
            Roles = model.Roles;
        }

        /// <summary>
        ///     Checks for equality
        /// </summary>
        /// <returns> Returns true, iff the compared to a UserModel object with the same username.  Returns false otherwise</returns>
        public override bool Equals(Object obj) {
            return (obj is UserModel) && ((UserModel)obj).Username == Username;
        }

        /// <summary>
        ///     Returns has code for this object
        /// </summary>
        /// <returns> Hash code for this object</returns>
        public override int GetHashCode() {
            return Username.GetHashCode();
        }
    }
}