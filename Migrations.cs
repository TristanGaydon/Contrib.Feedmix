using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace DeftIndustries.FeedMix {
    public class FeedsDataMigration : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable("FeedMixPartRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<string>("Title", t => t.NotNull())
                    .Column<string>("Description")
                    .Column<string>("Path")
                );

            SchemaBuilder.CreateTable("FeedPartRecord", t => t
           .ContentPartRecord()
           .Column<int>("FeedMixPartRecord_Id")
           .Column<string>("WebsiteUrl")
           .Column<string>("FeedUrl")
           .Column<string>("Title")
           .Column<string>("Author")
          );

           ContentDefinitionManager.AlterTypeDefinition("FeedMix",alt => alt.Creatable(false).WithPart("FeedMixPart"));
           ContentDefinitionManager.AlterTypeDefinition("Feed", alt => alt.Creatable(false).WithPart("FeedPart"));

            return 1;
        }
    }
}