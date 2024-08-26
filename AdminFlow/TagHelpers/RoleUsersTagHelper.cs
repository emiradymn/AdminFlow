using Microsoft.AspNetCore.Razor.TagHelpers;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RoleUsersTagHelper : TagHelper
{
    [HtmlAttributeName("asp-role-users")]
    public string RoleId { get; set; } = null!;

    private readonly string _connectionString;

    public RoleUsersTagHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var userNames = new List<string>();

        // MySQL bağlantısını manuel olarak açıyoruz
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Role'ü al
            var roleQuery = "SELECT Name FROM AspNetRoles WHERE Id = @RoleId";
            using (var roleCommand = new MySqlCommand(roleQuery, connection))
            {
                roleCommand.Parameters.AddWithValue("@RoleId", RoleId);
                var roleName = await roleCommand.ExecuteScalarAsync() as string;

                if (roleName != null)
                {
                    // Kullanıcıları al
                    var userQuery = @"
                        SELECT u.UserName 
                        FROM AspNetUsers u
                        JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                        JOIN AspNetRoles r ON ur.RoleId = r.Id
                        WHERE r.Name = @RoleName";

                    using (var userCommand = new MySqlCommand(userQuery, connection))
                    {
                        userCommand.Parameters.AddWithValue("@RoleName", roleName);

                        using (var reader = await userCommand.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                userNames.Add(reader["UserName"].ToString() ?? "");
                            }
                        }
                    }
                }
            }

            // Bağlantıyı kapatıyoruz
            await connection.CloseAsync();
        }

        // Sonuçları çıktı olarak ayarla
        output.Content.SetContent(userNames.Count == 0 ? "kullanıcı yok" : string.Join(",", userNames));
    }



    private string SetHtml(List<string> userNames)
    {
        var html = "<ul>";

        foreach (var item in userNames)
        {
            html += $"<li>{item}</li>";
        }

        html += "</ul>";
        return html;
    }
}

