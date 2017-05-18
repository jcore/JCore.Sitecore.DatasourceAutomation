using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCore.Foundation.Datasources.Commands
{
    [Serializable]
    internal class ProgressBoxWrapper
    {
        public virtual void Execute(string jobName, string title, string icon, ProgressBoxMethod method, string message, params object[] parameters)
        {
            ProgressBox.Execute(jobName, title, string.Empty, method, message, parameters);
        }
    }
}
