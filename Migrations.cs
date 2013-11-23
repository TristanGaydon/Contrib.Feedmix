using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace DeftIndustries.FeedMix {
    public class FeedsDataMigration : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable("FeedPartRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<string>("Title", t => t.NotNull())
                    .Column<string>("About")
                    .Column<string>("URL")
                );

            SchemaBuilder.CreateTable("FeedRecord", t => t
           .Column<int>("Id", column => column.PrimaryKey().Identity())
           .Column<int>("FeedPartRecord_Id")
           .Column<string>("URL")
           .Column<string>("Title")
           .Column<string>("Author")
          );

            ContentDefinitionManager.AlterTypeDefinition("Feed",alt => alt.WithPart("FeedPart"));

            return 1;
        }
    }
}