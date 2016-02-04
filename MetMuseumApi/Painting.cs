using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetMuseumApi
{
	public class Painting
	{
		public int Id { get; set; }
		public string Url { get; set; }
		public string PageUrl { get; set; }
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Date { get; set; }
		public string Medium { get; set; }
		public string Dimensions { get; set; }
		public string Classification { get; set; }
		public string CreditLine { get; set; }
		public string AccessionNumber { get; set; }

		[JsonIgnore]		
		public string Json { get; set; }
	}
}
