using MetMuseumApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebTesting
{
	public partial class _default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var api = new MetApi();
			api.GetPaintingPage();

			var painting = api.GetPaintingInfo();
			image.ImageUrl = painting.Url;
			title.Text = painting.Title;
			link.NavigateUrl = painting.PageUrl;
		}
	}
}