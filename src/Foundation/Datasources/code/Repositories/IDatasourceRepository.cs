using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;

namespace JCore.Foundation.Datasources.Repositories
{
    public interface IDatasourceRepository
    {
        void CreateItemDatasource(Item item);
        void DeleteItemDatasource(Item item);
        void MoveItemDatasource(Item item);
        void RenameItemDatasource(Item item);
    }
}
