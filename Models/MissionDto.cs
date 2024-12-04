using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common.Models;

namespace Trace.Models
{
    public class MissionDto:BaseDto
    {
		private string title;

		public string Title
		{
			get { return title; }
			set { title = value; }
		}
		private string content;

		public string Content
		{
			get { return content; }
			set { content = value; }
		}
		private string status;

		public string Status
		{
			get { return status; }
			set { status = value; }
		}

	}
}
