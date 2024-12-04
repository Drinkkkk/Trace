using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trace.Common.Models;

namespace Trace.Models
{
    public class TruckDto:BaseDto
    {
		//经度
		private string longitude;

		public string Longitude
        {
			get { return longitude; }
			set { longitude = value; }
		}
		//纬度
		private string latitude;

		public string Latitude
        {
			get { return latitude; }
			set { latitude = value; }
		}
		//车牌
		private string license;		

		public string License
        {
			get { return license; }
			set { license = value; }
		}
		//车型
		private string model;

		public string Model
        {
			get { return model; }
			set { model = value; }
		}
		//状态
		private string  status;

		public string  Status
		{
			get { return status; }
			set { status = value; }
		}

		private string  content;

		public string  Content
		{
			get { return content; }
			set { content = value; }
		}
		private string title;

		public string Title
		{
			get { return title; }
			set { title = value; }
		}

	}
}
