using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetMuseumApi;
using System.Net;

namespace MetMuseumApiTests
{
	[TestClass]
	public class MetMuseumApiTests
	{
		[TestMethod]
		public void WebMethodGetsResponseTest()
		{
			var api = new MetApi();
			Assert.IsTrue(api.GetHtml().Length > 0);
		}

		[TestMethod]
		public void WebMethodGetsCorrectResponseTest()
		{
			var api = new MetApi();

			var resp = api.GetHtml();
			Assert.IsTrue(resp.IndexOf("search-results-container") > 0);
		}

		[TestMethod]
		public void WebMethodGetPageCountTest()
		{
			var api = new MetApi();

			Assert.IsTrue(api.PageCount > 0);
		}

		[TestMethod]
		public void GetRandomPageTest()
		{
			var api = new MetApi();
			
			api.GetRandomPage();

			Assert.IsTrue(api.GetHtml().IndexOf("search-results-container") > 0);
		}

		[TestMethod]
		public void GetNumberOfPaintingsOnPageTest()
		{
			var api = new MetApi();
			
			api.GetRandomPage();

			Assert.IsTrue(api.GetPaintingsOnPage() > 0);
		}

		[TestMethod]
		public void GetRandomPaintingUrlTest()
		{
			var api = new MetApi();
			
			var url = api.GetPaintingPage();

			Assert.IsTrue(url.IndexOf("pos=") > 0);
		}

		[TestMethod]
		public void PaintingHtmlReturnedProperlyTest()
		{
			var api = new MetApi();
			
			api.GetPaintingPage();

			Assert.IsTrue(api.GetPaintingHtml().IndexOf("object-information") > 0);
		}

		[TestMethod]
		public void PaintingImageUrlReturnsProperlyTest()
		{
			var api = new MetApi();
			
			api.GetPaintingPage();
			var painting = api.GetPaintingInfo();

			Assert.IsTrue(painting.Url.IndexOf("web-large") > 0);
		}
	}
}
