using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cryptocurrency_manager.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class renamed_transaction_to_trade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transactions",
                newName: "TradeType");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transactions",
                newName: "TradeDate");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TradeType",
                table: "Transactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "TradeDate",
                table: "Transactions",
                newName: "TransactionDate");
        }
    }
}
