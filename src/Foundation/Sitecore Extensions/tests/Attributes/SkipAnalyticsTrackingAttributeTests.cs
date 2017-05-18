﻿using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using JCore.Foundation.SitecoreExtensions.Attributes;
using FluentAssertions;
using NSubstitute;
using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Xunit;

namespace JCore.Foundation.SitecoreExtensions.Tests.Attributes
{
    public class SkipAnalyticsTrackingAttributeTests
  {
    [Theory]
    [AutoDbMvcData]
    public void OnActionExecuting_OnAjaxRequest_ShouldCallCancelAnalytics(SkipAnalyticsTrackingAttribute trackingAttribute, ActionExecutingContext ctx, ITracker tracker)
    {
      //arrange
      InitializeActionFilterContext(ctx);
      tracker.IsActive.Returns(true);

      using (new TrackerSwitcher(tracker))
      {
        //act
        trackingAttribute.OnActionExecuting(ctx);
        //assert
        tracker.CurrentPage.Received(1).Cancel();
      }
     
    }


    [Theory]
    [AutoDbMvcData]
    public void OnActionExecuting_TrackerNotInitialized_ShouldDoNothing(SkipAnalyticsTrackingAttribute trackingAttribute, ActionExecutingContext ctx, ITracker tracker)
    {
      //arrange
      InitializeActionFilterContext(ctx);

      using (new TrackerSwitcher(tracker))
      {
        //act
        trackingAttribute.OnActionExecuting(ctx);
        //assert
        tracker.CurrentPage.DidNotReceive().Cancel();
      }
    }


    [Theory]
    [AutoDbMvcData]
    public void OnActionExecuting_CurrentPageIsNull_ShouldNotRaiseException(SkipAnalyticsTrackingAttribute trackingAttribute, ActionExecutingContext ctx, ITracker tracker)
    {
      //arrange
      InitializeActionFilterContext(ctx);
      tracker.IsActive.Returns(true);
      tracker.CurrentPage.Returns((ICurrentPageContext)null);


      using (new TrackerSwitcher(tracker))
      {
        //act
        Action action = () => trackingAttribute.OnActionExecuting(ctx);
        //assert
        action.ShouldNotThrow();
      }
    }


    private static void InitializeActionFilterContext(ActionExecutingContext ctx)
    {
      ctx.RequestContext.HttpContext.Request.Headers.Returns(new NameValueCollection());
      ctx.RequestContext.HttpContext.Request.Headers.Add("X-Requested-With", "XMLHttpRequest");

    }
  }
}