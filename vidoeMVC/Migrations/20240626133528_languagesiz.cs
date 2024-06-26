using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidoeMVC.Migrations
{
    /// <inheritdoc />
    public partial class languagesiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoCategory_Categories_CategoryId",
                table: "VideoCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoCategory_Videos_VideoId",
                table: "VideoCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoTag_Tags_TagId",
                table: "VideoTag");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoTag_Videos_VideoId",
                table: "VideoTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoTag",
                table: "VideoTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoCategory",
                table: "VideoCategory");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Privacy",
                table: "Videos");

            migrationBuilder.RenameTable(
                name: "VideoTag",
                newName: "VideoTags");

            migrationBuilder.RenameTable(
                name: "VideoCategory",
                newName: "VideoCategories");

            migrationBuilder.RenameIndex(
                name: "IX_VideoTag_TagId",
                table: "VideoTags",
                newName: "IX_VideoTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoCategory_CategoryId",
                table: "VideoCategories",
                newName: "IX_VideoCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoTags",
                table: "VideoTags",
                columns: new[] { "VideoId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoCategories",
                table: "VideoCategories",
                columns: new[] { "VideoId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCategories_Categories_CategoryId",
                table: "VideoCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCategories_Videos_VideoId",
                table: "VideoCategories",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoTags_Tags_TagId",
                table: "VideoTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoTags_Videos_VideoId",
                table: "VideoTags",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoCategories_Categories_CategoryId",
                table: "VideoCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoCategories_Videos_VideoId",
                table: "VideoCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoTags_Tags_TagId",
                table: "VideoTags");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoTags_Videos_VideoId",
                table: "VideoTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoTags",
                table: "VideoTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoCategories",
                table: "VideoCategories");

            migrationBuilder.RenameTable(
                name: "VideoTags",
                newName: "VideoTag");

            migrationBuilder.RenameTable(
                name: "VideoCategories",
                newName: "VideoCategory");

            migrationBuilder.RenameIndex(
                name: "IX_VideoTags_TagId",
                table: "VideoTag",
                newName: "IX_VideoTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoCategories_CategoryId",
                table: "VideoCategory",
                newName: "IX_VideoCategory_CategoryId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Videos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Privacy",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoTag",
                table: "VideoTag",
                columns: new[] { "VideoId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoCategory",
                table: "VideoCategory",
                columns: new[] { "VideoId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCategory_Categories_CategoryId",
                table: "VideoCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoCategory_Videos_VideoId",
                table: "VideoCategory",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoTag_Tags_TagId",
                table: "VideoTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoTag_Videos_VideoId",
                table: "VideoTag",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
