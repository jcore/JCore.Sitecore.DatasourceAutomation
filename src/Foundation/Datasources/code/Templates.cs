using Sitecore.Configuration;
using Sitecore.Data;

namespace JCore.Foundation.Datasources
{
    public struct Templates
    {
        public struct NonChildDatasourceSupport
        {
            public static readonly ID ID = new ID("{070CF52C-196F-449A-8FB4-D359695FF0AE}");

            public struct Fields
            {
                public static readonly ID DatasourceFolderFieldId = new ID("{43533608-7316-4255-9217-09F755C51DC6}");
                public static readonly string DatasourceFolderFieldName = "Datasource Folder";
            }
        }

        public struct DatasourceFolderBranch
        {
            public static readonly ID ID = ID.Parse(Settings.GetSetting("JCore.Foundation.Datasources.ParentDatasourceFolderBranchId", "{BC0F702C-6124-429D-A861-FDB7AC8F6E0C}"));
        }

        public struct DatasourceFolderTemplate
        {
            public static readonly ID ID = ID.Parse(Settings.GetSetting("JCore.Foundation.Datasources.ParentDatasourceFolderTemplateId", "{A8502920-D146-4545-83D0-CE9FB24046DB}"));
        }

        public struct DatasourceSubfolderTemplate
        {
            public static readonly ID ID = ID.Parse(Settings.GetSetting("JCore.Foundation.Datasources.ChildDatasourceFolderTemplateId", "{AC919CC0-6C40-456D-BA6E-23B64231118F}"));
        }

        public struct BranchTemplate
        {
            public static readonly ID ID = new ID("{35E75C72-4985-4E09-88C3-0EAC6CD1E64F}");
        }

        public struct TemplateTemplate
        {
            public static readonly ID ID = new ID("{070CF52C-196F-449A-8FB4-D359695FF0AE}");
        }

        public struct Rendering
        {
            public struct Fields
            {
                public static readonly string DatasourceTemplateFieldName = "Datasource Template";
            }
        }
    }
}