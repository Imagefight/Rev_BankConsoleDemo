using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectionsTask.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckbook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Logins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Checkbooks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkbooks", x => new { x.AccountNumber, x.ID });
                    table.ForeignKey(
                        name: "FK_Checkbooks_Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkbooks");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Logins");
        }
    }
}
