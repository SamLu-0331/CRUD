using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Added_DoctorId_To_Patient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "AppPatients",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppPatients_DoctorId",
                table: "AppPatients",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPatients_AppDoctors_DoctorId",
                table: "AppPatients",
                column: "DoctorId",
                principalTable: "AppDoctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPatients_AppDoctors_DoctorId",
                table: "AppPatients");

            migrationBuilder.DropIndex(
                name: "IX_AppPatients_DoctorId",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "AppPatients");
        }
    }
}
