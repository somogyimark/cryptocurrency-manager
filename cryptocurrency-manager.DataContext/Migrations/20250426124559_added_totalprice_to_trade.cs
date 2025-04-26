﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cryptocurrency_manager.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class added_totalprice_to_trade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Transactions");
        }
    }
}
