using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataModels.Migrations
{
    public partial class UpdatedDataSourceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "DataSources",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStreaming",
                table: "DataSources",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "IsStreaming",
                table: "DataSources");
        }
    }
}
