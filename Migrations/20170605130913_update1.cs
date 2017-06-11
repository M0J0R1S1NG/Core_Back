using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CostPerGram",
                table: "Inventorys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Inventorys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageFilePath",
                table: "Inventorys",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PricePerGram",
                table: "Inventorys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PricePerOz",
                table: "Inventorys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PricePerQuarter",
                table: "Inventorys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PricePerhalf",
                table: "Inventorys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Qualities",
                table: "Inventorys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UPC",
                table: "Inventorys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "catagory",
                table: "Inventorys",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPerGram",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "ImageFilePath",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "PricePerGram",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "PricePerOz",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "PricePerQuarter",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "PricePerhalf",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "Qualities",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "UPC",
                table: "Inventorys");

            migrationBuilder.DropColumn(
                name: "catagory",
                table: "Inventorys");
        }
    }
}
