using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataModels.Migrations
{
    public partial class EFContextChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_models_tbl_ModelTableModelId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_models_tbl_DataSources_DataSourceTableDataSourceId",
                table: "models_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_scheduler_tbl_models_tbl_ModelId",
                table: "scheduler_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl");

            migrationBuilder.DropIndex(
                name: "IX_scheduler_tbl_ModelId",
                table: "scheduler_tbl");

            migrationBuilder.DropIndex(
                name: "IX_models_tbl_DataSourceTableDataSourceId",
                table: "models_tbl");

            migrationBuilder.DropIndex(
                name: "IX_DataSources_ModelTableModelId",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "scheduler_tbl");

            migrationBuilder.DropColumn(
                name: "DataSourceTableDataSourceId",
                table: "models_tbl");

            migrationBuilder.DropColumn(
                name: "ModelTableModelId",
                table: "DataSources");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl");

            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "scheduler_tbl",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DataSourceTableDataSourceId",
                table: "models_tbl",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModelTableModelId",
                table: "DataSources",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_scheduler_tbl_ModelId",
                table: "scheduler_tbl",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_models_tbl_DataSourceTableDataSourceId",
                table: "models_tbl",
                column: "DataSourceTableDataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_ModelTableModelId",
                table: "DataSources",
                column: "ModelTableModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSources_models_tbl_ModelTableModelId",
                table: "DataSources",
                column: "ModelTableModelId",
                principalTable: "models_tbl",
                principalColumn: "ModelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_models_tbl_DataSources_DataSourceTableDataSourceId",
                table: "models_tbl",
                column: "DataSourceTableDataSourceId",
                principalTable: "DataSources",
                principalColumn: "DataSourceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_scheduler_tbl_models_tbl_ModelId",
                table: "scheduler_tbl",
                column: "ModelId",
                principalTable: "models_tbl",
                principalColumn: "ModelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
