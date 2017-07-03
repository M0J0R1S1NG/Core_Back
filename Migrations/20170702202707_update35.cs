using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "DriverBalances");

            migrationBuilder.AddColumn<Guid>(
                name: "CunstomerGuid",
                table: "DriverBalances",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CunstomerGuid",
                table: "DriverBalances");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "DriverBalances",
                nullable: false,
                defaultValue: 0);
        }
    }
}
