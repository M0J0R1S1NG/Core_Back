using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Migrations
{
    public partial class update11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryAreaDrivers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeliverAreaIdID = table.Column<int>(nullable: true),
                    DriverIdID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAreaDrivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAreaDrivers_DeliveryArea_DeliverAreaIdID",
                        column: x => x.DeliverAreaIdID,
                        principalTable: "DeliveryArea",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryAreaDrivers_Drivers_DriverIdID",
                        column: x => x.DriverIdID,
                        principalTable: "Drivers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAreaDrivers_DeliverAreaIdID",
                table: "DeliveryAreaDrivers",
                column: "DeliverAreaIdID");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAreaDrivers_DriverIdID",
                table: "DeliveryAreaDrivers",
                column: "DriverIdID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAreaDrivers");
        }
    }
}
