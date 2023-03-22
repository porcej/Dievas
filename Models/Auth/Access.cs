using System.Collections.Generic;
using Dievas.Models.Auth;

namespace Dievas.Models.Auth
{
    // We are not taking data from data base so we get data from constant
    public class Access
    {
        public static List<UserModel> Users = new() {
            new UserModel(){ Username="joseph.porcelli", Roles= new List<string> {"admin"}}
        };
    }
}