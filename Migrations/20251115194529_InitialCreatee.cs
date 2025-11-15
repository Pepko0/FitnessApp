using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessAppMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operators_OperatorRoles_RoleId",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Operators");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Operators",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Operators",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_Email",
                table: "Operators",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_OperatorRoles_RoleId",
                table: "Operators",
                column: "RoleId",
                principalTable: "OperatorRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operators_OperatorRoles_RoleId",
                table: "Operators");

            migrationBuilder.DropIndex(
                name: "IX_Operators_Email",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Operators");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Operators",
                type: "TEXT",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Operators",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_OperatorRoles_RoleId",
                table: "Operators",
                column: "RoleId",
                principalTable: "OperatorRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
