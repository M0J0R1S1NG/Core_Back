using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GUID",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "SpecialInstructions",
                table: "Vehicles",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Vehicles",
                newName: "Make");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Vehicles",
                newName: "LicensePlate");

            migrationBuilder.RenameColumn(
                name: "GeocodedAddress",
                table: "Vehicles",
                newName: "InsurancePolicy");

            migrationBuilder.RenameColumn(
                name: "Address1",
                table: "Vehicles",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseProvince",
                table: "Drivers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseProvince",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Vehicles",
                newName: "SpecialInstructions");

            migrationBuilder.RenameColumn(
                name: "Make",
                table: "Vehicles",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "Vehicles",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "InsurancePolicy",
                table: "Vehicles",
                newName: "GeocodedAddress");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Vehicles",
                newName: "Address1");

            migrationBuilder.AddColumn<Guid>(
                name: "GUID",
                table: "Vehicles",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
