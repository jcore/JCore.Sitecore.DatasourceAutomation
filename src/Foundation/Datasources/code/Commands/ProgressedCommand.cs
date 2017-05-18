using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCore.Foundation.Datasources.Commands
{
    /// <summary>
    /// Defines the base class for all Content Editor commands that launch with progress.
    ///             This class inherits from the Command class.
    /// 
    /// </summary>
    [Serializable]
    public abstract class ProgressedCommand : Command
    {
        /// <summary>
        /// The progress box.
        /// 
        /// </summary>
        private ProgressBoxWrapper _progressBox;

        /// <summary>
        /// Gets or sets the name of the command to be shown in the progress box during the synchronization process.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The name of the command
        /// 
        /// </value>
        public string CommandName { get; set; }

        /// <summary>
        /// Gets or sets the command message
        ///             to show within progress box during synchronization.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The command message.
        /// 
        /// </value>
        public string CommandMessage { get; set; }

        /// <summary>
        /// Gets or sets the progress box wrapper.
        ///             It wraps the Execute static method of the
        ///             <see cref="T:Sitecore.Shell.Applications.Dialogs.ProgressBoxes.ProgressBox"/>
        ///             and can be unit-tested.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The progress box
        /// 
        /// </value>
        internal ProgressBoxWrapper ProgressBox
        {
            get
            {
                return _progressBox ?? (_progressBox = new ProgressBoxWrapper());
            }
            set
            {
                Assert.ArgumentNotNull((object)value, "value");
                _progressBox = value;
            }
        }

        /// <summary>
        /// Executes the command that is specified in the Content Editor context.
        /// 
        /// </summary>
        /// <param name="context">The command context</param>
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            object[] processParameters = this.GetProcessParameters(context);
            Assert.IsNotNullOrEmpty(this.CommandName, "CommandName");
            Assert.IsNotNullOrEmpty(this.CommandMessage, "CommandMessage");
            string message = "item:load(id=" + (object)context.Items[0].ID + ")";
            this.ProgressBox.Execute(this.CommandName, this.CommandMessage, string.Empty, new ProgressBoxMethod(this.StartProcess), message, processParameters);
        }

        /// <summary>
        /// Gets the process parameters.
        /// 
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>
        /// a collection of the parameters
        /// </returns>
        protected virtual object[] GetProcessParameters(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            Assert.AreEqual(context.Items.Length, 1, "Collection of context items should have the only element.");
            return new object[2]
            {
                context.Items[0].Language.Name,
                context.Items[0]
            };
        }

        /// <summary>
        /// Starts the process.
        /// 
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        protected abstract void StartProcess(object[] parameters);
    }
}
