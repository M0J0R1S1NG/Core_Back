using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Migrations
{
    public partial class update17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "shipment",
                table: "DebitWay",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<string>(
                name: "errors",
                table: "DebitWay",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DebitWayNotifications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WebSiteId = table.Column<string>(nullable: true),
                    action = table.Column<string>(nullable: true),
                    additional_fee = table.Column<double>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    business = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    custom = table.Column<string>(nullable: true),
                    customer_errors_meaning = table.Column<string>(nullable: true),
                    discount_fee = table.Column<double>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    errors = table.Column<string>(nullable: true),
                    errors_meaning = table.Column<string>(nullable: true),
                    first_name = table.Column<string>(nullable: true),
                    gross = table.Column<double>(nullable: false),
                    identifier = table.Column<string>(nullable: true),
                    issuer_confirmation = table.Column<string>(nullable: true),
                    issuer_name = table.Column<string>(nullable: true),
                    item_code = table.Column<string>(nullable: true),
                    item_name = table.Column<string>(nullable: true),
                    language = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    merchant_transaction_id = table.Column<string>(nullable: true),
                    net = table.Column<double>(nullable: false),
                    phone = table.Column<string>(nullable: true),
                    processing_rate = table.Column<string>(nullable: true),
                    quantity = table.Column<double>(nullable: false),
                    result = table.Column<string>(nullable: true),
                    shipment = table.Column<string>(nullable: true),
                    shipping_address = table.Column<string>(nullable: true),
                    shipping_city = table.Column<string>(nullable: true),
                    shipping_country = table.Column<string>(nullable: true),
                    shipping_state_or_province = table.Column<string>(nullable: true),
                    shipping_zip_or_postal_code = table.Column<string>(nullable: true),
                    state_or_province = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    transaction_date = table.Column<DateTime>(nullable: false),
                    transaction_id = table.Column<string>(nullable: true),
                    transaction_result = table.Column<string>(nullable: true),
                    transaction_status = table.Column<string>(nullable: true),
                    transaction_type = table.Column<string>(nullable: true),
                    vericode = table.Column<string>(nullable: true),
                    zip_or_postal_code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitWayNotifications", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebitWayNotifications");

            migrationBuilder.DropColumn(
                name: "errors",
                table: "DebitWay");

            migrationBuilder.AlterColumn<bool>(
                name: "shipment",
                table: "DebitWay",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
