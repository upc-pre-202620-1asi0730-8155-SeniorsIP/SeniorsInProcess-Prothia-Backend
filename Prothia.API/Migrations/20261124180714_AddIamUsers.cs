using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

#nullable disable

namespace Prothia.API.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20261124180714_AddIamUsers")]
public partial class AddIamUsers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                password_hash = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                role = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
            },
            constraints: table => table.PrimaryKey("p_k_users", x => x.id))
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "i_x_users_username",
            table: "users",
            column: "username",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "users");
    }
}
