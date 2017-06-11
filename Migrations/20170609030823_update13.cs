using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleGuid",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "VehicleGuid",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Drivers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Drivers");

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleGuid",
                table: "Vehicles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GUID",
                table: "Drivers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleGuid",
                table: "Drivers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GUID",
                table: "Addresses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
