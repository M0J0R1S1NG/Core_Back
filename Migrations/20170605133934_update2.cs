using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "partnerId",
                table: "Drivers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    DeliveryArea = table.Column<string>(nullable: true),
                    GUID = table.Column<Guid>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    SMSNumber = table.Column<string>(nullable: true),
                    ShippingAddress = table.Column<string>(nullable: true),
                    SpecialInstructions = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StreetAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropColumn(
                name: "partnerId",
                table: "Drivers");

            migrationBuilder.AlterColumn<decimal>(
                name: "PhoneNumber",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
