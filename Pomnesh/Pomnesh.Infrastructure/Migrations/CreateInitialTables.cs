using FluentMigrator;

namespace Pomnesh.Infrastructure.Migrations;

[Migration(29032025)]
public class CreateInitialTables : Migration
{
    public override void Up()
    {
        // Please find creating schema `public` with running migration
        
        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("VkId").AsInt64().NotNullable()
            .WithColumn("VkToken").AsString().Nullable();

        Create.Table("ChatContexts")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("MessageId").AsInt64().NotNullable()
            .WithColumn("MessageText").AsString().Nullable()
            .WithColumn("MessageDate").AsDateTime().NotNullable();

        Create.Table("Recollections")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Users", "Id")
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("DownloadLink").AsString().NotNullable();

        Create.Table("Attachments")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("FileId").AsInt64().NotNullable()
            .WithColumn("OwnerId").AsInt64().NotNullable()
            .WithColumn("OriginalLink").AsString().Nullable()
            .WithColumn("ContextId").AsInt64().NotNullable().ForeignKey("ChatContexts", "Id");
    }

    public override void Down()
    {
        Delete.Table("Attachments");
        Delete.Table("Recollections");
        Delete.Table("ChatContexts");
        Delete.Table("Users");
    }
}