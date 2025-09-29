using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaExpiracionToken",
                table: "AppUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenResetPassword",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "profesor" },
                    { 2, "alumno" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_Email",
                table: "AppUser",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUser_Email",
                table: "AppUser");

            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "FechaExpiracionToken",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "TokenResetPassword",
                table: "AppUser");
        }
    }
}
