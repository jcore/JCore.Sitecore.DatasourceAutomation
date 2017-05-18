using System.Web.Mvc;
using System.Web.Mvc.Html;
using Car.Foundation.Alerts.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Mvc;

namespace Car.Foundation.Alerts.Extensions
{
    public static class AlertHtmlHelpers
  {
    public static MvcHtmlString PageEditorError(this HtmlHelper helper, string errorMessage)
    {
      Log.Error($@"Presentation error on '{helper.Sitecore()?.CurrentRendering?.RenderingItemPath}': {errorMessage}", typeof(AlertHtmlHelpers));

      return Context.PageMode.IsNormal ? new MvcHtmlString(string.Empty) : helper.Partial(Constants.InfoMessageView, InfoMessage.Error(errorMessage));
    }

    public static MvcHtmlString PageEditorInfo(this HtmlHelper helper, string infoMessage)
    {
      return Context.PageMode.IsNormal ? new MvcHtmlString(string.Empty) : helper.Partial(Constants.InfoMessageView, InfoMessage.Info(infoMessage));
    }

    public static MvcHtmlString PageEditorError(this HtmlHelper helper, string errorMessage, string friendlyMessage, ID contextItemId, ID renderingId)
    {
      Log.Error($@"Presentation error: {errorMessage}, Context item ID: {contextItemId}, Rendering ID: {renderingId}", typeof(AlertHtmlHelpers));

      return Context.PageMode.IsNormal ? new MvcHtmlString(string.Empty) : helper.Partial(Constants.InfoMessageView, InfoMessage.Error(friendlyMessage));
    }
  }
}