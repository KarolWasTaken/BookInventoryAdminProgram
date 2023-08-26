using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInventoryAdminProgram.Stores
{
	/// <summary>
	/// Stores userdata of user. Called when loggin in.
	/// Its important that the public properties have the same exect name as the properties in SQL
	/// </summary>
    public class CurrentUserInfomation
    {
		private string _firstName;
		public string Firstname
		{
			get { return _firstName; }
			set { _firstName = value; }
		}
		private string _secondName;
		public string Secondname
		{
			get { return _secondName; }
			set { _secondName = value; }
		}

	}
}
