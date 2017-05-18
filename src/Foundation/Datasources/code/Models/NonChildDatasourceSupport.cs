using Sitecore.Data;

namespace JCore.Foundation.Datasources.Models
{
    public partial class NonChildDatasourceSupport : INonChildDatasourceSupport
    {
        public static ID DatasourceFolderFieldId = ID.Parse("{43533608-7316-4255-9217-09F755C51DC6}");
        public const string DatasourceFolderFieldName = "Datasource Folder";
        public static ID SitecoreTemplateId = ID.Parse("{070CF52C-196F-449A-8FB4-D359695FF0AE}");
    }
}
