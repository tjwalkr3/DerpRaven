using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DerpRaven.Api.Migrations;

public partial class RemoveProductIdFromOrder : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ProductId",
            table: "Orders");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ProductId",
            table: "Orders",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}

