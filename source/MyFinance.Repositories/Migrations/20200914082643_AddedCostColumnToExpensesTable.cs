using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFinance.Repositories.Migrations
{
    public partial class AddedCostColumnToExpensesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Expenses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Expenses");
        }
    }
}
