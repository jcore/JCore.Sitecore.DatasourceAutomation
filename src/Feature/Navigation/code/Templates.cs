using Sitecore.Data;

namespace Car.Feature.Navigation
{
    public struct Templates
    {
        public struct Link
        {
            public static readonly ID ID = new ID("{A16B74E9-01B8-439C-B44E-42B3FB2EE14B}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{FE71C30E-F07D-4052-8594-C3028CD76E1F}");
            }
        }

        public struct Navigable
        {
            public static readonly ID ID = new ID("{0AF6D020-1967-453B-9E05-11947603F423}");

            public struct Fields
            {
                public static readonly ID NavigationTitle = new ID("{1F5B8FC6-7441-478A-9426-A247D0D06971}");
                public static readonly ID ShowInNavigation = new ID("{04F82419-1706-4F2A-8E62-22294A0A9221}");
                public static readonly ID ShowChildren = new ID("{CAA02A19-2FE7-4295-B63D-EA7A989E6BE9}");
                public static readonly ID DataAttribute = new ID("{9E03E1EF-7EB8-41B2-B8BD-8BEF8966A7B8}");
            }
        }

        public struct NavigationRoot
        {
            public static readonly ID ID = new ID("{A3E60CCA-C234-43BB-B44A-BDC1A25EC887}");
        }

        public struct SectionLink
        {
            public static readonly ID ID = new ID("{ACD0F708-3B5A-417A-B000-27BF4AE14AB9}");

            public struct Fields
            {
                public static readonly ID Section = new ID("{8230EE45-6DA2-4538-AE12-8D02502C77E2}");
                public static readonly ID ClassAttribute = new ID("{B6528A32-8165-4FB0-BA60-64811B0400E5}");
            }
        }
    }
}