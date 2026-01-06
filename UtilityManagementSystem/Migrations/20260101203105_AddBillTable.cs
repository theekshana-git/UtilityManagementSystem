using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilityManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDate",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BillDate",
                table: "Bill",
                type: "date",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "PaymentDate",
                table: "Payment",
                type: "date",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BillDate",
                table: "Bill",
                type: "date",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "(getdate())");
        }
    }
}
