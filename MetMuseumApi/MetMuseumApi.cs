using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetMuseumApi
{
	public class MetApi
	{
		private string BaseUrl { get; set; }
		private string SearchUrl { get; set; }
		private string CurrentHtml { get; set; }
		private string PaintingHtml { get; set; }
		private string PaintingUrl { get; set; }
		private string PaintingBaseUrl { get; set; }
		public int PageCount { get; set; }

		public MetApi() {
			BaseUrl = "http://www.metmuseum.org/";
			PaintingBaseUrl = BaseUrl + "collection/the-collection-online/search/#id#?rpp=30";
			SearchUrl = BaseUrl + "collection/the-collection-online/search?deptids=2&pg=1";
			GetWebRequest(SearchUrl);
			GetPages();
		}

		public void GetWebRequest(string url, string type = "S") {
			var request = WebRequest.Create(url).GetResponse();
			var dataStream = request.GetResponseStream ();
      var reader = new StreamReader (dataStream);
      var responseFromServer = reader.ReadToEnd ();
      reader.Close();
			
			if(type == "S") {
				CurrentHtml = responseFromServer;
			} else {
				PaintingHtml = responseFromServer;
			}
		}
		
		public string GetHtml()
		{
			return CurrentHtml;
		}

		public string GetPaintingHtml()
		{
			return PaintingHtml;
		}

		public void GetPages()
		{
			try {
				var containerStart = CurrentHtml.IndexOf("phcontent_0_phfullwidthcontent_0_paginationWidget_rptPagination_paginationLineItem_6");
				var numberStart = CurrentHtml.IndexOf("pg=", containerStart) + 3;
				var numberEnd = CurrentHtml.IndexOf("<", numberStart);
			
				var number = CurrentHtml.Substring(numberStart, (numberEnd - numberStart) - 2);
				PageCount = Convert.ToInt32(number) - 1;
			} catch(Exception ex) {
				PageCount = 0;
			}
		}

		public void GetRandomPage() {
			var random = new Random(Guid.NewGuid().GetHashCode());
			var pageNum = random.Next(1, PageCount);

			GetWebRequest("http://www.metmuseum.org/collection/the-collection-online/search?deptids=2&pg=" + pageNum);
			
			var resp = GetHtml();
		}

		public string GetPaintingPage(int? id = null) {
			var url = "";
			if(id == null) {
				GetRandomPage();
				var random = new Random();
				var picNum = random.Next(1, GetPaintingsOnPage());
				url = GetPaintingUrlFromResults(picNum);
			} else {
				url = GetPaintingById(id);
			}
			GetWebRequest(url, "P");
			if(PaintingHtml.IndexOf("No image is available at this time.") >= 0) {
				url = GetPaintingPage();
			}
			return url;
		}

		private string GetPaintingById(int? id)
		{
			var url = PaintingBaseUrl.Replace("#id#", id.ToString());
			PaintingUrl = url;
			return url;
		}

		public Painting GetPaintingInfo(int? id = null) {
			GetPaintingPage(id);
			var pageHtml = GetPaintingHtml();
			var painting = PopulateInfo(pageHtml);
			painting.Json = JsonConvert.SerializeObject(painting);
			return painting;
		}

		public string GetPaintingInfoJson(int? id = null) {	
			return JsonConvert.SerializeObject(GetPaintingInfo(id));
		}

		public int GetPaintingsOnPage() {
			var selector = "list-view-object";
			int count = new Regex(Regex.Escape(selector)).Matches(GetHtml()).Count;
			return count;
		}

		public string GetPaintingUrlFromResults(int? picNum) {
			var startPos = GetHtml().IndexOf("list-view-thumbnail");
			for(var x = 1; x <= picNum; x++) {
				startPos = GetHtml().IndexOf("list-view-thumbnail", startPos);
			}
			startPos = GetHtml().IndexOf("href", startPos) + 6;
			var endPos = GetHtml().IndexOf(" ", startPos) - 1;
			var url = BaseUrl + (GetHtml().Substring(startPos, endPos - startPos));
			PaintingUrl = url;
			return url;
		}
		
		public Painting PopulateInfo(string pageHtml) {
			var startPosUrl = pageHtml.IndexOf("src=", pageHtml.IndexOf("inner-image-container")) + 5;
			var endPosUrl = pageHtml.IndexOf('"', startPosUrl + 3);
			var url = pageHtml.Substring(startPosUrl, endPosUrl - startPosUrl);

			var startPosTitle = pageHtml.IndexOf("h2", pageHtml.IndexOf("tombstone-container")) + 3;
			var endPosTitle = pageHtml.IndexOf("<", startPosTitle);
			var title = pageHtml.Substring(startPosTitle, endPosTitle - startPosTitle);
			
			var startPosId = PaintingUrl.LastIndexOf("/") + 1;
			var endPosId = PaintingUrl.IndexOf("?", startPosId);
			var Id = Convert.ToInt32(PaintingUrl.Substring(startPosId, endPosId - startPosId));

			var artist = GetPaintingAttribute("Artist", pageHtml);
			var date = GetPaintingAttribute("Date", pageHtml);
			var medium = GetPaintingAttribute("Medium", pageHtml);
			var dimensions = GetPaintingAttribute("Dimensions", pageHtml);
			var classification = GetPaintingAttribute("Classification", pageHtml);
			var creditLine = GetPaintingAttribute("Credit Line", pageHtml);
			var accessionNumber = GetPaintingAttribute("Accession Number", pageHtml);
			
			return new Painting {
				Id = Id,
				Url = url,
				Title = title,
				Artist = artist,
				Date = date,
				Medium = medium,
				Dimensions = dimensions,
				Classification = classification,
				CreditLine = creditLine,
				AccessionNumber = accessionNumber,
				PageUrl = PaintingUrl
			};
		}

		private string GetPaintingAttribute(string attributeName, string pageHtml)
		{
			attributeName = "<strong>" + attributeName + ":</strong>";
			var attributePos = pageHtml.IndexOf(attributeName) + attributeName.Length;
			var endPos = pageHtml.IndexOf("<", attributePos + 1);
			var value = pageHtml.Substring(attributePos, endPos - attributePos);
			value = value.Replace("\n", "").Replace("\r", "").Trim();

			return value;
		}
	}
}
