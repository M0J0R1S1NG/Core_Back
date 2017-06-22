using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class update21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WebSiteId",
                table: "DebitWay",
                newName: "website_unique_id");

            migrationBuilder.AddColumn<string>(
                name: "return_url",
                table: "DebitWay",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "return_url",
                table: "DebitWay");

            migrationBuilder.RenameColumn(
                name: "website_unique_id",
                table: "DebitWay",
                newName: "WebSiteId");
        }
    }
}
