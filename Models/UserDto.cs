using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common.Models;

namespace Trace.Models
{
    public class UserDto:BaseDto
    {
		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private string power;

		public string Power
		{
			get { return power; }
			set { power = value; }
		}

		private string account;

		public string Account
		{
			get { return account; }
			set { account = value; }
		}
		//头像
		private string profilePicture;

		public string ProfilePicture
        {
			get { return profilePicture; }
			set { profilePicture = value; }
		}

	}
}
