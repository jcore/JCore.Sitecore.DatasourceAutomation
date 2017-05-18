using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using JCore.Foundation.Datasources.Repositories;

namespace JCore.Foundation.Datasources.Events
{
    public class ItemEventHandler
    {
        private readonly IDatasourceRepository _datasourceRepository = new DatasourceRepository();

        /// <summary>
        ///     Gets or sets the database.
        /// </summary>
        /// <value>
        ///     The database.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        ///     Called when [item added].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void OnItemAdded(object sender, EventArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (Context.Job != null) return;

            var item = Event.ExtractParameter(args, 0) as Item;
            if (item == null)
            {
                return;
            }
            if (item.Database != null && string.CompareOrdinal(item.Database.Name, Database) != 0)
                return;

            _datasourceRepository.CreateItemDatasource(item);
        }

        protected void OnItemDeleted(object sender, EventArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (Context.Job != null) return;

            var item = Event.ExtractParameter(args, 0) as Item;
            if (item == null)
            {
                return;
            }
            if (item.Database != null && string.CompareOrdinal(item.Database.Name, Database) != 0)
                return;

            _datasourceRepository.DeleteItemDatasource(item);
        }

        protected void OnItemMoved(object sender, EventArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (Context.Job != null) return;

            var item = Event.ExtractParameter(args, 0) as Item;
            if (item == null)
            {
                return;
            }
            if (item.Database != null && string.CompareOrdinal(item.Database.Name, Database) != 0)
                return;

            _datasourceRepository.MoveItemDatasource(item);
        }

        protected void OnItemRenamed(object sender, EventArgs args)
        {
            if (args == null)
            {
                return;
            }

            if (Context.Job != null) return;

            var item = Event.ExtractParameter(args, 0) as Item;
            if (item == null)
            {
                return;
            }
            if (item.Database != null && string.CompareOrdinal(item.Database.Name, Database) != 0)
                return;

            _datasourceRepository.RenameItemDatasource(item);
        }
    }
}
