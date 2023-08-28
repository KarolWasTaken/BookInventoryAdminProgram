using BookInventoryAdminProgram.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Stores
{
    public class UserInfoStore
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        /// <summary>
        /// Stores Fistname and Secondname as properties inside the UserInfoStore class so that anyone with the same instance
        /// of this class can access these properties.
        /// </summary>
        /// <param name="UserNames">Result from SQL query that returns first and second name</param>
        public void StoreUserName(UserInfoStore UserNames) 
        {
            FirstName = UserNames.FirstName;
            SecondName = UserNames.SecondName;
        }
    }
}
