using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trace.Dto
{
    public class RegisterDto:UserDto
    {
		private string newPassWord;

		public string NewPassWord
		{
			get { return newPassWord; }
			set { newPassWord = value; OnPropertyChanged(); }
		}

	}
}
