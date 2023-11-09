using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class Update_User_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "AbpUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Job",
                table: "AbpUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "AbpUsers");
        }
    }
}
