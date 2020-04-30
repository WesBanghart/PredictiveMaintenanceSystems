using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataModels.Migrations
{
    public partial class UpdatedDataSourceModelV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "DataSources");

            migrationBuilder.AddColumn<string>(
                name: "LocalFilePath",
                table: "DataSources",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalFilePath",
                table: "DataSources");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "DataSources",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
