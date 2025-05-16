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
        
        Alter.Table("Users")
            .AddColumn("Username").AsString(50).NotNullable().WithDefaultValue("")
            .AddColumn("Email").AsString(100).NotNullable().WithDefaultValue("")
            .AddColumn("PasswordHash").AsString(100).NotNullable().WithDefaultValue("")
            .AddColumn("CreatedAt").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .AddColumn("LastLoginAt").AsDateTime().Nullable();
        
        Create.UniqueConstraint("UQ_Users_Username")
            .OnTable("Users")
            .Column("Username");

        Create.UniqueConstraint("UQ_Users_Email")
            .OnTable("Users")
            .Column("Email");

        Create.Table("ChatContexts")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("MessageId").AsInt64().NotNullable()
            .WithColumn("MessageText").AsString().Nullable()
            .WithColumn("MessageDate").AsDateTime().NotNullable();

        Create.Table("Recollections")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("DownloadLink").AsString().NotNullable();

        Create.ForeignKey()
            .FromTable("Recollections").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table("Attachments")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("FileId").AsInt64().NotNullable()
            .WithColumn("OwnerId").AsInt64().NotNullable()
            .WithColumn("OriginalLink").AsString().Nullable()
            .WithColumn("ContextId").AsInt64().NotNullable();

        Create.ForeignKey()
            .FromTable("Attachments").ForeignColumn("ContextId")
            .ToTable("ChatContexts").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("Attachments");
        Delete.Table("Recollections");
        Delete.Table("ChatContexts");
        Delete.UniqueConstraint("UQ_Users_Username").FromTable("Users");
        Delete.UniqueConstraint("UQ_Users_Email").FromTable("Users");
        Delete.Table("Users");
    }
}
