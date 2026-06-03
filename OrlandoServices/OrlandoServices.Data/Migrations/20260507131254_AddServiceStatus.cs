using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrlandoServices.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // שלב 1: מוסיפים את עמודת Status עם ברירת מחדל 1 (Active)
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Service",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            // שלב 2: ממירים נתונים — IsActive=false הופך ל-Hidden (3)
            migrationBuilder.Sql(@"UPDATE ""Service"" SET ""Status"" = 3 WHERE ""IsActive"" = false");

            // שלב 3: מוחקים את העמודה הישנה אחרי שהנתונים הועברו
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Service");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // שלב 1: מוסיפים בחזרה את IsActive
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Service",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            // שלב 2: ממירים — Active(1) ו-Unavailable(2) → IsActive=true, Hidden(3) → IsActive=false
            migrationBuilder.Sql(@"UPDATE ""Service"" SET ""IsActive"" = (""Status"" != 3)");

            // שלב 3: מוחקים את Status
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Service");
        }
    }
}
