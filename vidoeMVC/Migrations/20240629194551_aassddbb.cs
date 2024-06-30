using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidoeMVC.Migrations
{
    /// <inheritdoc />
    public partial class aassddbb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FBlink",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instlink",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Xlink",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FBlink",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Instlink",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Xlink",
                table: "AspNetUsers");
        }
    }
}
