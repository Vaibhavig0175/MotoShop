using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProfile_AspNetUsers_UserId",
                table: "SellerProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerProfile",
                table: "SellerProfile");

            migrationBuilder.RenameTable(
                name: "SellerProfile",
                newName: "SellerProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProfile_UserId",
                table: "SellerProfiles",
                newName: "IX_SellerProfiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerProfiles",
                table: "SellerProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProfiles_AspNetUsers_UserId",
                table: "SellerProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerProfiles_AspNetUsers_UserId",
                table: "SellerProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerProfiles",
                table: "SellerProfiles");

            migrationBuilder.RenameTable(
                name: "SellerProfiles",
                newName: "SellerProfile");

            migrationBuilder.RenameIndex(
                name: "IX_SellerProfiles_UserId",
                table: "SellerProfile",
                newName: "IX_SellerProfile_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerProfile",
                table: "SellerProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerProfile_AspNetUsers_UserId",
                table: "SellerProfile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
