using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidoeMVC.Migrations
{
    /// <inheritdoc />
    public partial class userfoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilPhotoURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilPhotoURL",
                table: "AspNetUsers");
        }
    }
}
