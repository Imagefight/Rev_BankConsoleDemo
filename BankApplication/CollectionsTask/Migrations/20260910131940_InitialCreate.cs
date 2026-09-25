using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectionsTask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssociatedID = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "Decimal(19,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    AccountKind = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IsOverdraftEnabled = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    OverdraftLimit = table.Column<decimal>(type: "Decimal(19,2)", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_Accounts_Logins_AssociatedID",
                        column: x => x.AssociatedID,
                        principalTable: "Logins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AssociatedID",
                table: "Accounts",
                column: "AssociatedID");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_Username",
                table: "Logins",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Logins");
        }
    }
}
