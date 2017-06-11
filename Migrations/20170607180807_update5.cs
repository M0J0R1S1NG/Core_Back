using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Migrations
{
    public partial class update5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryArea",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClosedTime = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Open = table.Column<bool>(nullable: false),
                    OpenTime = table.Column<DateTime>(nullable: false),
                    Partner = table.Column<Guid>(nullable: false),
                    Points = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryArea", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryArea");
        }
    }
}
