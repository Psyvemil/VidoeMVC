using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidoeMVC.Migrations
{
    /// <inheritdoc />
    public partial class _12111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryVideo");

            migrationBuilder.DropTable(
                name: "TagVideo");

            migrationBuilder.AlterColumn<string>(
                name: "Privacy",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Videos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VideoCategory",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoCategory", x => new { x.VideoId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_VideoCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoCategory_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoTag",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoTag", x => new { x.VideoId, x.TagId });
                    table.ForeignKey(
                        name: "FK_VideoTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoTag_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoCategory_CategoryId",
                table: "VideoCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoTag_TagId",
                table: "VideoTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoCategory");

            migrationBuilder.DropTable(
                name: "VideoTag");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "Privacy",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryVideo",
                columns: table => new
                {
                    VCategoriesId = table.Column<int>(type: "int", nullable: false),
                    VideosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryVideo", x => new { x.VCategoriesId, x.VideosId });
                    table.ForeignKey(
                        name: "FK_CategoryVideo_Categories_VCategoriesId",
                        column: x => x.VCategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryVideo_Videos_VideosId",
                        column: x => x.VideosId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagVideo",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    VideosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagVideo", x => new { x.TagsId, x.VideosId });
                    table.ForeignKey(
                        name: "FK_TagVideo_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagVideo_Videos_VideosId",
                        column: x => x.VideosId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryVideo_VideosId",
                table: "CategoryVideo",
                column: "VideosId");

            migrationBuilder.CreateIndex(
                name: "IX_TagVideo_VideosId",
                table: "TagVideo",
                column: "VideosId");
        }
    }
}
