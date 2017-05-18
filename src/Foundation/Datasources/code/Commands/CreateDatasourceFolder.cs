using JCore.Foundation.Datasources.Models;
using JCore.Foundation.Datasources.Repositories;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCore.Foundation.Datasources.Commands
{
    [Serializable]
    public class CreateDatasourceFolder : ProgressedCommand
    {
        private readonly IDatasourceRepository _datasourceRepository;

        public CreateDatasourceFolder()
        {
            this.CommandName = "CreateDataSourceFolder";
            this.CommandMessage = "Creating Data Source Folder";
            _datasourceRepository = new DatasourceRepository();
        }

        protected override void StartProcess(object[] parameters)
        {
            var item = (Item)parameters[1];
            _datasourceRepository.CreateItemDatasource(item);
        }

        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context.Items.Length != 1)
                return CommandState.Disabled;
            if (!string.IsNullOrWhiteSpace(context.Items[0][NonChildDatasourceSupport.DatasourceFolderFieldId]))
                return CommandState.Disabled;
            return base.QueryState(context);
        }
    }
}
