using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "Inventorys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    DisLikes = table.Column<int>(nullable: false),
                    Likes = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Inventorys");
        }
    }
}
