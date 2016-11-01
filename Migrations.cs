using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Contrib.FeedMix
{
    using Models;
    using Orchard.Core.Common.Models;
    using Orchard.Widgets.Models;

    public class FeedsDataMigration : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable("FeedMixPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Title", t => t.NotNull())
                    .Column<string>("Description")
                    .Column<string>("Path")
                );

            SchemaBuilder.CreateTable("FeedPartRecord", table => table
               .ContentPartRecord()
               .Column<int>("FeedMixPartRecord_Id")
               .Column<string>("WebsiteUrl")
               .Column<string>("FeedUrl")
               .Column<string>("Title")
               .Column<string>("Author")
              );

            SchemaBuilder.CreateTable("FeedMixWidgetPartRecord", table => table
           .ContentPartRecord()
           .Column<int>("FeedMixPartRecord_Id")
          );

            ContentDefinitionManager.AlterTypeDefinition("FeedMix", alt => alt.Creatable(false).WithPart("FeedMixPart"));
            ContentDefinitionManager.AlterTypeDefinition("Feed", alt => alt.Creatable(false).WithPart("FeedPart"));

            ContentDefinitionManager.AlterTypeDefinition(
              "FeedMixWidget", cfg => cfg
                .WithSetting("Stereotype", "Widget")
                .WithPart(typeof(FeedMixWidgetPart).Name)
                .WithPart(typeof(CommonPart).Name)
                .WithPart(typeof(WidgetPart).Name));

            return 1;
        }
    }
}