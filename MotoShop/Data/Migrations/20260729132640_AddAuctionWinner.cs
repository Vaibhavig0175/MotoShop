using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuctionWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_AspNetUsers_WinnerId",
                table: "Auctions");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedOn",
                table: "Auctions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WinningBidAmount",
                table: "Auctions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_AspNetUsers_WinnerId",
                table: "Auctions",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_AspNetUsers_WinnerId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "ClosedOn",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "WinningBidAmount",
                table: "Auctions");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_AspNetUsers_WinnerId",
                table: "Auctions",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
