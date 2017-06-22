using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "time_stamp",
                table: "DebitWayNotifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "DebitWay",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "time_stamp",
                table: "DebitWay",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_stamp",
                table: "DebitWayNotifications");

            migrationBuilder.DropColumn(
                name: "status",
                table: "DebitWay");

            migrationBuilder.DropColumn(
                name: "time_stamp",
                table: "DebitWay");
        }
    }
}
