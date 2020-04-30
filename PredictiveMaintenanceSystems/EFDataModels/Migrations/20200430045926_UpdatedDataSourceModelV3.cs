using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataModels.Migrations
{
    public partial class UpdatedDataSourceModelV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalFilePath",
                table: "DataSources");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "DataSources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileContentDisposition",
                table: "DataSources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileContentType",
                table: "DataSources",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileLength",
                table: "DataSources",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "DataSources",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "FileContentDisposition",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "FileContentType",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "FileLength",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "DataSources");

            migrationBuilder.AddColumn<string>(
                name: "LocalFilePath",
                table: "DataSources",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
