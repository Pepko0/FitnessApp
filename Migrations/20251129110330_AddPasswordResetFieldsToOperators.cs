using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessAppMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFieldsToOperators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChangePassword",
                table: "Operators",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Operators",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpires",
                table: "Operators",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangePassword",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpires",
                table: "Operators");
        }
    }
}
