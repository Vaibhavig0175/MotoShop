using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerSettlement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSettled",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SettledById",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SettlementDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SettledById",
                table: "Payments",
                column: "SettledById");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_SettledById",
                table: "Payments",
                column: "SettledById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_SettledById",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SettledById",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsSettled",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SettledById",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SettlementDate",
                table: "Payments");
        }
    }
}
