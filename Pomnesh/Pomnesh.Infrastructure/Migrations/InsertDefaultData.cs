using FluentMigrator;

namespace Pomnesh.Infrastructure.Migrations;

[Migration(29032025_1)]
public class InsertDefaultData : Migration
{
    public override void Up()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123");
        Insert.IntoTable("Users").Row(new { VkId = 123456789, VkToken = "default_token", Email="vsky@mail.ru", PasswordHash = hashedPassword, Username="vsky_admin" });
        Insert.IntoTable("ChatContexts").Row(new 
        {
            MessageId = 1,
            MessageText = "Welcome to Pomnesh!",
            MessageDate = System.DateTime.UtcNow
        });
        
        Insert.IntoTable("Recollections").Row(new 
        {
            UserId = 1,
            CreatedAt = System.DateTime.UtcNow,
            DownloadLink = "https://example.com/default_recollection"
        });
        
        Insert.IntoTable("Attachments").Row(new 
        {
            Type = 1,
            FileId = 1001,
            OwnerId = 1,
            OriginalLink = "https://example.com/default_attachment",
            ContextId = 1
        });
    }

    public override void Down()
    {
        Delete.FromTable("Attachments").AllRows();
        Delete.FromTable("Recollections").AllRows();
        Delete.FromTable("ChatContexts").AllRows();
    }
}