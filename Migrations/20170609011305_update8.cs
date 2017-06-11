using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Drivers_DriverID",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryArea_Drivers_DriverID",
                table: "DeliveryArea");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Franchise_FranchiseId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Partners_PartnerId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Core_User_UserAppUserId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_DriverID",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_DriverID",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_FranchiseId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_PartnerId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_UserAppUserId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryArea_DriverID",
                table: "DeliveryArea");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_DriverID",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "FranchiseId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "DeliveryArea");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "UserAppUserId",
                table: "Drivers",
                newName: "Address1");

            migrationBuilder.AlterColumn<string>(
                name: "Address1",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "Drivers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleGuid",
                table: "Drivers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "VehicleGuid",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "Address1",
                table: "Drivers",
                newName: "UserAppUserId");

            migrationBuilder.AddColumn<int>(
                name: "DriverID",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserAppUserId",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchiseId",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriverID",
                table: "DeliveryArea",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriverID",
                table: "Addresses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverID",
                table: "Vehicles",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_FranchiseId",
                table: "Drivers",
                column: "FranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_PartnerId",
                table: "Drivers",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_UserAppUserId",
                table: "Drivers",
                column: "UserAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryArea_DriverID",
                table: "DeliveryArea",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DriverID",
                table: "Addresses",
                column: "DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Drivers_DriverID",
                table: "Addresses",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryArea_Drivers_DriverID",
                table: "DeliveryArea",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Franchise_FranchiseId",
                table: "Drivers",
                column: "FranchiseId",
                principalTable: "Franchise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Partners_PartnerId",
                table: "Drivers",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Core_User_UserAppUserId",
                table: "Drivers",
                column: "UserAppUserId",
                principalTable: "Core_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_DriverID",
                table: "Vehicles",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
