using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cryptocurrency_manager.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Assets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Assets");
        }
    }
}
