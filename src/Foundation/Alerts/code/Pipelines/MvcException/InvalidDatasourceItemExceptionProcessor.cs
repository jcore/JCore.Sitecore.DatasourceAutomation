using System.Web.Mvc;
using Car.Foundation.Alerts.Exceptions;
using Car.Foundation.Alerts.Models;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.MvcEvents.Exception;

namespace Car.Foundation.Alerts.Pipelines.MvcException
{
    public class InvalidDatasourceItemExceptionProcessor
  {
    public void Process(ExceptionArgs exceptionArgs)
    {
      if (exceptionArgs.ExceptionContext.ExceptionHandled)
      {
        return;
      }

      this.HandleException(exceptionArgs.ExceptionContext);
    }

    protected virtual void HandleException(ExceptionContext exceptionContext)
    {
      var dataSourceException = exceptionContext.Exception as InvalidDataSourceItemException;
      if (dataSourceException == null)
        return;
      Log.Error(dataSourceException.Message, dataSourceException, this);

      if (Context.PageMode.IsNormal)
      {
        exceptionContext.Result = new EmptyResult();
      }
      else
      {
        var viewData = new ViewDataDictionary
                       {
                         Model = InfoMessage.Error(AlertTexts.InvalidDataSource)
                       };
        exceptionContext.Result = new ViewResult
                                  {
                                    ViewName = Constants.InfoMessageView,
                                    ViewData = viewData
                                  };
      }

      exceptionContext.ExceptionHandled = true;
    }
  }
}