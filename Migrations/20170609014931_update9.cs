using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Partners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Partners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxId",
                table: "Partners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "Partners");
        }
    }
}
