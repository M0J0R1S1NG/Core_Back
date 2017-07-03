using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Migrations
{
    public partial class update34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateSent",
                table: "SMS",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DateRecieved",
                table: "SMS",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Orders",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Inventorys",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BestBefore",
                table: "Inventorys",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Franchise",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ContactDate",
                table: "Franchise",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "transaction_date",
                table: "DebitWayNotifications",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "time_stamp",
                table: "DebitWayNotifications",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "transaction_date_datetime",
                table: "DebitWayNotifications",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "transaction_date_datetime",
                table: "DebitWay",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "time_stamp",
                table: "DebitWay",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DoB",
                table: "Core_User",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "DriverBalances",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CreditOrDebit = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    DeliveryFeeCustomer = table.Column<double>(nullable: false),
                    DeliveryFeeSupplier = table.Column<double>(nullable: false),
                    DriverFlatRate = table.Column<double>(nullable: false),
                    DriverId = table.Column<int>(nullable: false),
                    DriverPercentageRate = table.Column<double>(nullable: false),
                    InventoryId = table.Column<int>(nullable: false),
                    LastChangeBy = table.Column<string>(nullable: true),
                    LastChangeDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    NetAmount = table.Column<double>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    PartnerId = table.Column<int>(nullable: false),
                    RunningBalance = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Taxes = table.Column<double>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    merchant_transaction_id = table.Column<Guid>(nullable: false),
                    quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverBalances", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverBalances");

            migrationBuilder.DropColumn(
                name: "transaction_date_datetime",
                table: "DebitWayNotifications");

            migrationBuilder.AlterColumn<string>(
                name: "DateSent",
                table: "SMS",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<string>(
                name: "DateRecieved",
                table: "SMS",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Inventorys",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BestBefore",
                table: "Inventorys",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Franchise",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ContactDate",
                table: "Franchise",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "transaction_date",
                table: "DebitWayNotifications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "time_stamp",
                table: "DebitWayNotifications",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "transaction_date_datetime",
                table: "DebitWay",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "time_stamp",
                table: "DebitWay",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DoB",
                table: "Core_User",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");
        }
    }
}
